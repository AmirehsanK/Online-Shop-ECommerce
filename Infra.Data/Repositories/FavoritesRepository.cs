using Domain.Entities.Favorites;
using Domain.Interface;
using Domain.ViewModel.Favorites;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repositories;

public class FavoritesRepository(ApplicationDbContext context) : IFavoritesRepository
{
    #region Retrival

    public async Task<bool> IsProductFavoriteAsync(int userId, int productId)
    {
        return await context.UserProductFavorites
            .AnyAsync(upf => upf.UserId == userId && upf.ProductId == productId);
    }

    public async Task<List<FavoriteProductViewModel>> GetFavoriteProductsAsync(int userId)
    {
        var favoriteProductIds = await context.UserProductFavorites
            .Where(upf => upf.UserId == userId)
            .Select(upf => upf.ProductId)
            .ToListAsync();

        var favoriteProducts = await context.Products
            .Where(p => favoriteProductIds.Contains(p.Id))
            .Select(p => new FavoriteProductViewModel
            {
                ProductId = p.Id,
                ProductName = p.ProductName,
                Price = p.Price,
                ImageUrl = p.ImageName
            })
            .ToListAsync();

        return favoriteProducts;
    }

    #endregion

    #region Operations

    public async Task AddFavoriteAsync(UserProductFavorites favorite)
    {
        await context.UserProductFavorites.AddAsync(favorite);
        await context.SaveChangesAsync();
    }

    public async Task RemoveFavoriteAsync(int userId, int productId)
    {
        var favorite = await context.UserProductFavorites
            .FirstOrDefaultAsync(upf => upf.UserId == userId && upf.ProductId == productId);

        if (favorite != null)
        {
            context.UserProductFavorites.Remove(favorite);
            await context.SaveChangesAsync();
        }
    }

    #endregion
}