using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class HomeController(IProductService productService) : SiteBaseController
{
    public async Task<IActionResult> Index()
    {
        var model = await productService.Get8MostDiscountedProducts();
        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }
}