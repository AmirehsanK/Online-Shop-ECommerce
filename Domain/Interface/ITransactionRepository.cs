using Domain.Entities.Account;

namespace Domain.Interface;

public interface ITransactionRepository
{
    Task AddTransaction(Transaction transaction);

    void UpdateTransaction(Transaction transaction);

    Task<Transaction> GetTransactionById(int transactionId);
    Task Save();
    Task<List<Transaction>> GetUserTransaction(int userId);
}