using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Product;

namespace Domain.Interface
{
    public interface IProductRepository
    {
        #region Category
        Task AddCategoryAsync(ProductCategory product);
        void UpdateCategoryAsync(ProductCategory product);

        Task SaveChangeAsync();

        Task<List<ProductCategory>> GetAllCategoriesAsync();
        Task<List<ProductCategory>> GetAllCategory(int? parentid=null);

        Task<List<ProductCategory>> GetAllSubCategory();

        Task<ProductCategory> GetBaseCategory(int CategoryId);

        Task<List<ProductCategory>> GetSubCategory(int CategoryId);

        void UpdateCategoryList(List<ProductCategory> model);
        #endregion

        #region Product

        Task<List<Product>> GetProductsAsync();
        Task<Product> GetProductById(int ProductId);
        Task AddProductAsync(Product product);
        Task UpdateProduct(Product product);
        Task DeleteProduct(int ProductId);

        #endregion
    }
}
