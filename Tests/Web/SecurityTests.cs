using System.Net;
using Domain.Entities.Account;
using Microsoft.EntityFrameworkCore;
using Web.DemoData;

namespace Tests.Web;

/// <summary>
/// Each test reproduces a hole that existed before, through real HTTP requests.
/// </summary>
public class SecurityTests(ShopFactory shop) : IClassFixture<ShopFactory>
{
    [Theory]
    [InlineData("/Admin")]
    [InlineData("/Admin/User/UserList")]
    [InlineData("/Admin/Order/UserOrdersList")]
    public async Task Anonymous_visitors_are_sent_to_sign_in_from_admin_pages(string url)
    {
        var response = await shop.NewBrowser().GetAsync(url);

        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.Contains("/Login", response.Headers.Location!.OriginalString, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// The permission filter used to write a redirect but let the action run, so any
    /// signed-in customer could delete users, edit products, and so on.
    /// </summary>
    [Fact]
    public async Task A_customer_cannot_run_an_admin_action()
    {
        var customer = await shop.CreateCustomerAsync();
        var adminId = await shop.AdminIdAsync();
        var browser = shop.NewBrowser();
        await ShopFactory.SignInAsync(browser, customer.Email, ShopFactory.CustomerPassword);

        var response = await browser.GetAsync($"/Admin/User/DeleteUser?userId={adminId}");

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        Assert.False(await shop.WithDbAsync(db => db.Users.Where(u => u.Id == adminId).Select(u => u.IsDeleted).SingleAsync()));
    }

    /// <summary>The dashboard had no permission check at all and showed tickets and messages to anyone.</summary>
    [Fact]
    public async Task The_admin_dashboard_is_forbidden_to_customers_and_open_to_the_administrator()
    {
        var customer = await shop.CreateCustomerAsync();
        var customerBrowser = shop.NewBrowser();
        await ShopFactory.SignInAsync(customerBrowser, customer.Email, ShopFactory.CustomerPassword);
        Assert.Equal(HttpStatusCode.Forbidden, (await customerBrowser.GetAsync("/Admin")).StatusCode);

        var adminBrowser = shop.NewBrowser();
        await ShopFactory.SignInAsync(adminBrowser, DemoDataSeeder.AdminEmail, ShopFactory.AdminPassword);
        Assert.Equal(HttpStatusCode.OK, (await adminBrowser.GetAsync("/Admin")).StatusCode);
    }

    [Fact]
    public async Task Posting_extra_fields_to_the_profile_form_changes_nothing_but_the_profile()
    {
        var customer = await shop.CreateCustomerAsync();
        var browser = shop.NewBrowser();
        await ShopFactory.SignInAsync(browser, customer.Email, ShopFactory.CustomerPassword);

        await browser.PostAsync("/UserPanel/Home/UserInfo", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["FirstName"] = "Renamed",
            ["LastName"] = "Customer",
            ["PhoneNumber"] = "09121111111",
            ["Address"] = "Tehran",
            ["Email"] = DemoDataSeeder.AdminEmail,
            ["IsAdmin"] = "true",
            ["IsEmailActive"] = "false"
        }));

        var saved = await shop.WithDbAsync(db => db.Users.AsNoTracking().SingleAsync(u => u.Id == customer.Id));
        Assert.Equal("Renamed", saved.FirstName);
        Assert.Equal(customer.Email, saved.Email);
        Assert.False(saved.IsAdmin);
        Assert.True(saved.IsEmailActive);

        // And the account still works afterwards.
        await ShopFactory.SignInAsync(shop.NewBrowser(), customer.Email, ShopFactory.CustomerPassword);
    }

