

using Domain.Entities.Product;

namespace Domain.Interface
{
    public interface IProductColorRepository
    {
        Task AddColorAsync(Color  color);

        void UpdateColor(Color color);

        Task SaveChangeAsync();

        Task AddProductColorAsync(ProductColor color);

        void UpdateProductColor(ProductColor color);

        Task<List<Color>> GetAllColorAsync();

        Task<bool> CheckIsColorExistForProduct(int productId, string colorCode);

        Task<ProductColor?> GetProductColorWithid(int? productColorid);
        Task<List<ProductColor>> GetProductColorAsync(int productId);

        Task<Color> GetColorById(int id);
    }
}
