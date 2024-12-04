using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.UserPanel.Controllers
{
    public class HomeController : UserPanelBaseController
    {

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