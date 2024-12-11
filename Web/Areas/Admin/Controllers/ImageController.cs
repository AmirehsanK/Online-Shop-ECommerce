using Application.Services.Interfaces;
using Domain.Enums;
using Domain.ViewModel.Image;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers;

public class ImageController : AdminBaseController
{
    private readonly IFileHandleService _fileHandleService;

    public ImageController(IFileHandleService fileHandleService)
    {
        _fileHandleService = fileHandleService;
    }

    #region Slider Images

    public async Task<IActionResult> ImageSlider()
    {
        var banners = await _fileHandleService.GetAllBanner();
        return View(banners);
    }

    [HttpGet]
    public async Task<IActionResult> AddBanner()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddBanner(BannerViewModel banner)
    {
        await _fileHandleService.AddBanner(banner);
        TempData[SuccessMessage] = "عکس با موفقیت اضافه شد";
        return RedirectToAction("ImageSlider");
    }

    public async Task<IActionResult> Details(string title)
    {
        if (!ModelState.IsValid)
            return RedirectToAction("ImageSlider");

        var banner = await _fileHandleService.GetBanner(title);
        return View(banner);
    }

    public async Task<IActionResult> Update(string guid)
    {
        if (!ModelState.IsValid)
            return RedirectToAction("ImageSlider");

        var banner = await _fileHandleService.GetBanner(guid);
        await _fileHandleService.UpdateBanner(banner);
        return View(banner);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string guid)
    {
        if (!ModelState.IsValid)
        {
            TempData["error"] = "فرایند با خطا مواجه شد";
            return RedirectToAction("ImageSlider");
        }

        await _fileHandleService.DeleteBanner(guid);
        TempData[SuccessMessage] = "حذف عکس با موفقیت انجام شد";
        return RedirectToAction(nameof(ImageSlider));
    }

    #endregion

    #region Fixed Images
    
    public async Task<IActionResult> FixedImages()
    {
        var banners = await _fileHandleService.GetAllFixedBanners();
        return View(banners);
    }
    [HttpGet]
    public async Task<IActionResult> AddFixedBanner()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> AddFixedBanner(BannerFixViewModel banner)
    {
        if (banner.Image == null)
        {
            TempData[ErrorMessage] = "فرایند با خطا مواجه شد";
            return View();
        };
        await _fileHandleService.AddFixedBanner(banner);
        return View();
    }

    public async Task<IActionResult> Edit(string guid)
    {
        var banner = await _fileHandleService.GetFixedBanner(guid);
        if (banner == null)
        {
            TempData[ErrorMessage] = "عملیات با شکست مواجه شد";
            return View();
        }
        
        return View(banner);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(BannerFixViewModel banner)
    {
        if (!ModelState.IsValid)
            return View(banner);

        var result = await _fileHandleService.UpdateFixedBanner(banner);
        if (result == ImageEnum.Status.Success)
        {
            TempData[SuccessMessage] = "عملیات با موفقیت انجام شد";
            return RedirectToAction(nameof(FixedImages));
        }

        TempData[ErrorMessage] = "عملیات با شکست مواجه شد";
        return View(banner);
    }
    
    #endregion
}