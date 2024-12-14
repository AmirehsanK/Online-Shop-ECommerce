using Application.Services.Interfaces;
using Domain.ViewModel.Product.ProductGallery;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers
{
    public class ProductController : AdminBaseController
    {
        #region Ctor

        private readonly IProductGalleryService _galleryService;

        public ProductController(IProductGalleryService galleryService)
        {
            _galleryService = galleryService;
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

            await _galleryService.AddProductGalleries(model);

            #endregion


            return View();
        }

        #endregion





    }
}
