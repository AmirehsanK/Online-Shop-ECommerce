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
                      ,CategoryId=c.CategoryId
                    })
                    .ToList(), 
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductViewModel model, IFormFile Image)
        {
            if (model.ProductName == null || model.ShortDescription==null)
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
            else { TempData[ErrorMessage] = "اضافه کردن محصول با مشکل مواجه شد"; }
            TempData[SuccessMessage] = "محصول با موفقیت اضافه شد";
            return RedirectToAction("ProductList");
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> ProductList(FilterProductViewModel filter)
        {
            var products = await _productService.GetAllProductsAsync(filter);
            return View(products);
        }
        [HttpGet]
        public async Task<IActionResult> ProductDetails(int productid)
        {
            var product=await _productService.GetProductByIdAsync(productid);
            return View(product);
        }
        #endregion

        #region ShowProductGallery

        [HttpGet]
       
        
        public async Task<IActionResult> ShowProductGallery(int id = 5)
        {
            ViewData["Gallery"] = await _galleryService.GetGalleryListAsync(id);
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> RemoveProductGallery(int galleryid)
        {
            await _galleryService.RemoveProductGallery(galleryid);
            return RedirectToRefererUrl();
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
