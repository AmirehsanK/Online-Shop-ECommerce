using Application.Services.Interfaces;
using Application.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.UserPanel.ViewComponents.UserProfile;

public class UserProfileViewComponent : ViewComponent
{
    #region Ctor

    private readonly IUserService _userService;

    public UserProfileViewComponent(IUserService userService)
    {
        _userService = userService;
    }

    #endregion


    [Authorize]
    public async Task<IViewComponentResult> InvokeAsync()
    {
        if (User.Identity.IsAuthenticated)
        {
            ViewData["User"] = await _userService.GetUserById(User.GetCurrentUserId());
        }

        return View("UserProfile");
    }
}