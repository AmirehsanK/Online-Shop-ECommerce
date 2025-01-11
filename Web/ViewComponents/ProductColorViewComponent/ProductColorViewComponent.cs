using Microsoft.AspNetCore.Mvc;

namespace Web.ViewComponents.ProductColorViewComponent;

public class ProductColorViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View("ProductColor");
    }
}