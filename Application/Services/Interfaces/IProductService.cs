using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Product;
using Domain.ViewModel.Product.CategoryAdmin;
using Domain.ViewModel.Product.Product;

namespace Application.Services.Interfaces
{
    public interface IProductService
    {
        #region Category
        Task AddBaseCategory(BaseCategoryViewModel  category,int? parentid=null);

        Task<List<CategoryListViewModel>> GetAllCategories(int? parentid);

        Task<List<CategoryListViewModel>> GetAllSubCategories();

        Task AddSubCategory(SubCategoryViewModel model);

        Task<EditBaseCategoryViewModel> GetBaseCategoryForEdit(int categoryid);


        Task EditBaseCategory(EditBaseCategoryViewModel model);

        Task DeleteBaseCategory(int categoryid);

        #endregion

        #region Product

        Task<FilterProductViewModel> GetAllProductsAsync(FilterProductViewModel product);
        Task<ProductViewModel> GetProductByIdAsync(int productId);
        Task AddProductAsync(AddProductViewModel model);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int productId);

        #endregion
    }
}
