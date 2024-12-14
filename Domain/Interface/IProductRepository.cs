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

        Task AddCategoryAsync(ProductCategory product);
        void UpdateCategoryAsync(ProductCategory product);

        Task SaveChangeAsync();

        Task<List<ProductCategory>> GetAllCategory(int? parentid=null);

        Task<ProductCategory> GetBaseCategory(int CategoryId);

        Task<List<ProductCategory>> GetSubCategory(int CategoryId);

        void UpdateCategoryList(List<ProductCategory> model);
    }
}
