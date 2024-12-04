using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers;

[Area("Admin")]
public class AdminBaseController : Controller
{
    public static string SuccessMessage = "Success";
    public static string ErrorMessage = "Error";
}