using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.UserPanel.Controllers;

[Area("UserPanel")]
[Route("user")]
public class UserPanelBaseController : Controller
{
    public static string SuccessMessage = "Success";
    public static string ErrorMessage = "Error";
    public static string WarningMessage = "Warning";
}