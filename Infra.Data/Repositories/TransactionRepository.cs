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
        return await context.Transactions.FindAsync(transactionId);
    }

    public async Task Save()
    {
        await context.SaveChangesAsync();
    }

    public async Task<List<Transaction>> GetUserTransaction(int userId)
    {
        return await context.Transactions.ToListAsync();
    }
}