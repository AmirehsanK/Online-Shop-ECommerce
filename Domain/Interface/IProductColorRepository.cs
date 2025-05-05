using Domain.Entities.Product;

namespace Domain.Interface;

public interface IProductColorRepository
{
    #region Save Changes

    Task SaveChangeAsync();

    #endregion

    Task<ProductColor?> GetProductColorWithid(int? productColorid);

    #region Color Management

    Task AddColorAsync(Color color);
    void UpdateColor(Color color);
    Task<List<Color>> GetAllColorAsync();
    Task<Color> GetColorById(int id);

    #endregion

    #region Product Color Management

    Task AddProductColorAsync(ProductColor color);
    void UpdateProductColor(ProductColor color);
    Task<bool> CheckIsColorExistForProduct(int productId, string colorCode);
    Task<ProductColor> GetProductColorWithid(int productColorid);
    Task<List<ProductColor>> GetProductColorAsync(int productId);

    #endregion
}