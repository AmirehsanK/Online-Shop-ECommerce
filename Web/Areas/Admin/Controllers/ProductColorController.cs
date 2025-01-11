using Application.Services.Interfaces;
using Domain.Enums;
using Domain.ViewModel.Product.ProductColor;
using Infra.Data.Statics;
using Microsoft.AspNetCore.Mvc;
using Web.Attributes;

namespace Web.Areas.Admin.Controllers;

[InvokePermission(PermissionName.ColorManagement)]
public class ProductColorController(IProductColorService productColorService) : AdminBaseController
{
    #region ColorList

    [HttpGet]
    [InvokePermission(PermissionName.ColorList)]
    public async Task<IActionResult> ColorList()
    {
        var model = await productColorService.GetColorList();
        return View(model);
    }

    #endregion

    #region Delete Color

    [HttpGet]
    [InvokePermission(PermissionName.DeleteColor)]
    public async Task<IActionResult> DeleteColor(int colorId)
    {
        await productColorService.DeleteColorAsync(colorId);
        TempData[SuccessMessage] = "رنگ با موفقیت حذف شد.";
        return RedirectToAction(nameof(ColorList));
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
        if (!ModelState.IsValid) return View(color);

        await productColorService.AddNewColor(color);
        TempData[SuccessMessage] = "رنگ با موفقیت اضافه شد.";
        return RedirectToAction(nameof(ColorList));
    }

    #endregion

    #region Add Product Color

    [HttpGet]
    [InvokePermission(PermissionName.ProductManagement)]
    public async Task<IActionResult> AddProductColor(int productId)
    {
        ViewData["Colors"] = await productColorService.GetColorList();
        return View();
    }

    [HttpPost]
    [InvokePermission(PermissionName.ProductManagement)]
    public async Task<IActionResult> AddProductColor(AddProductColorViewModel model, int productId)
    {
        if (!ModelState.IsValid) return View(model);

        var res = await productColorService.AddColorToProduct(model, productId);
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

    #endregion
}