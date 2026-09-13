using Application.Services.Interfaces;
using Domain.Entities.Account;
using Microsoft.EntityFrameworkCore;

namespace Tests;

public class CheckoutServiceTests
{
    [Fact]
    public async Task Wallet_payment_charges_the_basket_total_worked_out_on_the_server()
    {
        using var db = new TestDatabase();
        var user = await db.AddUserAsync("buyer@test");
        var (productId, colorId) = await db.AddProductAsync(price: 300, stock: 5, colorExtraPrice: 50);
        await db.DepositAsync(user.Id, 1_000);
        var orderId = await db.AddToBasketAsync(user.Id, productId, colorId, times: 2); // (300 + 50) * 2

        await using (var context = db.NewContext())
            Assert.Equal(WalletCheckoutResult.Paid, await TestDatabase.Checkout(context).PayBasketFromWalletAsync(user.Id));

        Assert.Equal(300, await db.BalanceAsync(user.Id));
        Assert.True((await db.OrderAsync(orderId)).IsFinally);
    }

    [Fact]
    public async Task Wallet_payment_is_refused_when_the_balance_is_short_and_nothing_changes()
    {
        using var db = new TestDatabase();
        var user = await db.AddUserAsync("buyer@test");
        var (productId, colorId) = await db.AddProductAsync(price: 900, stock: 5);
        await db.DepositAsync(user.Id, 500);
        var orderId = await db.AddToBasketAsync(user.Id, productId, colorId);

        await using (var context = db.NewContext())
            Assert.Equal(WalletCheckoutResult.InsufficientBalance, await TestDatabase.Checkout(context).PayBasketFromWalletAsync(user.Id));

        Assert.Equal(500, await db.BalanceAsync(user.Id));
        Assert.False((await db.OrderAsync(orderId)).IsFinally);
    }

    [Fact]
    public async Task An_empty_basket_cannot_be_paid()
    {
        using var db = new TestDatabase();
        var user = await db.AddUserAsync("buyer@test");
        await db.DepositAsync(user.Id, 500);

        await using var context = db.NewContext();
        Assert.Equal(WalletCheckoutResult.EmptyBasket, await TestDatabase.Checkout(context).PayBasketFromWalletAsync(user.Id));
        Assert.Null(await TestDatabase.Checkout(context).StartBasketPaymentAsync(user.Id));
    }

    /// <summary>The repository used to return every user's transactions for any user id.</summary>
    [Fact]
    public async Task A_wallet_balance_only_counts_that_users_own_transactions()
    {
        using var db = new TestDatabase();
        var rich = await db.AddUserAsync("rich@test");
        var buyer = await db.AddUserAsync("buyer@test");
        await db.DepositAsync(rich.Id, 1_000_000);
        var (productId, colorId) = await db.AddProductAsync(price: 100, stock: 5);
        await db.AddToBasketAsync(buyer.Id, productId, colorId);

        Assert.Equal(0, await db.BalanceAsync(buyer.Id));
        await using var context = db.NewContext();
        Assert.Equal(WalletCheckoutResult.InsufficientBalance, await TestDatabase.Checkout(context).PayBasketFromWalletAsync(buyer.Id));
    }

    [Fact]
    public async Task Paying_a_basket_by_card_closes_the_order_and_leaves_the_wallet_unchanged()
    {
        using var db = new TestDatabase();
        var user = await db.AddUserAsync("buyer@test");
        var (productId, colorId) = await db.AddProductAsync(price: 400, stock: 5);
        await db.DepositAsync(user.Id, 100);
        var orderId = await db.AddToBasketAsync(user.Id, productId, colorId);

        PaymentStart start;
        await using (var context = db.NewContext())
            start = (await TestDatabase.Checkout(context).StartBasketPaymentAsync(user.Id))!;
        Assert.Equal(400, start.Amount);

        await using (var context = db.NewContext())
            Assert.Equal(PaymentCompletionResult.OrderPaid, await TestDatabase.Checkout(context).CompletePaymentAsync(start.TransactionId, "auth"));

        Assert.True((await db.OrderAsync(orderId)).IsFinally);
        Assert.Equal(100, await db.BalanceAsync(user.Id));
    }

