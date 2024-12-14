

using Domain.Entities.Product;

namespace Domain.Interface
{
    public interface IProductGalleryRepository
    {
        Task AddGalleryAsync(ProductGallery  gallery);

        Task<List<ProductGallery>> GetGalleryWithIdAsync(int id);

        Task SaveChangeAsync();
    }
}
