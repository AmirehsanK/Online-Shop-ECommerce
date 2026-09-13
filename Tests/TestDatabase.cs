using Application.Services.Impelementation;
using Application.Services.Interfaces;
using Domain.Entities.Account;
using Domain.Entities.Orders;
using Domain.Entities.Product;
using Infra.Data.Context;
using Infra.Data.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Tests;

/// <summary>
/// In-memory SQLite database built from the real model (seed permissions and categories
/// included). It lives as long as this object; every context shares the one connection.
/// </summary>
public sealed class TestDatabase : IDisposable
{
    private readonly SqliteConnection _connection = new("Data Source=:memory:");

    public TestDatabase()
    {
        _connection.Open();
        using var context = NewContext();
        context.Database.EnsureCreated();
    }

    public ApplicationDbContext NewContext() =>
        new(new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(_connection).Options);

    public static CheckoutService Checkout(ApplicationDbContext context, IPaymentGateway? gateway = null) =>
        new(new OrderRepository(context), new TransactionRepository(context), gateway ?? new FakeGateway());

    public static OrderService Orders(ApplicationDbContext context) =>
        new(new OrderRepository(context), new ProductColorRepository(context), new UserRepository(context));

    public async Task<User> AddUserAsync(string email, string passwordHash = "not-a-real-hash", bool isAdmin = false)
    {
        await using var context = NewContext();
        var user = new User
        {
            Email = email,
            FirstName = "Test",
            LastName = "User",
            PhoneNumber = "09120000000",
            Password = passwordHash,
            EmailActiveCode = Guid.NewGuid().ToString("N"),
            IsEmailActive = true,
            IsAdmin = isAdmin,
            CreateDate = DateTime.Now
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return user;
    }

    /// <summary>A product with one colour variant. Category 6 comes from the model's seed data.</summary>
    public async Task<(int ProductId, int ColorId)> AddProductAsync(int price, int stock, int colorExtraPrice = 0)
    {
        await using var context = NewContext();
        var color = new ProductColor
        {
            Count = stock,
            Price = colorExtraPrice,
            CreateDate = DateTime.Now,
            Color = new Color { Title = "Black", ColorCode = "#000", CreateDate = DateTime.Now }
        };
        var product = new Product
        {
            ProductName = "Phone",
            ShortDescription = "A phone",
            ImageName = "phone.jpg",
            Price = price,
            CategoryId = 6,
            CreateDate = DateTime.Now,
            ProductColors = [color]
        };
        context.Products.Add(product);
        await context.SaveChangesAsync();
        return (product.Id, color.Id);
    }

    public async Task DepositAsync(int userId, int amount)
    {
        await using var context = NewContext();
        context.Transactions.Add(new Transaction
        {
            UserId = userId,
            Price = amount,
            TransactionType = TransactionType.Deposit,
            IsPay = true,
            CreateDate = DateTime.Now
        });
        await context.SaveChangesAsync();
    }

    public async Task<int> AddToBasketAsync(int userId, int productId, int colorId, int times = 1)
    {
        for (var i = 0; i < times; i++)
        {
            await using var context = NewContext();
            await Orders(context).AddProductToOrder(productId, userId, colorId);
        }

        await using var check = NewContext();
        return (await check.Orders.SingleAsync(o => o.UserId == userId && !o.IsFinally)).Id;
    }

    public async Task<int> BalanceAsync(int userId)
    {
        await using var context = NewContext();
        return await Checkout(context).GetWalletBalanceAsync(userId);
    }

    public async Task<Order> OrderAsync(int orderId)
    {
        await using var context = NewContext();
        return await context.Orders.AsNoTracking().SingleAsync(o => o.Id == orderId);
    }

    public void Dispose() => _connection.Dispose();

    public sealed class FakeGateway(bool verifies = true) : IPaymentGateway
    {
        public int Verifications { get; private set; }

        public Task<string?> RequestPaymentAsync(int transactionId, int amount, string description, string callbackUrl) =>
            Task.FromResult<string?>("https://gateway.test/pay");

        public Task<bool> VerifyPaymentAsync(string authority, int amount)
        {
            Verifications++;
            return Task.FromResult(verifies);
        }
    }
}
