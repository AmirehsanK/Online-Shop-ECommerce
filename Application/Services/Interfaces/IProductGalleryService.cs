

using Domain.Entities.Product;
using Domain.ViewModel.Product.ProductColor;
using Domain.ViewModel.Product.ProductGallery;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Interfaces
{
    public interface IProductGalleryService
    {
        Task<List<ProductGallery>> GetGalleryListAsync(int productid);

        Task AddProductGalleries(ShowProductGalleryViewModel galleries);

        Task RemoveProductGallery(int galleryid);


    }
}
