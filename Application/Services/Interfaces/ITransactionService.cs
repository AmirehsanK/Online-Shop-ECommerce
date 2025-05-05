using Domain.Entities.Account;
using Domain.Enums;
using Domain.ViewModel.AddWallet;

namespace Application.Services.Interfaces;

public interface ITransactionService
{
    Task<int> AddNewTransaction(int userid, int price);

    Task AddTransactionFromWallet(AddWalletViewModel model);

    Task<Transaction> GetTransactionByid(int transId);

    Task<int> GetUserBalanceTransaction(int userId);

    Task<WalletStatusBalance> GetUserBalanceWallet(int userId, int amount);
}