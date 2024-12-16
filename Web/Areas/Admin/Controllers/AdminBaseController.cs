using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers;

[Area("Admin")]
public class AdminBaseController : Controller
{
    public static string SuccessMessage = "Success";
    public static string ErrorMessage = "Error";

    protected IActionResult RedirectToRefererUrl()
    {
        var url = HttpContext.Request.Headers.Referer.ToString();
        if (Url.IsLocalUrl(url))
            return Redirect(url);

        return RedirectToAction("Index", "Home", new { area = "Admin" });
    }
}