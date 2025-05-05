using Domain.ViewModel.Favorites;

namespace Application.Services.Interfaces;

public interface IFavoritesService
{
    #region Retrival

    Task<bool> IsProductFavoriteAsync(int userId, int productId);
    Task<List<FavoriteProductViewModel>> GetFavoriteProductsAsync(int userId);

    #endregion

    #region Operation

    Task AddFavoriteAsync(int userId, int productId);
    Task RemoveFavoriteAsync(int userId, int productId);

    #endregion
}