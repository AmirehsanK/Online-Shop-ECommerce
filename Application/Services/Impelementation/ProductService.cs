

using Application.Extention;
using Application.Services.Interfaces;
using Application.Tools;
using Domain.Entities.Product;
using Domain.Enums;
using Domain.Interface;
using Domain.ViewModel.Product.CategoryAdmin;
using Domain.ViewModel.Product.Product;
using Infra.Data.Repositories;
using System;

namespace Application.Services.Impelementation
{
    public class ProductService:IProductService
    {
        #region Ctor

        private readonly IProductRepository _productRepository;
        private readonly IProductGalleryRepository _productGalleryRepository;

        public ProductService(IProductRepository productRepository, IProductGalleryRepository productGalleryRepository)
        {
            _productRepository = productRepository;
            _productGalleryRepository = productGalleryRepository;
        }

        #endregion

        #region Category
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

        public async Task<List<CategoryListViewModel>> GetAllSubCategories()
        {
            var Subcategory = await _productRepository.GetAllSubCategory();
            return Subcategory.Select(u => new CategoryListViewModel()
            {
                Title = u.Title,
                ParentId = u.ParentId,
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
        #endregion

        #region Product

        public async Task<List<ProductViewModel>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetProductsAsync();

            return products
                .Where(p => !p.IsDeleted)
                .Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    ShortDescription = p.ShortDescription,
                    Review = p.Review,
                    ExpertReview = p.ExpertReview,
                    ImageName = p.ImageName,
                    Price = p.Price,
                    Inventory = p.Inventory,
                    SubCategoryId = p.CategoryId, 
                    SubCategoryTitle=p.Category.Title,
                    ProductGalleries = p.ProductGalleries
                })
                .ToList();
        }

        public async Task<ProductViewModel> GetProductByIdAsync(int productId)
        {
            var product = await _productRepository.GetProductById(productId);
            if (product == null) return null;

            return new ProductViewModel
            {
                ProductName = product.ProductName,
                ShortDescription = product.ShortDescription,
                Review = product.Review,
                ExpertReview = product.ExpertReview,
                ImageName = product.ImageName,
                Price = product.Price,
                Inventory = product.Inventory,
                SubCategoryId = product.CategoryId,
                ProductGalleries = product.ProductGalleries
                , IsDeleted = product.IsDeleted
            };
        }

        public async Task AddProductAsync(AddProductViewModel model)
        {
            var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(model.Image.FileName);
            model.Image.AddImageToServer(imageName, PathTools.FileServerPath, null, null);
            var product = new Product
            {
                ProductName = model.ProductName,
                ShortDescription = model.ShortDescription,
                Review = model.Review,
                ExpertReview = model.ExpertReview,
                ImageName = imageName,
                Price = model.Price,
                CategoryId = model.SubCategoryId
                ,IsDeleted=false,
                CreateDate = DateTime.Now,
                Inventory=0
            };

            await _productRepository.AddProductAsync(product);
            await _productRepository.SaveChangeAsync();
        }
        //TODO Update
        public async Task UpdateProductAsync(Product product)
        {
            await _productRepository.UpdateProduct(product);
            await _productRepository.SaveChangeAsync();
        }

        public async Task DeleteProductAsync(int productId)
        {
            var imageName = await _productRepository.GetProductById(productId);   
            imageName.ImageName.DeleteImage(PathTools.FilePath, null);
            imageName.IsDeleted = true;
            await _productRepository.UpdateProduct(imageName);
            await _productRepository.SaveChangeAsync();
        }

        #endregion

    }
}
