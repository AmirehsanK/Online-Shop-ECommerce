using Application.Services.Interfaces;
using Domain.ViewModel.Product.ProductColor;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers
{
    public class ProductColorController : AdminBaseController
    {
        #region Ctor

        private readonly IProductColorService _productColorService;

        public ProductColorController(IProductColorService productColorService)
        {
            _productColorService = productColorService;
        }

        #endregion


        #region CraeateColor

        [HttpGet]
        public IActionResult CreateColor()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateColor(CreateProductColorViewModel color)
        {
            #region Validation

            if (!ModelState.IsValid)
            {
                return View(color);
            }

            #endregion

            await _productColorService.AddNewColor(color);
            return RedirectToAction(nameof(ColorList));
        }

        #endregion

        #region Colorlist

        [HttpGet]
        public async Task<IActionResult> ColorList()
        {
            var model = await _productColorService.GetColorList();
            return View(model);
        }

        #endregion

        [HttpGet]
        public async Task<IActionResult> DeleteColor(int colorId)
        {
            await _productColorService.DeleteColorAsync(colorId);
            return RedirectToAction(nameof(ColorList));
        }

        [HttpGet]
        public async Task<IActionResult> AddProductColor(int productId)
        {
            ViewData["Colors"] = await _productColorService.GetColorList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProductColor(AddProductColorViewModel model,int productId)
        {
            return View();
        }




    }
}
