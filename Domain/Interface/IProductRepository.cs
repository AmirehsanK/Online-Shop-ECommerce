using Domain.Entities.Product;
using Domain.ViewModel.Product.Product;

namespace Domain.Interface;

public interface IProductRepository
{
    #region Save Changes

    Task SaveChangeAsync();

    #endregion

    #region Category

    Task AddCategoryAsync(ProductCategory product);
    void UpdateCategoryAsync(ProductCategory product);
    Task<List<ProductCategory>> GetAllCategoriesAsync();
    Task<List<ProductCategory>> GetAllCategory(int? parentid = null);
    Task<List<ProductCategory>> GetAllSubCategory();
    Task<ProductCategory> GetBaseCategory(int CategoryId);
    Task<List<ProductCategory>> GetSubCategory(int CategoryId);
    void UpdateCategoryList(List<ProductCategory> model);

    #endregion

    #region Product

    Task<FilterProductViewModel> GetProductsAsync(FilterProductViewModel filter);
    Task<Product> GetProductById(int ProductId);
    Task<List<Product>> GetAllProductsNoFilterAsync();
    Task AddProductAsync(Product product);
    void UpdateProduct(Product product);
    void DeleteProduct(Product product);
    Task<List<Product>> GetProductsByCategoryAsync(int categoryId);

    #endregion
}