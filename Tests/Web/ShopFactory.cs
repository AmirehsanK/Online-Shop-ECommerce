using System.Net;
using System.Text.RegularExpressions;
using Application.Services.Interfaces;
using Domain.Entities.Account;
using Infra.Data.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Web.DemoData;

namespace Tests.Web;

/// <summary>
/// The whole site in memory over a throwaway SQLite file, seeded with the demo admin.
/// reCAPTCHA is off and payments use the built-in demo gateway, so tests drive the same
/// controllers, filters and views a browser would.
/// </summary>
public sealed class ShopFactory : WebApplicationFactory<Program>
{
    public const string AdminPassword = "Admin#Test123";
    public const string CustomerPassword = "Customer#Test123";

    private readonly string _databasePath = Path.Combine(Path.GetTempPath(), $"shop-tests-{Guid.NewGuid():N}.db");

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.UseSetting("Database:Provider", "Sqlite");
        builder.UseSetting("ConnectionStrings:DefaultConnection", $"Data Source={_databasePath}");
        builder.UseSetting("Database:SeedDemoData", "true");
        builder.UseSetting("DemoData:AdminPassword", AdminPassword);
        builder.UseSetting("GoogleRecaptcha:Enabled", "false");
        builder.UseSetting("Payment:Provider", "Demo");
        builder.UseSetting("ApplicationSettings:DomainLink", "http://localhost");
    }

    public HttpClient NewBrowser() => CreateClient(new WebApplicationFactoryClientOptions
    {
        AllowAutoRedirect = false,
        HandleCookies = true
    });

    public async Task<T> WithDbAsync<T>(Func<ApplicationDbContext, Task<T>> action)
    {
        using var scope = Services.CreateScope();
        return await action(scope.ServiceProvider.GetRequiredService<ApplicationDbContext>());
    }

    /// <summary>A fresh, activated customer with the given wallet balance, so tests don't share state.</summary>
    public async Task<User> CreateCustomerAsync(int walletBalance = 0)
    {
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        var user = new User
        {
            Email = $"customer-{Guid.NewGuid():N}@test",
            FirstName = "Test",
            LastName = "Customer",
            PhoneNumber = "09120000000",
            Password = await hasher.EncodePasswordAsync(CustomerPassword),
            EmailActiveCode = Guid.NewGuid().ToString("N"),
            IsEmailActive = true,
            CreateDate = DateTime.Now
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();
        if (walletBalance > 0)
        {
            context.Transactions.Add(new Transaction
            {
                UserId = user.Id, Price = walletBalance, TransactionType = TransactionType.Deposit, IsPay = true, CreateDate = DateTime.Now
            });
            await context.SaveChangesAsync();
        }

        return user;
    }

    public Task<int> AdminIdAsync() =>
        WithDbAsync(db => db.Users.Where(u => u.Email == DemoDataSeeder.AdminEmail).Select(u => u.Id).SingleAsync());

    public static async Task SignInAsync(HttpClient browser, string email, string password)
    {
        var response = await browser.PostAsync("/login", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["Email"] = email,
            ["Password"] = password
        }));
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
    }

    public static async Task<string> AntiforgeryTokenFromAsync(HttpClient browser, string url)
    {
        var html = await browser.GetStringAsync(url);
        var match = Regex.Match(html, "name=\"__RequestVerificationToken\" type=\"hidden\" value=\"([^\"]+)\"");
        Assert.True(match.Success, $"No antiforgery token on {url}");
        return match.Groups[1].Value;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        Microsoft.Data.Sqlite.SqliteConnection.ClearAllPools();
        foreach (var file in new[] { _databasePath, _databasePath + "-shm", _databasePath + "-wal" })
            try { File.Delete(file); } catch (IOException) { }
    }
}
