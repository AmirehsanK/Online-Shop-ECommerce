using Application.Services.Interfaces;
using Application.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.UserPanel.ViewComponents.UserProfile;

public class UserProfileViewComponent : ViewComponent
{
    #region Ctor

    private readonly IUserService _userService;
    private readonly ITransactionService _transactionService;

    public UserProfileViewComponent(IUserService userService, ITransactionService transactionService)
    {
        _userService = userService;
        _transactionService = transactionService;
    }

   

    #endregion


    [Authorize]
    public async Task<IViewComponentResult> InvokeAsync()
    {
        if (User.Identity.IsAuthenticated)
        {
            ViewData["UserBalance"] = await _transactionService.GetUserBalanceTransaction(User.GetCurrentUserId());
            ViewData["User"] = await _userService.GetUserById(User.GetCurrentUserId());

        }

        return View("UserProfile");
    }
}