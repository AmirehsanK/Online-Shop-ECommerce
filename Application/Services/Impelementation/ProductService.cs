

using Application.Extention;
using Application.Services.Interfaces;
using Application.Tools;
using Domain.Entities.Discount;
using Domain.Entities.Product;
using Domain.Enums;
using Domain.Interface;
using Domain.Shared;
using Domain.ViewModel.Product.CategoryAdmin;
using Domain.ViewModel.Product.Product;
using Infra.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Domain.ViewModel.Product.ProductGallery;

namespace Application.Services.Impelementation
{
    public class ProductService : IProductService
    {
        #region Ctor

        private readonly IProductRepository _productRepository;
        private readonly IProductGalleryRepository _productGalleryRepository;
        private readonly IDiscountRepository _discountRepository;

        public ProductService(IProductRepository productRepository, IProductGalleryRepository productGalleryRepository, IDiscountRepository discountRepository)
        {
            _productRepository = productRepository;
            _productGalleryRepository = productGalleryRepository;
            _discountRepository = discountRepository;
        }

        #endregion

        #region Category
        public async Task AddBaseCategory(BaseCategoryViewModel model, int? parentid = null)
        {
            var category = new ProductCategory()
            {
                CreateDate = DateTime.Now,
                IsDeleted = false,


                Title = model.Title,
            };
            if (parentid.HasValue)
            {
                category.ParentId = parentid.Value;
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

        public async Task<List<CategoriesListViewModel>> GetAllCategoriesForMegaMenu()
        {
            var Category = await _productRepository.GetAllCategoriesAsync();
            return Category.Select(u => new CategoriesListViewModel()
            {
                Title = u.Title,
                Id = u.Id,
                ParentId = u.ParentId
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
            var category = await _productRepository.GetBaseCategory(model.ParentId);
            var subCategory = new ProductCategory()
            {
                CreateDate = DateTime.Now,
                Title = model.Title,
                IsDeleted = false,
                ParentId = model.ParentId,


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
            var mainCategory = await _productRepository.GetBaseCategory(categoryid);
            mainCategory.IsDeleted = true;
            var SubCategory = await _productRepository.GetSubCategory(categoryid);
            if (SubCategory != null)
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

        public async Task<FilterProductViewModel> GetAllProductsAsync(FilterProductViewModel product)
        {
            var activeDiscounts = await _discountRepository.GetActiveDiscounts();
            var products = await _productRepository.GetProductsAsync(product);

            foreach (var item in products.Entities)
            {
                var productDiscount = await _discountRepository.GetHighestDiscountForProductAsync(item.Id);
                var discount = productDiscount != null
                    ? activeDiscounts.FirstOrDefault(d =>
                        d.Id == productDiscount.Id &&
                        (!d.StartDate.HasValue || d.StartDate <= DateTime.UtcNow) &&
                        (!d.EndDate.HasValue || d.EndDate >= DateTime.UtcNow))
                    : null;

                if (discount != null)
                {
                    if (discount.IsPercentage)
                    {
                        item.DiscountValue = discount.Value;
                    }
                    else
                    {
                        item.DiscountValue = (int)Math.Ceiling(((double)discount.Value / item.Price) * 100);
                    }

                    
                    var offPrice = discount.IsPercentage
                        ? item.Price * (1 - (discount.Value / 100.0))
                        : item.Price - discount.Value;

                    item.OffPrice = (int)Math.Max(0, offPrice);
                }
                else
                {
                   
                    item.OffPrice = 0;
                    item.DiscountValue = 0;
                }
            }
            return products;
        }
        public async Task<List<ProductViewModel>> GetAllProductsNoFilter()
        {

            var products = await _productRepository.GetAllProductsNoFilterAsync();

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
                    SubCategoryTitle = p.Category.Title,
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
                Id = product.Id,
                ProductName = product.ProductName,
                ShortDescription = product.ShortDescription,
                Review = product.Review,
                ExpertReview = product.ExpertReview,
                ImageName = product.ImageName,
                Price = product.Price,
                Inventory = product.Inventory,
                SubCategoryId = product.CategoryId,
                ProductGalleries = product.ProductGalleries
                ,
                IsDeleted = product.IsDeleted
            };
        }
        public async Task<List<ProductViewModel>> GetProductsByCategoryAsync(int categoryId)
        {
            var products = await _productRepository.GetProductsByCategoryAsync(categoryId);
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
                    SubCategoryTitle = p.Category.Title,
                    ProductGalleries = p.ProductGalleries
                })
                .ToList();
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
                ,
                IsDeleted = false,
                CreateDate = DateTime.Now,
                Inventory = 0
            };

            await _productRepository.AddProductAsync(product);
            await _productRepository.SaveChangeAsync();
        }
        public async Task UpdateProductAsync(ProductViewModel product)
        {
            var newProduct = await _productRepository.GetProductById(product.Id);
            if (product.Image != null)
            {
                var title = Guid.NewGuid().ToString("N") + Path.GetExtension(product.Image.FileName);
                product.ImageName.DeleteImage(PathTools.FileServerPath, null);
                product.Image.AddImageToServer(title, PathTools.FileServerPath, null, null);
                newProduct.ImageName = title;
            }
            newProduct.ProductName = product.ProductName;
            newProduct.ShortDescription = product.ShortDescription;
            newProduct.Review = product.Review;
            newProduct.Price = product.Price;
            newProduct.CategoryId = product.SubCategoryId;
            newProduct.ExpertReview = product.ExpertReview;
            newProduct.Inventory = product.Inventory;
            _productRepository.UpdateProduct(newProduct);
            await _productRepository.SaveChangeAsync();
        }

        public async Task DeleteProductAsync(int productId)
        {
            var imageName = await _productRepository.GetProductById(productId);
            imageName.ImageName.DeleteImage(PathTools.FilePath, null);
            imageName.IsDeleted = true;
            _productRepository.UpdateProduct(imageName);
            await _productRepository.SaveChangeAsync();
        }

        public async Task<ProductDetailViewModel> GetProductDetailForSite(int productid)
        {
            var activeDiscounts = await _discountRepository.GetActiveDiscounts();
            var item = await _productRepository.GetProductById(productid);
            Discount? productDiscount = await _discountRepository.GetHighestDiscountForProductAsync(productid);
            var discount = productDiscount != null
                ? activeDiscounts.FirstOrDefault(d =>
                    d.Id == productDiscount.Id &&
                    (!d.StartDate.HasValue || d.StartDate <= DateTime.UtcNow) &&
                    (!d.EndDate.HasValue || d.EndDate >= DateTime.UtcNow))
                : null;
            var viewmodel = new ProductDetailViewModel()
            {
                ExpertReview = item.ExpertReview,
                ImageName = item.ImageName,
                Inventory = item.Inventory,
                Price = item.Price,
                ProductName = item.ProductName,
                Review = item.Review,
                ShortDescription = item.ShortDescription,
                ProductId = productid,
                ProductGalleries = item.ProductGalleries,
                ProductColors = item.ProductColors
            };
            if (discount != null)
            {
                if (discount.IsPercentage)
                {
                    viewmodel.DiscountValue = discount.Value;
                }
                else
                {
                    viewmodel.DiscountValue = (int)Math.Ceiling(((double)discount.Value / item.Price) * 100);
                }


                var offPrice = discount.IsPercentage
                    ? item.Price * (1 - (discount.Value / 100.0))
                    : item.Price - discount.Value;

                viewmodel.OffPrice = (int)Math.Max(0, offPrice);
            }
            else
            {

                viewmodel.OffPrice = 0;
                viewmodel.DiscountValue = 0;
            }
            return viewmodel;

        }

        #endregion

    }
}
