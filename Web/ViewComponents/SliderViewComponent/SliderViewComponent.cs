using Application.Services.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Web.ViewComponents.SliderViewComponent
{
    public class SliderViewComponent:ViewComponent
    {
        private readonly IFileHandleService _fileHandleService;
        public SliderViewComponent(IFileHandleService fileHandleService)
        {
            _fileHandleService = fileHandleService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var images =await _fileHandleService.GetAllWorkingBanner();
            return View(images);
        }
    }
}
