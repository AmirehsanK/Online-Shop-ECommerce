using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Product;
using Domain.ViewModel.Product.CategoryAdmin;

namespace Application.Services.Interfaces
{
    public interface IProductService
    {
        Task AddBaseCategory(BaseCategoryViewModel  category,int? parentid=null);

        Task<List<CategoryListViewModel>> GetAllCategories(int? parentid);

        Task AddSubCategory(SubCategoryViewModel model);

        Task<EditBaseCategoryViewModel> GetBaseCategoryForEdit(int categoryid);


        Task EditBaseCategory(EditBaseCategoryViewModel model);

        Task DeleteBaseCategory(int categoryid);


    }
}
