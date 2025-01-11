using Domain.ViewModel.Permission;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.ViewComponents.PermissionViewComponent;

public class PermissionViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(List<PermissionSelectionViewModel> permissions)
    {
        return View(permissions);
    }
}