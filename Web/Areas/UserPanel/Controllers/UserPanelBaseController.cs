using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.UserPanel.Controllers;

[Area("UserPanel")]

public class UserPanelBaseController : Controller
{
    protected const string SuccessMessage = "Success";
    protected const string ErrorMessage = "Error";
    protected const string WarningMessage = "Warning";
}