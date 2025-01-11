using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Web.ViewComponents.HeaderBottomViewComponent;

public class HeaderBottomViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var category = await _productService.GetAllCategoriesForMegaMenu();
        return View("Headerbottom", category);
    }

    #region Ctor

    private readonly IProductService _productService;

    public HeaderBottomViewComponent(IProductService productService)
    {
        _productService = productService;
    }

    #endregion
}