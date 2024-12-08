using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers;

public class AdminContactUsController : AdminBaseController
{
    public IActionResult Index()
    {
        return View();
    }
}