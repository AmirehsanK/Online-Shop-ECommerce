using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers
{
    public class UserController : AdminBaseController
    {
        public async Task<IActionResult> UserList()
        {
            return View();
        }
    }
}
