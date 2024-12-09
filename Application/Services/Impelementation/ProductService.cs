

using Application.Services.Interfaces;
using Domain.Entities.Product;
using Domain.Interface;
using Domain.ViewModel.Product;

namespace Application.Services.Impelementation
{
    public class ProductService:IProductService
    {
        #region Ctor

        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        #endregion
        public async Task AddBaseCategory(BaseCategoryViewModel model,int? parentid=null)
        {
            var category = new ProductCategory()
            {
                CreateDate = DateTime.Now,
                IsDeleted = false,
               
                Title = model.Title,
            };
            if (parentid.HasValue)
            {
                category.ParentId= parentid.Value;
            }
         
            await _productRepository.AddCategoryAsync(category);
            await _productRepository.SaveChangeAsync();
        }


        public async Task<List<CategoryListViewModel>> GetAllCategories(int? parentid)
        {

            var Subcategory = await _productRepository.GetAllCategory(parentid);
            return Subcategory.Select(u => new CategoryListViewModel()
            {
                Title = u.Title,
                ParentId = parentid,
                CategoryId = u.Id
            }).ToList();

        }

        public async Task AddSubCategory(SubCategoryViewModel model)
        {
            var subCategory = new ProductCategory()
            {
                CreateDate = DateTime.Now,
                Title = model.Title,
                IsDeleted = false,
                ParentId = model.ParentId
            };
            await _productRepository.AddCategoryAsync(subCategory);
            await _productRepository.SaveChangeAsync();
        }
    }
}
