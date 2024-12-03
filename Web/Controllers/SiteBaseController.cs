using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class SiteBaseController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}