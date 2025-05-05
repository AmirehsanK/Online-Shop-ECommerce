using Domain.Entities.Product;
using Domain.Interface;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repositories;

public class ProductColorRepository(ApplicationDbContext context) : IProductColorRepository
{
    #region Save Changes

    public async Task SaveChangeAsync()
    {
        await context.SaveChangesAsync();
    }

    #endregion

    #region Color Methods

    public async Task AddColorAsync(Color color)
    {
        await context.Colors.AddAsync(color);
    }

    public void UpdateColor(Color color)
    {
        context.Colors.Update(color);
    }

    public async Task<List<Color>> GetAllColorAsync()
    {
        return await context.Colors.Where(u => u.IsDeleted == false).ToListAsync();
    }

    public async Task<Color> GetColorById(int id)
    {
        return (await context.Colors.FindAsync(id))!;
    }

    #endregion

    #region Product Color Methods

    public async Task AddProductColorAsync(ProductColor color)
    {
        await context.ProductColors.AddAsync(color);
    }

    public void UpdateProductColor(ProductColor color)
    {
        context.Update(color);
    }

    public async Task<bool> CheckIsColorExistForProduct(int productId, string colorCode)
    {
        return await context.ProductColors
            .Where(u => u.ProductId == productId)
            .Include(u => u.Color)
            .AnyAsync(u => u.Color.ColorCode == colorCode);
    }

    public async Task<ProductColor?> GetProductColorWithid(int? productColorid)
    {
        var color = await context.ProductColors.Include(u => u.Color).FirstOrDefaultAsync(u => u.Id == productColorid);
        if (color == null) return null;

        return color;
    }

    public async Task<ProductColor> GetProductColorWithid(int productColorid)
    {
        return (await context.ProductColors
            .Include(u => u.Color)
            .FirstOrDefaultAsync(u => u.Id == productColorid))!;
    }

    public async Task<List<ProductColor>> GetProductColorAsync(int productId)
    {
        return await context.ProductColors
            .Where(u => u.ProductId == productId)
            .Include(u => u.Color)
            .ToListAsync();
    }

    #endregion
}