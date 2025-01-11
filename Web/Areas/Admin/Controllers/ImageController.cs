using Application.Services.Interfaces;
using Domain.Enums;
using Domain.ViewModel.Image;
using Infra.Data.Statics;
using Microsoft.AspNetCore.Mvc;
using Web.Attributes;

namespace Web.Areas.Admin.Controllers;

[InvokePermission(PermissionName.SliderImageManagement)]
public class ImageController(IFileHandleService fileHandleService) : AdminBaseController
{
    #region Slider Images

    [InvokePermission(PermissionName.SliderImageList)]
    public async Task<IActionResult> ImageSlider()
    {
        var banners = await fileHandleService.GetAllBanner();
        return View(banners);
    }

    [HttpGet]
    [InvokePermission(PermissionName.CreateSliderImage)]
    public IActionResult AddBanner()
    {
        return View();
    }

    [HttpPost]
    [InvokePermission(PermissionName.CreateSliderImage)]
    public async Task<IActionResult> AddBanner(BannerViewModel banner)
    {
        await fileHandleService.AddBanner(banner);
        TempData[SuccessMessage] = "عکس با موفقیت اضافه شد";
        return RedirectToAction("ImageSlider");
    }

    [InvokePermission(PermissionName.SliderImageList)]
    public async Task<IActionResult> Details(string title)
    {
        if (!ModelState.IsValid)
            return RedirectToAction("ImageSlider");

        var banner = await fileHandleService.GetBanner(title);
        return View(banner);
    }

    [HttpPost]
    [InvokePermission(PermissionName.UpdateSliderImage)]
    public async Task<IActionResult> Update(BannerViewModel model)
    {
        if (!ModelState.IsValid)
            return RedirectToAction("ImageSlider");

        await fileHandleService.UpdateBanner(model);
        TempData[SuccessMessage] = "ویرایش با موفقیت انجام شد";
        return RedirectToAction(nameof(ImageSlider));
    }

    [HttpPost]
    [InvokePermission(PermissionName.DeleteSliderImage)]
    public async Task<IActionResult> Delete(string title)
    {
        await fileHandleService.DeleteBanner(title);
        TempData[SuccessMessage] = "حذف عکس با موفقیت انجام شد";
        return RedirectToAction(nameof(ImageSlider));
    }

    #endregion

    #region Fixed Images

    [InvokePermission(PermissionName.FixedBannerList)]
    public async Task<IActionResult> FixedImages()
    {
        var banners = await fileHandleService.GetAllFixedBanners();
        return View(banners);
    }

    [HttpGet]
    [InvokePermission(PermissionName.CreateFixedBanner)]
    public IActionResult AddFixedBanner()
    {
        return View();
    }

    [HttpPost]
    [InvokePermission(PermissionName.CreateFixedBanner)]
    public async Task<IActionResult> AddFixedBanner(BannerFixViewModel banner)
    {
        if (banner.Image == null)
        {
            TempData[ErrorMessage] = "فرایند با خطا مواجه شد";
            return View();
        }

        if (await fileHandleService.GetBannerByPosition(banner.Position) != null!)
            TempData[ErrorMessage] = "مکان عکس تکراری میباشد";

        await fileHandleService.AddFixedBanner(banner);
        TempData[SuccessMessage] = "عکس با موفقیت اضافه شد";
        return RedirectToAction(nameof(FixedImages));
    }

    [HttpGet]
    [InvokePermission(PermissionName.UpdateFixedBanner)]
    public async Task<IActionResult> Edit(string title)
    {
        var banner = await fileHandleService.GetFixedBanner(title);
        if (banner.Title == null!)
        {
            TempData[ErrorMessage] = "عملیات با شکست مواجه شد";
            return RedirectToAction(nameof(FixedImages));
        }

        return View(banner);
    }

    [HttpPost]
    [InvokePermission(PermissionName.UpdateFixedBanner)]
    public async Task<IActionResult> EditBanner(BannerFixViewModel banner)
    {
        if (banner.Image == null && banner.Link == null!)
        {
            TempData[ErrorMessage] = "لطفا فیلد هارا با دقت پر کنید";
            return RedirectToAction(nameof(FixedImages));
        }

        var result = await fileHandleService.UpdateFixedBanner(banner);
        if (result == ImageEnum.Status.Success)
        {
            TempData[SuccessMessage] = "عملیات با موفقیت انجام شد";
            return RedirectToAction(nameof(FixedImages));
        }

        TempData[ErrorMessage] = "عملیات با شکست مواجه شد";
        return RedirectToAction(nameof(FixedImages));
    }

    [HttpPost]
    [InvokePermission(PermissionName.DeleteFixedBanner)]
    public async Task<IActionResult> DeleteFixedImage(string title)
    {
        await fileHandleService.DeleteFixedBanner(title);
        TempData[SuccessMessage] = "با موفقیت حذف شد";
        return RedirectToAction(nameof(FixedImages));
    }

    #endregion
}