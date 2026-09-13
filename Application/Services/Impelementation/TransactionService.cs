using Application.Services.Interfaces;

namespace Application.Services.Impelementation;

/// <summary>
/// Read-only wallet queries for views. Anything that creates or completes a payment lives
/// in <see cref="CheckoutService"/>.
/// </summary>
public class TransactionService(ICheckoutService checkoutService) : ITransactionService
{
    public Task<int> GetUserBalanceTransaction(int userId) => checkoutService.GetWalletBalanceAsync(userId);
}
