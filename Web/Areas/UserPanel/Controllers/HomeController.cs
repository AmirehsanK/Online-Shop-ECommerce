using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.UserPanel.Controllers;
[Area("UserPanel")]
public class HomeController : Controller
{
    [Route("UserPanel")]
    public IActionResult Index()
    {
        return View();
    }
    [Route("UserPanel/ChangePassword")]
    public IActionResult ChangePassword()
    {
        return View();
    }
}