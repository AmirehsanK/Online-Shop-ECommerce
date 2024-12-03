using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.ViewComponents.AdminSideBarViewComponent;

public class AdminSideBarViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View("AdminSideBar");
    }
}