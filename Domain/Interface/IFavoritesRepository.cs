using Domain.Entities.Favorites;
using Domain.ViewModel.Favorites;

namespace Domain.Interface;

public interface IFavoritesRepository
{
    #region Retrival

    Task<List<FavoriteProductViewModel>> GetFavoriteProductsAsync(int userId);
    Task<bool> IsProductFavoriteAsync(int userId, int productId);

    #endregion

    #region Operations

    Task AddFavoriteAsync(UserProductFavorites favorite);
    Task RemoveFavoriteAsync(int userId, int productId);

    #endregion
}