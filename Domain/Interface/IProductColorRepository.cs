

using Domain.Entities.Product;

namespace Domain.Interface
{
    public interface IProductColorRepository
    {
        Task AddColorAsync(Color  color);

        void UpdateColor(Color color);

        Task SaveChangeAsync();

        Task ProductColorAsync(ProductColor color);

        void UpdateProductColor(ProductColor color);

        Task<List<Color>> GetAllColorAsync();

        Task<Color> GetColorById(int id);
    }
}
