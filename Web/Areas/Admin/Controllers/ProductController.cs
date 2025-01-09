using Application.Security;
using Application.Services.Interfaces;
using Domain.ViewModel.Product.CategoryAdmin;
using Domain.ViewModel.Product.Product;
using Domain.ViewModel.Product.ProductGallery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web.Attributes;
using Infra.Data.Statics;

namespace Web.Areas.Admin.Controllers
{
    [InvokePermission(PermissionName.ProductManagement)]
    public class ProductController : AdminBaseController
    {
        #region Ctor

        private readonly IProductService _productService;
        private readonly IProductGalleryService _galleryService;

        public ProductController(IProductGalleryService galleryService, IProductService productService)
        {
            _productService = productService;
            _galleryService = galleryService;
        }

        #endregion

        [InvokePermission(PermissionName.ProductManagement)]
        public IActionResult ProductManagement()
        {
            return View();
        }

        #region Product

        #region Add Product

        [InvokePermission(PermissionName.CreateProduct)]
        public async Task<IActionResult> AddProduct()
        {
            var categories = await _productService.GetAllSubCategories();

            var model = new AddProductViewModel
            {
                SubCategories = categories
                    .Where(c => c.ParentId != null)
                    .Select(c => new CategoryListViewModel
                    {
                        Title = c.Title,
                        CategoryId = c.CategoryId
                    })
                    .ToList(),
            };

            return View(model);
        }

        [HttpPost]
        [InvokePermission(PermissionName.CreateProduct)]
        public async Task<IActionResult> AddProduct(AddProductViewModel model, IFormFile Image)
        {
            if (model.ProductName == null || model.ShortDescription == null)
            {
                var categories = await _productService.GetAllCategories(null);
                model = new AddProductViewModel
                {
                    SubCategories = categories
                        .Where(c => c.ParentId != null)
                        .Select(c => new CategoryListViewModel
                        {
                            Title = c.Title
                        })
                        .ToList(),
                };
                TempData[ErrorMessage] = "اضافه کردن محصول با مشکل مواجه شد";
                return View(model);
            }

            if (Image != null && Image.Length > 0)
            {
                await _productService.AddProductAsync(model);
            }
            else
            {
                TempData[ErrorMessage] = "اضافه کردن محصول با مشکل مواجه شد";
            }
            TempData[SuccessMessage] = "محصول با موفقیت اضافه شد";
            return RedirectToAction("ProductList");
        }

        #endregion

        [InvokePermission(PermissionName.ProductList)]
        public async Task<IActionResult> ProductList(FilterProductViewModel filter)
        {
            var products = await _productService.GetAllProductsAsync(filter);
            return View(products);
        }

        [InvokePermission(PermissionName.UpdateProduct)]
        public async Task<IActionResult> UpdateProduct(int Productid)
        {
            var categories = await _productService.GetAllSubCategories();

            ViewData["Category"] = new ProductViewModel
            {
                SubCategories = categories
                    .Where(c => c.ParentId != null)
                    .Select(c => new CategoryListViewModel
                    {
                        Title = c.Title,
                        CategoryId = c.CategoryId
                    })
                    .ToList(),
            };
            var product = await _productService.GetProductByIdAsync(Productid);
            return View(product);
        }

        [HttpPost]
        [InvokePermission(PermissionName.UpdateProduct)]
        public async Task<IActionResult> UpdateProduct(ProductViewModel model)
        {
            if (model.ProductName == null || model.ShortDescription == null)
            {
                var categories = await _productService.GetAllCategories(null);
                model = new ProductViewModel
                {
                    SubCategories = categories
                        .Where(c => c.ParentId != null)
                        .Select(c => new CategoryListViewModel
                        {
                            Title = c.Title
                        })
                        .ToList(),
                };
                TempData[ErrorMessage] = "ویرایش محصول با مشکل مواجه شد";
                return View(model);
            }
            await _productService.UpdateProductAsync(model);
            TempData[SuccessMessage] = "محصول با موفقیت ویرایش شد";
            return RedirectToAction(nameof(ProductList));
        }

        #endregion

        #region ShowProductGallery

        [InvokePermission(PermissionName.ProductGallery)]
        public async Task<IActionResult> ShowProductGallery(int Productid)
        {
            ViewData["Gallery"] = await _galleryService.GetGalleryListAsync(Productid);
            return View();
        }

        [InvokePermission(PermissionName.ProductGallery)]
        public async Task<IActionResult> RemoveProductGallery(int galleryid)
        {
            await _galleryService.RemoveProductGallery(galleryid);
            return RedirectToRefererUrl();
        }

        [HttpPost]
        [InvokePermission(PermissionName.ProductGallery)]
        public async Task<IActionResult> ShowProductGallery(ShowProductGalleryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            await _galleryService.AddProductGalleries(model);
            TempData[SuccessMessage] = "گالری محصول با موفقیت به‌روزرسانی شد.";
            return View();
        }

        [HttpPost("UploadCkeditorImage")]
        [InvokePermission(PermissionName.ProductManagement)]
        public async Task<JsonResult> UploadCkeditorImage(IFormFile upload)
        {
            if (upload.Length <= 0 || !upload.IsImage()) { return null; }

            string fileName = Guid.NewGuid() +
                  Path.GetExtension(upload.FileName).ToLower();

            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/docs/images/ckeditor");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = Path.Combine(path, fileName);

            using (FileStream stream = new(path, FileMode.Create))
            {
                await upload.CopyToAsync(stream);
            }

            string url = $"{"/docs/images/ckeditor/"}{fileName}";

            return Json(new { uploaded = true, url });
        }

        #endregion
    }
}