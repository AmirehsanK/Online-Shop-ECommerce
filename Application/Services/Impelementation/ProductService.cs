

using Application.Services.Interfaces;
using Domain.Entities.Product;
using Domain.Interface;
using Domain.ViewModel.Product.CategoryAdmin;

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

        public async Task<EditBaseCategoryViewModel> GetBaseCategoryForEdit(int categoryid)
        {
            var category = await _productRepository.GetBaseCategory(categoryid);
            var viewmodel = new EditBaseCategoryViewModel()
            {
                CategoryId = category.Id,
                CategoryTitle = category.Title
            };
            return viewmodel;
        }

        public async Task EditBaseCategory(EditBaseCategoryViewModel model)
        {
            var category = await _productRepository.GetBaseCategory(model.CategoryId);
            category.Title = model.CategoryTitle;
            _productRepository.UpdateCategoryAsync(category);
            await _productRepository.SaveChangeAsync();
        }

        public async Task DeleteBaseCategory(int categoryid)
        {
           var mainCategory= await _productRepository.GetBaseCategory(categoryid);
           mainCategory.IsDeleted = true;
           var SubCategory = await _productRepository.GetSubCategory(categoryid);
           if (SubCategory!=null)
           {
               foreach (var item in SubCategory)
               {
                   item.IsDeleted = true;
                   
               }
               _productRepository.UpdateCategoryList(SubCategory);
           }
        
           _productRepository.UpdateCategoryAsync(mainCategory);
   
            await _productRepository.SaveChangeAsync();




        }
    }
}
