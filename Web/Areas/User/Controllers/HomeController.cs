using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.User.Controllers;
[Area("User")]
[Authorize]
public class HomeController : Controller
{
    
    public IActionResult Index()
    {
        return View();
    }
    [AllowAnonymous]
    public IActionResult Failed()
    {
        return View();
    }
}