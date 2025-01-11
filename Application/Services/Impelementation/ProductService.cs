using Application.Extention;
using Application.Security;
using Application.Services.Interfaces;
using Application.Tools;
using Domain.Entities.Discount;
using Domain.Entities.Product;
using Domain.Interface;
using Domain.ViewModel.Product.CategoryAdmin;
using Domain.ViewModel.Product.Product;

namespace Application.Services.Impelementation;

public class ProductService(
    IProductRepository productRepository,
    IProductGalleryRepository productGalleryRepository,
    IDiscountRepository discountRepository)
    : IProductService
{
        
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

        await productRepository.AddCategoryAsync(category);
        await productRepository.SaveChangeAsync();
    }

    public async Task<List<CategoryListViewModel>> GetAllCategories(int? parentid)
    {
        var subcategory = await productRepository.GetAllCategory(parentid);
        return subcategory.Select(u => new CategoryListViewModel()
        {
            Title = u.Title,
            ParentId = parentid,
            CategoryId = u.Id
        }).ToList();
    }

    public async Task<List<CategoriesListViewModel>> GetAllCategoriesForMegaMenu()
    {
        var category = await productRepository.GetAllCategoriesAsync();
        return category.Select(u => new CategoriesListViewModel()
        {
            Title = u.Title,
            Id = u.Id,
            ParentId = u.ParentId
        }).ToList();
    }

    public async Task<List<CategoryListViewModel>> GetAllSubCategories()
    {
        var subcategory = await productRepository.GetAllSubCategory();
        return subcategory.Select(u => new CategoryListViewModel()
        {
            Title = u.Title,
            ParentId = u.ParentId,
            CategoryId = u.Id
        }).ToList();
    }

    public async Task<List<SubCategoryViewModel>> GetAllSubCategoriesForSlider()
    {
        var subcategory = await productRepository.GetAllSubCategory();
        return subcategory.Select(u => new SubCategoryViewModel()
        {
            Title = u.Title,
            ParentId = (int)u.ParentId!,
            Id = u.Id,
            ImageName = u.ImageName!
        }).Take(9).ToList();
    }

    public async Task AddSubCategory(SubCategoryViewModel model)
    {
        var category = await productRepository.GetBaseCategory(model.ParentId);
        var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(model.Image.FileName);
        model.Image.AddImageToServer(imageName, PathTools.CategoryImageServerPath, null, null);
        var subCategory = new ProductCategory()
        {
            CreateDate = DateTime.Now,
            Title = model.Title,
            IsDeleted = false,
            ParentId = model.ParentId,
            ImageName = imageName
        };
        await productRepository.AddCategoryAsync(subCategory);
        await productRepository.SaveChangeAsync();
    }

    public async Task<EditBaseCategoryViewModel> GetBaseCategoryForEdit(int categoryid)
    {
        var category = await productRepository.GetBaseCategory(categoryid);
        var viewmodel = new EditBaseCategoryViewModel()
        {
            CategoryId = category.Id,
            CategoryTitle = category.Title
        };
        return viewmodel;
    }

    public async Task EditBaseCategory(EditBaseCategoryViewModel model)
    {
        var category = await productRepository.GetBaseCategory(model.CategoryId);
        if (model.ParentId != null && model.Image!.IsImage())
        {
            var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(model.Image!.FileName);
            model.Image.AddImageToServer(imageName, PathTools.CategoryImageServerPath, null, null);
            category.ImageName?.DeleteImage(PathTools.CategoryImagePath, null);
            category.ImageName = imageName;
        }
        category.Title = model.CategoryTitle;
        productRepository.UpdateCategoryAsync(category);
        await productRepository.SaveChangeAsync();
    }

    public async Task DeleteBaseCategory(int categoryid)
    {
        var mainCategory = await productRepository.GetBaseCategory(categoryid);
        mainCategory.IsDeleted = true;
        var subCategory = await productRepository.GetSubCategory(categoryid);
        if (subCategory != null!)
        {
            foreach (var item in subCategory)
            {
                item.IsDeleted = true;
                if (item.ImageName != null)
                {
                    item.ImageName.DeleteImage(PathTools.CategoryImagePath, null);
                }
            }
            productRepository.UpdateCategoryList(subCategory);
        }
        productRepository.UpdateCategoryAsync(mainCategory);
        await productRepository.SaveChangeAsync();
    }
    #endregion

    #region Product
    public async Task<List<ProductViewModel>> GetAllProductsNoFilter()
    {
        var products = await productRepository.GetAllProductsNoFilterAsync();
        return products
            .Where(p => !p.IsDeleted)
            .Select(p => new ProductViewModel
            {
                Id = p.Id,
                ProductName = p.ProductName,
                ShortDescription = p.ShortDescription,
                Review = p.Review!,
                ExpertReview = p.ExpertReview!,
                ImageName = p.ImageName,
                Price = p.Price,
                Inventory = p.Inventory,
                SubCategoryId = p.CategoryId,
                SubCategoryTitle = p.Category.Title,
                ProductGalleries = p.ProductGalleries
            })
            .ToList();
    }

    public async Task<FilterProductViewModel> GetAllProductsAsync(FilterProductViewModel product)
    {
        var activeDiscounts = await discountRepository.GetActiveDiscounts();
        var products = await productRepository.GetProductsAsync(product);

        foreach (var item in products.Entities)
        {
            var productDiscount = await discountRepository.GetHighestDiscountForProductAsync(item.Id);
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

                item.DiscountEndDate = productDiscount!.EndDate;
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

    public async Task<List<ProductViewModel>> Get8MostDiscountedProducts()
    {
        var allProducts = await GetAllProductsAsync(new FilterProductViewModel());

        var discountsEndingSoon = allProducts.Entities
            .Where(p => p.DiscountValue > 0 && p.DiscountEndDate.HasValue)
            .OrderBy(p => p.DiscountEndDate)
            .Take(8)
            .ToList();

        return discountsEndingSoon;
    }

    public async Task<ProductViewModel> GetProductByIdAsync(int productId)
    {
        var product = await productRepository.GetProductById(productId);
        if (product == null!) return null!;

        return new ProductViewModel
        {
            Id = product.Id,
            ProductName = product.ProductName,
            ShortDescription = product.ShortDescription,
            Review = product.Review!,
            ExpertReview = product.ExpertReview!,
            ImageName = product.ImageName,
            Price = product.Price,
            Inventory = product.Inventory,
            SubCategoryId = product.CategoryId,
            ProductGalleries = product.ProductGalleries,
            IsDeleted = product.IsDeleted
        };
    }

    public async Task<List<ProductViewModel>> GetProductsByCategoryAsync(int categoryId)
    {
        var products = await productRepository.GetProductsByCategoryAsync(categoryId);
        return products
            .Where(p => !p.IsDeleted)
            .Select(p => new ProductViewModel
            {
                Id = p.Id,
                ProductName = p.ProductName,
                ShortDescription = p.ShortDescription,
                Review = p.Review!,
                ExpertReview = p.ExpertReview!,
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
            CategoryId = model.SubCategoryId,
            IsDeleted = false,
            CreateDate = DateTime.Now,
            Inventory = 0
        };

        await productRepository.AddProductAsync(product);
        await productRepository.SaveChangeAsync();
    }

    public async Task UpdateProductAsync(ProductViewModel product)
    {
        var newProduct = await productRepository.GetProductById(product.Id);
        if (product.Image != null!)
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
        productRepository.UpdateProduct(newProduct);
        await productRepository.SaveChangeAsync();
    }

    public async Task DeleteProductAsync(int productId)
    {
        var imageName = await productRepository.GetProductById(productId);
        imageName.ImageName.DeleteImage(PathTools.FilePath, null);
        imageName.IsDeleted = true;
        productRepository.UpdateProduct(imageName);
        await productRepository.SaveChangeAsync();
    }

    public async Task<ProductDetailViewModel> GetProductDetailForSite(int productid)
    {
        var activeDiscounts = await discountRepository.GetActiveDiscounts();
        var item = await productRepository.GetProductById(productid);
        var productDiscount = await discountRepository.GetHighestDiscountForProductAsync(productid);
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