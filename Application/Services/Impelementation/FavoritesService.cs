using Application.Services.Interfaces;
using Domain.Entities.Favorites;
using Domain.Interface;
using Domain.ViewModel.Favorites;

namespace Application.Services.Impelementation;

public class FavoritesService(IFavoritesRepository repository) : IFavoritesService
{
    #region Retrival

    public async Task<bool> IsProductFavoriteAsync(int userId, int productId)
    {
        return await repository.IsProductFavoriteAsync(userId, productId);
    }

    public async Task<List<FavoriteProductViewModel>> GetFavoriteProductsAsync(int userId)
    {
        return await repository.GetFavoriteProductsAsync(userId);
    }

    #endregion

    #region Operations

    public async Task AddFavoriteAsync(int userId, int productId)
    {
        var isAlreadyFavorite = await repository.IsProductFavoriteAsync(userId, productId);

        if (isAlreadyFavorite) throw new InvalidOperationException("This product is already in your favorites.");

        var favorite = new UserProductFavorites
        {
            UserId = userId,
            ProductId = productId
        };

        await repository.AddFavoriteAsync(favorite);
    }

    public async Task RemoveFavoriteAsync(int userId, int productId)
    {
        await repository.RemoveFavoriteAsync(userId, productId);
    }

    #endregion
}