using Application.Services.Interfaces;
using Domain.Entities.Account;
using Domain.Enums;
using Domain.Interface;
using Domain.ViewModel.AddWallet;
using Transaction = Domain.Entities.Account.Transaction;

namespace Application.Services.Impelementation;

public class TransactionService(
    ITransactionRepository transactionRepository,
    IOrderRepository orderRepository) : ITransactionService
{
    public async Task<int> AddNewTransaction(int userid, int price)
    {
        var newTranaction = new Transaction
        {
            CreateDate = DateTime.Now,
            IsDeleted = false,
            Price = price,
            IsPay = false,
            TransactionType = TransactionType.Deposit,
            UserId = userid
        };

        await transactionRepository.AddTransaction(newTranaction);
        await transactionRepository.Save();
        return newTranaction.Id;
    }

    public async Task AddTransactionFromWallet(AddWalletViewModel model)
    {
        var transaction = new Transaction
        {
            CreateDate = DateTime.Now,
            UserId = model.UserId,
            IsDeleted = false,
            IsPay = false,
            TransactionType = TransactionType.Deposit,
            Price = model.Amount
        };
        await transactionRepository.AddTransaction(transaction);
        await transactionRepository.Save();
    }

    public async Task<Transaction> GetTransactionByid(int transId)
    {
        return await transactionRepository.GetTransactionById(transId);
    }

    public async Task<int> GetUserBalanceTransaction(int userId)
    {
        var transActions = await transactionRepository.GetUserTransaction(userId);
        var deposit = transActions.Where(u => u.TransactionType == TransactionType.Deposit && u.IsPay)
            .Sum(u => u.Price);
        var withDraw = transActions.Where(u => u.TransactionType == TransactionType.WithDraw && u.IsPay)
            .Sum(u => u.Price);
        return deposit - withDraw;
    }

    public async Task<WalletStatusBalance> GetUserBalanceWallet(int userId, int amount)
    {
        var WalletBalance = await GetUserBalanceTransaction(userId);
        if (WalletBalance >= amount)
        {
            var newTrans = new Transaction
            {
                CreateDate = DateTime.Now,
                IsDeleted = false,
                IsPay = true,
                Price = amount,
                TransactionType = TransactionType.WithDraw,
                UserId = userId
            };
            await transactionRepository.AddTransaction(newTrans);
            var openUserOrder = await orderRepository.GetUserLatestOpenOrder(userId);
            openUserOrder.IsFinally = true;
            openUserOrder.PaymentDate = DateTime.Now;
            orderRepository.UpdateOrder(openUserOrder);

            await transactionRepository.Save();
            return WalletStatusBalance.IsOkay;
        }

        return WalletStatusBalance.NoneBalance;
    }
}