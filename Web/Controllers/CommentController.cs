using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class CommentController : SiteBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
