using Infra.Data.Statics;
using Microsoft.AspNetCore.Mvc;
using Web.Attributes;

namespace Web.Areas.Admin.Controllers;

[InvokePermission(PermissionName.AdminPanel)]
public class OrderController : AdminBaseController
{
    [HttpGet]
    public IActionResult UserOrdersList()
    {
        return View();
    }
}
