using Application.Services.Interfaces;
using Domain.ViewModel.Product.CategoryAdmin;
using Domain.ViewModel.Product.Product;
using Domain.ViewModel.Product.ProductGallery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Areas.Admin.Controllers
{
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

        #region Product

        #region Add Product
        public async Task<IActionResult> AddProduct()
        {
             var categories = await _productService.GetAllSubCategories();

            var model = new AddProductViewModel
            {
                 SubCategories = categories
                .Where(c => c.ParentId != null) 
                .Select(c => new CategoryListViewModel
                   {
                      Title = c.Title
                    })
                    .ToList(), 
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductViewModel model, IFormFile Image)
        {
            if (!ModelState.IsValid)
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

                return View(model);
            }

            if (Image != null && Image.Length > 0)
            {
                await _productService.AddProductAsync(model);
            }
            else { TempData[ErrorMessage] = "اضافه کردن محصول با مشکل مواجه شد"; }
            TempData[SuccessMessage] = "محصول با موفقیت اضافه شد";
            return RedirectToAction("ProductList");
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> ProductList()
        {
            var products = await _productService.GetAllProductsAsync();
            return View(products);
        }

        #endregion

        #region ShowProductGallery

        [HttpGet]
        
        public async Task<IActionResult> ShowProductGallery(int id = 5)
        {

            ViewData["Gallery"] = await _galleryService.GetGalleryListAsync(id);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ShowProductGallery(ShowProductGalleryViewModel model)
        {
            model.ProductId = 5;
            #region Validation

            if (!ModelState.IsValid)
            {
                return View();
            }

            #endregion

            await _galleryService.AddProductGalleries(model);
            return View();
        }

        
        #endregion
    }
}
