using Domain.Entities.Product;

namespace Domain.Interface;

public interface IProductGalleryRepository
{
    #region Save Changes

    Task SaveChangeAsync();

    #endregion

    #region Gallery Management

    Task AddGalleryAsync(ProductGallery gallery);
    Task<List<ProductGallery>> GetGalleryWithIdAsync(int id);
    Task<ProductGallery> GetOneGalleryWithIdAsync(int id);
    void RemoveProductGallery(ProductGallery gallery);

    #endregion
}