using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers;

public class OrderController : AdminBaseController
{
    [HttpGet]
    public IActionResult UserOrdersList()
    {
        return View();
    }
}