using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Product;
using Domain.Shared;
using Domain.ViewModel.Product.Product;
using static Domain.Shared.Paging;

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
        public IQueryable<Product> GetProducts();
        Task<Product> GetProductById(int ProductId);
        Task<List<Product>> GetAllProductsNoFilterAsync();
        Task AddProductAsync(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);

        Task<List<Product>> GetProductsByCategoryAsync(int categoryId);

        #endregion
    }
}
