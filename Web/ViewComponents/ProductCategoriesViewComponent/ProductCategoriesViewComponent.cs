using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Web.ViewComponents.ProductCategoriesViewComponent;

public class ProductCategoriesViewComponent : ViewComponent
{
    #region Ctor

    private readonly IProductService _productService;

    public ProductCategoriesViewComponent(IProductService productService)
    {
        _productService = productService;
    }

    #endregion

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var category = await _productService.GetAllCategoriesForMegaMenu();
        return View("ProductCategories",category);
    }

}