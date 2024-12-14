using System.Security.Claims;
using Application.Services.Interfaces;
using Application.Tools;
using Domain.ViewModel.User;
using Domain.ViewModel.User.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.UserPanel.Controllers;

[Authorize]
public class HomeController : UserPanelBaseController
{
    #region Ctor

    private readonly IUserService _userService;

    public HomeController(IUserService userService)
    {
        _userService = userService;
    }

    #endregion
    public IActionResult Index()
    {
        return View();
    }

    //TODO Fix Change Password logic
    [HttpPost("UserPanel/ChangePassword")]
    public async Task<IActionResult> ChangePassword(ChangePasswordUserViewModel changePassword)
    {
        if (!ModelState.IsValid)
            return View(changePassword);

        var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var user = await _userService.GetUsersByIDAsync(currentUserId);


        if (!await _userService.ComparePasswordAsync(user.Password, changePassword.OldPassword))
        {
            ModelState.AddModelError("oldPassword", "کلمه عبور فعلی صحیح نمیباشد");
            return View(changePassword);
        }

        await _userService.ChangePasswordAsync(currentUserId, changePassword.NewPassword);

        ViewBag.IsSuccess = true;
        return View();

       
    }
    #region Profile-Info

    [HttpGet]
    public async Task<IActionResult> UserInfo()
    {
        var model = await _userService.GetUsersByIDAsync(User.GetCurrentUserId());
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> UserInfo(EditUserViewModel model)
    {
        model.Id = User.GetCurrentUserId();
        #region Validation

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        #endregion

        await _userService.EditUserAsync(model);
        return View();
    }

    #endregion
}