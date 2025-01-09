using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers;

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