    /// <summary>
    /// Wallet checkout used to be GET /UserPanel/Order/CheckWalletBalance?amount=..&amp;userId=..,
    /// trusting both values from the query string.
    /// </summary>
    [Fact]
    public async Task Wallet_checkout_charges_the_real_basket_total_whatever_the_request_says()
    {
        var customer = await shop.CreateCustomerAsync(walletBalance: 100_000_000);
        var browser = shop.NewBrowser();
        await ShopFactory.SignInAsync(browser, customer.Email, ShopFactory.CustomerPassword);
        var (productId, colorId, unitPrice) = await shop.WithDbAsync(db => db.ProductColors
            .Where(c => c.Count > 1)
            .Select(c => new ValueTuple<int, int, int>(c.ProductId, c.Id, c.Product.Price + c.Price))
            .FirstAsync());

        await browser.PostAsync("/UserPanel/Order/AddToCart", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["productId"] = productId.ToString(),
            ["productColorId"] = colorId.ToString()
        }));

        Assert.Equal(HttpStatusCode.NotFound,
            (await browser.GetAsync($"/UserPanel/Order/CheckWalletBalance?amount=1&userId={customer.Id}")).StatusCode);

        var token = await ShopFactory.AntiforgeryTokenFromAsync(browser, "/UserPanel/Order/ChoosePaymentWay");
        var pay = await browser.PostAsync("/UserPanel/Order/PayWithWallet", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["__RequestVerificationToken"] = token,
            ["amount"] = "1",
            ["userId"] = (await shop.AdminIdAsync()).ToString()
        }));

        Assert.Equal(HttpStatusCode.Redirect, pay.StatusCode);
        Assert.Contains("SuccessPayment", pay.Headers.Location!.OriginalString);
        var withdrawn = await shop.WithDbAsync(db => db.Transactions
            .Where(t => t.UserId == customer.Id && t.TransactionType == TransactionType.WithDraw)
            .SumAsync(t => t.Price));
        Assert.Equal(unitPrice, withdrawn);
    }

    [Fact]
    public async Task Wallet_checkout_without_an_antiforgery_token_is_rejected()
    {
        var customer = await shop.CreateCustomerAsync(walletBalance: 100_000_000);
        var browser = shop.NewBrowser();
        await ShopFactory.SignInAsync(browser, customer.Email, ShopFactory.CustomerPassword);

        var response = await browser.PostAsync("/UserPanel/Order/PayWithWallet", new FormUrlEncodedContent([]));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    /// <summary>
    /// Card checkout through the demo gateway and back into the real, anonymous callback -
    /// posted twice, as gateways and back buttons do.
    /// </summary>
    [Fact]
    public async Task A_card_payment_callback_completes_the_order_once_even_if_repeated()
    {
        var customer = await shop.CreateCustomerAsync();
        var browser = shop.NewBrowser();
        await ShopFactory.SignInAsync(browser, customer.Email, ShopFactory.CustomerPassword);
        var (productId, colorId) = await shop.WithDbAsync(db => db.ProductColors
            .Where(c => c.Count > 1)
            .Select(c => new ValueTuple<int, int>(c.ProductId, c.Id))
            .FirstAsync());
        await browser.PostAsync("/UserPanel/Order/AddToCart", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["productId"] = productId.ToString(),
            ["productColorId"] = colorId.ToString()
        }));

        var start = await browser.GetAsync("/start-pay");
        Assert.Equal(HttpStatusCode.Redirect, start.StatusCode);
        var gatewayQuery = System.Web.HttpUtility.ParseQueryString(new Uri(new Uri("http://localhost"), start.Headers.Location!).Query);
        var callbackPath = new Uri(gatewayQuery["callback"]!).PathAndQuery;

        var gatewayPost = new Dictionary<string, string> { ["paymentStatus"] = "OK", ["authority"] = gatewayQuery["authority"]! };
        var bank = shop.NewBrowser(); // the gateway's request carries none of the customer's cookies
        var first = await bank.PostAsync(callbackPath, new FormUrlEncodedContent(gatewayPost));
        var second = await bank.PostAsync(callbackPath, new FormUrlEncodedContent(gatewayPost));

        Assert.Contains("SuccessPayment", first.Headers.Location!.OriginalString);
        Assert.Contains("SuccessPayment", second.Headers.Location!.OriginalString);
        Assert.Equal(1, await shop.WithDbAsync(db => db.Orders.CountAsync(o => o.UserId == customer.Id && o.IsFinally)));
        Assert.Equal(1, await shop.WithDbAsync(db => db.Transactions.CountAsync(t =>
            t.UserId == customer.Id && t.TransactionType == TransactionType.WithDraw)));
    }
}
