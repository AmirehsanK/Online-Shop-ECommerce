namespace Application.Services.Interfaces;

public interface ITransactionService
{
    Task<int> GetUserBalanceTransaction(int userId);
}
