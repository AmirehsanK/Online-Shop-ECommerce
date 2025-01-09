using Application.Services.Interfaces;
using Domain.Enums;
using Domain.ViewModel.Product.ProductColor;
using Microsoft.AspNetCore.Mvc;
using Web.Attributes;
using Infra.Data.Statics;

namespace Web.Areas.Admin.Controllers
{
    [InvokePermission(PermissionName.ColorManagement)]
    public class ProductColorController : AdminBaseController
    {
        #region Ctor

        private readonly IProductColorService _productColorService;

        public ProductColorController(IProductColorService productColorService)
        {
            _productColorService = productColorService;
        }

        #endregion

        #region CreateColor

        [HttpGet]
        [InvokePermission(PermissionName.CreateColor)]
        public IActionResult CreateColor()
        {
            return View();
        }

        [HttpPost]
        [InvokePermission(PermissionName.CreateColor)]
        public async Task<IActionResult> CreateColor(CreateProductColorViewModel color)
        {
            if (!ModelState.IsValid)
            {
                return View(color);
            }

            await _productColorService.AddNewColor(color);
            TempData[SuccessMessage] = "رنگ با موفقیت اضافه شد.";
            return RedirectToAction(nameof(ColorList));
        }

        #endregion

        #region ColorList

        [HttpGet]
        [InvokePermission(PermissionName.ColorList)]
        public async Task<IActionResult> ColorList()
        {
            var model = await _productColorService.GetColorList();
            return View(model);
        }

        #endregion

        [HttpGet]
        [InvokePermission(PermissionName.DeleteColor)]
        public async Task<IActionResult> DeleteColor(int colorId)
        {
            await _productColorService.DeleteColorAsync(colorId);
            TempData[SuccessMessage] = "رنگ با موفقیت حذف شد.";
            return RedirectToAction(nameof(ColorList));
        }

        [HttpGet]
        [InvokePermission(PermissionName.ProductManagement)]
        public async Task<IActionResult> AddProductColor(int productId)
        {
            ViewData["Colors"] = await _productColorService.GetColorList();
            return View();
        }

        [HttpPost]
        [InvokePermission(PermissionName.ProductManagement)]
        public async Task<IActionResult> AddProductColor(AddProductColorViewModel model, int productId)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var res = await _productColorService.AddColorToProduct(model, productId);
            switch (res)
            {
                case ProductExistColor.Exist:
                    TempData[ErrorMessage] = "رنگ وارد شده موجود میباشد";
                    return RedirectToAction("Productlist", "Product");
                case ProductExistColor.NotFound:
                    TempData[ErrorMessage] = "محصول مورد نظر یافت نشد";
                    return RedirectToAction("Productlist", "Product");
            }

            TempData[SuccessMessage] = "رنگ با موفقیت به محصول اضافه شد.";
            return RedirectToAction("Productlist", "Product");
        }
    }
}