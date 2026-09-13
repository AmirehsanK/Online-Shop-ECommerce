using Domain.Entities.Account;
using Domain.Interface;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repositories;

public class TransactionRepository(ApplicationDbContext context) : ITransactionRepository
{
    public async Task AddTransaction(Transaction transaction)
    {
        await context.Transactions.AddAsync(transaction);
    }

    public void UpdateTransaction(Transaction transaction)
    {
        context.Update(transaction);
    }

    public async Task<Transaction> GetTransactionById(int transactionId)
    {
        return (await context.Transactions.FindAsync(transactionId))!;
    }

    public async Task Save()
    {
        await context.SaveChangesAsync();
    }

    public async Task<List<Transaction>> GetUserTransaction(int userId)
    {
        // Filtered by user: this used to return every customer's transactions, so each wallet
        // balance was the whole store's deposits minus withdrawals.
        return await context.Transactions.Where(t => t.UserId == userId && !t.IsDeleted).ToListAsync();
    }

    public async Task<int> GetTotalSalesAsync()
    {
        return await context.Transactions
            .Where(t => t.TransactionType == TransactionType.WithDraw && t.IsPay && t.OrderId != null && !t.IsDeleted)
            .SumAsync(t => t.Price);
    }
}