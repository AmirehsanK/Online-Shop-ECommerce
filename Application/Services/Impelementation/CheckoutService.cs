using Application.Services.Interfaces;
using Domain.Entities.Account;
using Domain.Entities.Orders;
using Domain.Interface;

namespace Application.Services.Impelementation;

public class CheckoutService(
    IOrderRepository orderRepository,
    ITransactionRepository transactionRepository,
    IPaymentGateway paymentGateway) : ICheckoutService
{
    public static int TotalOf(Order order) =>
        order.OrderDetails
            .Where(d => !d.IsDeleted)
            .Sum(d => (d.Product.Price + d.ColorPrice) * d.Count);

    public async Task<int> GetBasketTotalAsync(int userId)
    {
        var basket = await orderRepository.GetUserBasketDetail(userId);
        return basket == null ? 0 : TotalOf(basket);
    }

    public async Task<int> GetWalletBalanceAsync(int userId)
    {
        var transactions = await transactionRepository.GetUserTransaction(userId);
        var paid = transactions.Where(t => t.IsPay).ToList();
        return paid.Where(t => t.TransactionType == TransactionType.Deposit).Sum(t => t.Price)
               - paid.Where(t => t.TransactionType == TransactionType.WithDraw).Sum(t => t.Price);
    }

    public async Task<WalletCheckoutResult> PayBasketFromWalletAsync(int userId)
    {
        var basket = await orderRepository.GetUserBasketDetail(userId);
        var total = basket == null ? 0 : TotalOf(basket);
        if (total <= 0)
            return WalletCheckoutResult.EmptyBasket;

        if (await GetWalletBalanceAsync(userId) < total)
            return WalletCheckoutResult.InsufficientBalance;

        await transactionRepository.AddTransaction(Withdrawal(userId, total, basket!.Id));
        Close(basket);
        orderRepository.UpdateOrder(basket);

        // One SaveChanges: the withdrawal and the closed order commit together or not at all.
        await transactionRepository.Save();
        return WalletCheckoutResult.Paid;
    }

    public async Task<PaymentStart?> StartBasketPaymentAsync(int userId)
    {
        var basket = await orderRepository.GetUserBasketDetail(userId);
        var total = basket == null ? 0 : TotalOf(basket);
        if (total <= 0)
            return null;

        var transaction = PendingDeposit(userId, total, basket!.Id);
        await transactionRepository.AddTransaction(transaction);
        await transactionRepository.Save();
        return new PaymentStart(transaction.Id, total);
    }

    public async Task<PaymentStart?> StartWalletTopUpAsync(int userId, int amount)
    {
        if (amount <= 0)
            return null;

        var transaction = PendingDeposit(userId, amount, orderId: null);
        await transactionRepository.AddTransaction(transaction);
        await transactionRepository.Save();
        return new PaymentStart(transaction.Id, amount);
    }

    public async Task<PaymentCompletionResult> CompletePaymentAsync(int transactionId, string authority)
    {
        var transaction = await transactionRepository.GetTransactionById(transactionId);
        if (transaction == null || transaction.TransactionType != TransactionType.Deposit)
            return PaymentCompletionResult.NotFound;

        // Gateways retry callbacks and people press back. Completing twice must not close an
        // order twice or credit a wallet twice.
        if (transaction.IsPay)
            return PaymentCompletionResult.AlreadyCompleted;

        // Verify the amount we recorded when the payment started, not anything in the request.
        if (string.IsNullOrEmpty(authority) || !await paymentGateway.VerifyPaymentAsync(authority, transaction.Price))
            return PaymentCompletionResult.Failed;

        transaction.IsPay = true;
        transactionRepository.UpdateTransaction(transaction);

        var result = PaymentCompletionResult.WalletCharged;
        if (transaction.OrderId is { } orderId)
        {
            var order = await orderRepository.GetOrderWithDetailsAsync(orderId);
            if (order is { IsFinally: false } && order.UserId == transaction.UserId && TotalOf(order) == transaction.Price)
            {
                // The paid deposit and this withdrawal cancel out, so paying by card leaves the
                // wallet balance where it was. Without it every card purchase also topped up the
                // wallet by the same amount.
                await transactionRepository.AddTransaction(Withdrawal(transaction.UserId, transaction.Price, orderId));
                Close(order);
                orderRepository.UpdateOrder(order);
                result = PaymentCompletionResult.OrderPaid;
            }
            else
            {
                // The basket was edited (or paid another way) while the customer was at the bank.
                // They paid for something other than what is in it now, so keep the money as
                // wallet credit rather than closing an order with different contents.
                result = PaymentCompletionResult.CreditedToWallet;
            }
        }

        await transactionRepository.Save();
        return result;
    }

    private static Transaction PendingDeposit(int userId, int amount, int? orderId) => new()
    {
        UserId = userId,
        Price = amount,
        TransactionType = TransactionType.Deposit,
        IsPay = false,
        OrderId = orderId,
        CreateDate = DateTime.Now
    };

    private static Transaction Withdrawal(int userId, int amount, int orderId) => new()
    {
        UserId = userId,
        Price = amount,
        TransactionType = TransactionType.WithDraw,
        IsPay = true,
        OrderId = orderId,
        CreateDate = DateTime.Now
    };

    private static void Close(Order order)
    {
        order.IsFinally = true;
        order.PaymentDate = DateTime.Now;
    }
}
