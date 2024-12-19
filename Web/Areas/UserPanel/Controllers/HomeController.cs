using System.Security.Claims;
using Application.Services.Interfaces;
using Application.Tools;
using Domain.Enums;
using Domain.ViewModel.User;
using Domain.ViewModel.User.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace Web.Areas.UserPanel.Controllers;

[Authorize]
public class HomeController : UserPanelBaseController
{
    #region Ctor

    private readonly IUserService _userService;
    private readonly INotificationService _notificationService;



    public HomeController(IUserService userService, INotificationService notificationService)
    {
        _userService = userService;
        _notificationService = notificationService;
    }

    #endregion


    public async Task<IActionResult> Index()
    {
        var res = await _notificationService.GetNotificationById(User.GetCurrentUserId());
        var publicmessage = await _notificationService.GetpublicMessage(User.GetCurrentUserId());
        switch (res)
        {
            case NotificationEnum.HasMessage:
                TempData[WarningMessage] = "کاربر عزیز یک پیام دارید";
                return View();

            case NotificationEnum.NotFound:
                if (publicmessage != null)
                {
                    TempData[WarningMessage] =  publicmessage.Message;
                    await _notificationService.markSeenForPrivateMessage(User.GetCurrentUserId(), publicmessage.Id);
                    return View();
                }
                return View();

        }



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


        if (!_userService.ComparePasswordAsync(user.Password, changePassword.OldPassword))
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


    #region UserNotification

    [HttpGet]
    public async Task<IActionResult> UserNotification()
    {
        var message = await _notificationService.GetShowNotificationById(User.GetCurrentUserId());
        if (message != null)
        {
            await _notificationService.markSeenForPrivateMessage(User.GetCurrentUserId(), message.MessageId);
        }

        return View(message);
    }

    #endregion
}