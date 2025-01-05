using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Web.ViewComponents.CategorySliderViewComponent;

public class CategorySliderViewComponent(IProductService productService) : ViewComponent
{

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var model = await productService.GetAllSubCategoriesForSlider();
        return View(model);
    }
}
