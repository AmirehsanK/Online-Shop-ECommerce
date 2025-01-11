using Application.Security;
using Application.Services.Interfaces;
using Domain.ViewModel.Product.CategoryAdmin;
using Domain.ViewModel.Product.Product;
using Domain.ViewModel.Product.ProductGallery;
using Infra.Data.Statics;
using Microsoft.AspNetCore.Mvc;
using Web.Attributes;

namespace Web.Areas.Admin.Controllers;

[InvokePermission(PermissionName.ProductManagement)]
public class ProductController(IProductGalleryService galleryService, IProductService productService)
    : AdminBaseController
{
    #region Product

    #region Add Product

    [InvokePermission(PermissionName.CreateProduct)]
    public async Task<IActionResult> AddProduct()
    {
        var categories = await productService.GetAllSubCategories();

        var model = new AddProductViewModel
        {
            SubCategories = categories
                .Where(c => c.ParentId != null)
                .Select(c => new CategoryListViewModel
                {
                    Title = c.Title,
                    CategoryId = c.CategoryId
                })
                .ToList()
        };

        return View(model);
    }

    [HttpPost]
    [InvokePermission(PermissionName.CreateProduct)]
    public async Task<IActionResult> AddProduct(AddProductViewModel model, IFormFile image)
    {
        if (model.ProductName == null! || model.ShortDescription == null!)
        {
            var categories = await productService.GetAllCategories(null);
            model = new AddProductViewModel
            {
                SubCategories = categories
                    .Where(c => c.ParentId != null)
                    .Select(c => new CategoryListViewModel
                    {
                        Title = c.Title
                    })
                    .ToList()
            };
            TempData[ErrorMessage] = "اضافه کردن محصول با مشکل مواجه شد";
            return View(model);
        }

        if (image is { Length: > 0 })
            await productService.AddProductAsync(model);
        else
            TempData[ErrorMessage] = "اضافه کردن محصول با مشکل مواجه شد";
        TempData[SuccessMessage] = "محصول با موفقیت اضافه شد";
        return RedirectToAction("ProductList");
    }

    #endregion

    #region Product List

    [InvokePermission(PermissionName.ProductList)]
    public async Task<IActionResult> ProductList(FilterProductViewModel filter)
    {
        var products = await productService.GetAllProductsAsync(filter);
        return View(products);
    }

    #endregion

    #region Update Product

    [InvokePermission(PermissionName.UpdateProduct)]
    public async Task<IActionResult> UpdateProduct(int productId)
    {
        var categories = await productService.GetAllSubCategories();

        ViewData["Category"] = new ProductViewModel
        {
            SubCategories = categories
                .Where(c => c.ParentId != null)
                .Select(c => new CategoryListViewModel
                {
                    Title = c.Title,
                    CategoryId = c.CategoryId
                })
                .ToList()
        };
        var product = await productService.GetProductByIdAsync(productId);
        return View(product);
    }

    [HttpPost]
    [InvokePermission(PermissionName.UpdateProduct)]
    public async Task<IActionResult> UpdateProduct(ProductViewModel model)
    {
        if (model.ProductName == null! || model.ShortDescription == null!)
        {
            var categories = await productService.GetAllCategories(null);
            model = new ProductViewModel
            {
                SubCategories = categories
                    .Where(c => c.ParentId != null)
                    .Select(c => new CategoryListViewModel
                    {
                        Title = c.Title
                    })
                    .ToList()
            };
            TempData[ErrorMessage] = "ویرایش محصول با مشکل مواجه شد";
            return View(model);
        }

        await productService.UpdateProductAsync(model);
        TempData[SuccessMessage] = "محصول با موفقیت ویرایش شد";
        return RedirectToAction(nameof(ProductList));
    }

    #endregion

    #endregion

    #region ShowProductGallery

    [InvokePermission(PermissionName.ProductGallery)]
    public async Task<IActionResult> ShowProductGallery(int productId)
    {
        ViewData["Gallery"] = await galleryService.GetGalleryListAsync(productId);
        return View();
    }

    [InvokePermission(PermissionName.ProductGallery)]
    public async Task<IActionResult> RemoveProductGallery(int galleryId)
    {
        await galleryService.RemoveProductGallery(galleryId);
        return RedirectToRefererUrl();
    }

    [HttpPost]
    [InvokePermission(PermissionName.ProductGallery)]
    public async Task<IActionResult> ShowProductGallery(ShowProductGalleryViewModel model)
    {
        if (!ModelState.IsValid) return View();

        await galleryService.AddProductGalleries(model);
        TempData[SuccessMessage] = "گالری محصول با موفقیت به‌روزرسانی شد.";
        return View();
    }

    [HttpPost("UploadCkeditorImage")]
    [InvokePermission(PermissionName.ProductManagement)]
    public async Task<JsonResult> UploadCkeditorImage(IFormFile upload)
    {
        if (upload.Length <= 0 || !upload.IsImage()) return null!;
        var fileName = Guid.NewGuid() +
                       Path.GetExtension(upload.FileName).ToLower();
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/docs/images/ckeditor");

        if (!Directory.Exists(path)) Directory.CreateDirectory(path);

        path = Path.Combine(path, fileName);
        await using (FileStream stream = new(path, FileMode.Create))
        {
            await upload.CopyToAsync(stream);
        }

        var url = $"/docs/images/ckeditor/{fileName}";

        return Json(new { uploaded = true, url });
    }

    #endregion
}