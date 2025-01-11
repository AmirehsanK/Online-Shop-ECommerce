using Domain.Entities.Product;
using Domain.ViewModel.Product.ProductGallery;

namespace Application.Services.Interfaces;

public interface IProductGalleryService
{
    #region Product Gallery Management

    Task<List<ProductGallery>> GetGalleryListAsync(int productid);
    Task AddProductGalleries(ShowProductGalleryViewModel galleries);
    Task RemoveProductGallery(int galleryid);

    #endregion
}