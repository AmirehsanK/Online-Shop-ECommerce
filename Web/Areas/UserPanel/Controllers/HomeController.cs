using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.UserPanel.Controllers
{
    [Authorize]
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
}