    /// <summary>The old callback closed whatever was in the basket, whatever the payment was for.</summary>
    [Fact]
    public async Task Topping_up_the_wallet_does_not_mark_the_basket_as_paid()
    {
        using var db = new TestDatabase();
        var user = await db.AddUserAsync("buyer@test");
        var (productId, colorId) = await db.AddProductAsync(price: 400, stock: 5);
        var orderId = await db.AddToBasketAsync(user.Id, productId, colorId);

        PaymentStart start;
        await using (var context = db.NewContext())
            start = (await TestDatabase.Checkout(context).StartWalletTopUpAsync(user.Id, 50))!;

        await using (var context = db.NewContext())
            Assert.Equal(PaymentCompletionResult.WalletCharged, await TestDatabase.Checkout(context).CompletePaymentAsync(start.TransactionId, "auth"));

        Assert.False((await db.OrderAsync(orderId)).IsFinally);
        Assert.Equal(50, await db.BalanceAsync(user.Id));
    }

    [Fact]
    public async Task A_repeated_callback_does_not_complete_the_payment_twice()
    {
        using var db = new TestDatabase();
        var user = await db.AddUserAsync("buyer@test");
        var (productId, colorId) = await db.AddProductAsync(price: 400, stock: 5);
        await db.AddToBasketAsync(user.Id, productId, colorId);

        PaymentStart start;
        await using (var context = db.NewContext())
            start = (await TestDatabase.Checkout(context).StartBasketPaymentAsync(user.Id))!;

        await using (var context = db.NewContext())
            await TestDatabase.Checkout(context).CompletePaymentAsync(start.TransactionId, "auth");
        await using (var context = db.NewContext())
            Assert.Equal(PaymentCompletionResult.AlreadyCompleted, await TestDatabase.Checkout(context).CompletePaymentAsync(start.TransactionId, "auth"));

        await using var check = db.NewContext();
        Assert.Equal(1, await check.Transactions.CountAsync(t => t.UserId == user.Id && t.TransactionType == TransactionType.WithDraw));
    }

    [Fact]
    public async Task A_payment_the_gateway_does_not_confirm_is_not_completed()
    {
        using var db = new TestDatabase();
        var user = await db.AddUserAsync("buyer@test");
        var (productId, colorId) = await db.AddProductAsync(price: 400, stock: 5);
        var orderId = await db.AddToBasketAsync(user.Id, productId, colorId);

        PaymentStart start;
        await using (var context = db.NewContext())
            start = (await TestDatabase.Checkout(context).StartBasketPaymentAsync(user.Id))!;

        await using (var context = db.NewContext())
            Assert.Equal(PaymentCompletionResult.Failed,
                await TestDatabase.Checkout(context, new TestDatabase.FakeGateway(verifies: false)).CompletePaymentAsync(start.TransactionId, "auth"));

        Assert.False((await db.OrderAsync(orderId)).IsFinally);
        Assert.Equal(0, await db.BalanceAsync(user.Id));
    }

    [Fact]
    public async Task If_the_basket_changes_during_card_payment_the_money_goes_to_the_wallet()
    {
        using var db = new TestDatabase();
        var user = await db.AddUserAsync("buyer@test");
        var (productId, colorId) = await db.AddProductAsync(price: 400, stock: 5);
        var orderId = await db.AddToBasketAsync(user.Id, productId, colorId);

        PaymentStart start;
        await using (var context = db.NewContext())
            start = (await TestDatabase.Checkout(context).StartBasketPaymentAsync(user.Id))!;

        await db.AddToBasketAsync(user.Id, productId, colorId); // now 800, but 400 was paid

        await using (var context = db.NewContext())
            Assert.Equal(PaymentCompletionResult.CreditedToWallet, await TestDatabase.Checkout(context).CompletePaymentAsync(start.TransactionId, "auth"));

        Assert.False((await db.OrderAsync(orderId)).IsFinally);
        Assert.Equal(400, await db.BalanceAsync(user.Id));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-500)]
    public async Task A_top_up_must_be_a_positive_amount(int amount)
    {
        using var db = new TestDatabase();
        var user = await db.AddUserAsync("buyer@test");

        await using var context = db.NewContext();
        Assert.Null(await TestDatabase.Checkout(context).StartWalletTopUpAsync(user.Id, amount));
    }
}
