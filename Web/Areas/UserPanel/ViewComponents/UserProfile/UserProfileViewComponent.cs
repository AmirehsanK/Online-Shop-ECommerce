using Application.Services.Interfaces;
using Application.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.UserPanel.ViewComponents.UserProfile;

public class UserProfileViewComponent(IUserService userService, ITransactionService transactionService)
    : ViewComponent
{
    [Authorize]
    public async Task<IViewComponentResult> InvokeAsync()
    {
        if (User.Identity.IsAuthenticated)
        {
            ViewData["UserBalance"] = await transactionService.GetUserBalanceTransaction(User.GetCurrentUserId());
            ViewData["User"] = await userService.GetUserById(User.GetCurrentUserId());
        }

        return View("UserProfile");
    }
}