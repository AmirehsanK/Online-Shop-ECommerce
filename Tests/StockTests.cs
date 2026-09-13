using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Tests;

public class StockTests
{
    [Fact]
    public async Task The_last_unit_can_be_added_once_and_then_the_variant_is_sold_out()
    {
        using var db = new TestDatabase();
        var first = await db.AddUserAsync("first@test");
        var second = await db.AddUserAsync("second@test");
        var (productId, colorId) = await db.AddProductAsync(price: 100, stock: 1);

        await using (var context = db.NewContext())
            Assert.Equal(AddToBasketResult.Success, await TestDatabase.Orders(context).AddProductToOrder(productId, first.Id, colorId));
        await using (var context = db.NewContext())
            Assert.Equal(AddToBasketResult.OutOfStock, await TestDatabase.Orders(context).AddProductToOrder(productId, second.Id, colorId));

        await using var check = db.NewContext();
        Assert.Equal(0, (await check.ProductColors.SingleAsync(c => c.Id == colorId)).Count);
        Assert.False(await check.OrderDetails.AnyAsync(d => d.Order.UserId == second.Id));
    }

    [Fact]
    public async Task A_colour_from_a_different_product_is_rejected()
    {
        using var db = new TestDatabase();
        var user = await db.AddUserAsync("buyer@test");
        var (phone, _) = await db.AddProductAsync(price: 100, stock: 5);
        var (_, otherProductsColor) = await db.AddProductAsync(price: 999_999, stock: 5, colorExtraPrice: -999_000);

        await using var context = db.NewContext();
        Assert.Equal(AddToBasketResult.Failed, await TestDatabase.Orders(context).AddProductToOrder(phone, user.Id, otherProductsColor));
    }

    [Fact]
    public async Task Basket_rows_without_a_colour_do_not_show_the_previous_rows_colour()
    {
        using var db = new TestDatabase();
        var user = await db.AddUserAsync("buyer@test");
        var (withColor, colorId) = await db.AddProductAsync(price: 100, stock: 5);
        var (plain, _) = await db.AddProductAsync(price: 200, stock: 5);

        await db.AddToBasketAsync(user.Id, withColor, colorId);
        await using (var context = db.NewContext())
            await TestDatabase.Orders(context).AddProductToOrder(plain, user.Id, productColorId: null);

        await using var read = db.NewContext();
        var rows = await TestDatabase.Orders(read).GetBasketDetail(user.Id);
        Assert.Equal("Black", rows.Single(r => r.ProductId == withColor).ColorName);
        Assert.Null(rows.Single(r => r.ProductId == plain).ColorName);
    }
}
