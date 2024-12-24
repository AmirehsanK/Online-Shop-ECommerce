using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers
{
    public class QuestionController : AdminBaseController
    {
        public async Task<IActionResult> QuestionList()
        {
            return View();
        }
    }
}
