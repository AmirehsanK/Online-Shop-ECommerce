using Application.Services.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Web.ViewComponents.FixedImagesViewComponent;

public class FixedImagesViewComponent : ViewComponent
{
    private readonly IFileHandleService _fileHandleService;

    public FixedImagesViewComponent(IFileHandleService fileHandleService)
    {
        _fileHandleService = fileHandleService;
    }

    public async Task<IViewComponentResult> InvokeAsync(ImageEnum.Banner banner)
    {
        var subjects = await _fileHandleService.GetBannerByPosition(banner);
        return View(subjects);
    }
}
