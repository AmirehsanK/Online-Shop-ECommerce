using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.UserPanel.ViewComponents.UserProfile;

public class UserProfileViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View("UserProfile");
    }
}