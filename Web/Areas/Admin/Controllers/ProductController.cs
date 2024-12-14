using Application.Services.Interfaces;
using Domain.ViewModel.Product.ProductGallery;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers
{
    public class ProductController : AdminBaseController
    {
        private readonly IProductService _productService;
        #region Ctor

        //public ProductController(IProductService productService)
        private readonly IProductGalleryService _galleryService;

        public ProductController(IProductGalleryService galleryService)
        {
            //_productService = productService;
            _galleryService = galleryService;
        }

        #endregion

        #region ShowProductGallery

        [HttpGet]
        //public async Task<IActionResult> AddProduct()
        public async Task<IActionResult> ShowProductGallery(int id = 5)
        {

            ViewData["Gallery"] = await _galleryService.GetGalleryListAsync(id);

            //var model = new ProductViewModel
            //{
            //    Categories = await _productService.GetAllCategories(null)
            //        .Select(c => new SelectListItem
            //        {
            //            Value = c.CategoryId.ToString(),
            //            Text = c.Title
            //        }).ToListAsync()
            //};

            //return View(model);
            return View();
        }
        [HttpGet]
        //public async Task<IActionResult> GetCategories()

        [HttpPost]
        public async Task<IActionResult> ShowProductGallery(ShowProductGalleryViewModel model)
        {
            //var categories = await _productService.GetAllCategories(null); 
            //return Json(categories.Select(c => new { c.CategoryId, c.Title }));

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

        [HttpGet]
        public async Task<IActionResult> GetSubCategories(int categoryId)
        {
            var subCategories = await _productService.GetAllCategories(categoryId); 
            return Json(subCategories.Select(sc => new { sc.CategoryId, sc.Title }));
        }
        #endregion





    }
}
