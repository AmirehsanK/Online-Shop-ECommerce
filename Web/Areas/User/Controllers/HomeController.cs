using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.User.Controllers;
[Area("User")]
[Authorize]
public class HomeController : Controller
{
    [Route("User/Home")]
    public IActionResult Index()
    {
        return View();
    }
    [Route("Failed")]
    [AllowAnonymous]
    public IActionResult Failed()
    {
        return View();
    }
}