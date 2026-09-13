using Application.Services.Interfaces;
using Infra.Data.Statics;
using Microsoft.AspNetCore.Mvc;
using Web.Attributes;

namespace Web.Areas.Admin.Controllers;

[InvokePermission(PermissionName.AdminPanel)]
public class HomeController(IAdminService adminService) : AdminBaseController
{
    #region Index

    public async Task<IActionResult> Index()
    {
        var model = await adminService.GetAdminPanelAsync();
        return View(model);
    }

    #endregion
}
