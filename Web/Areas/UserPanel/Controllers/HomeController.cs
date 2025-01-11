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
public class HomeController(IUserService userService, INotificationService notificationService)
    : UserPanelBaseController
{
    #region Index

    public async Task<IActionResult> Index()
    {
        var res = await notificationService.GetNotificationById(User.GetCurrentUserId());
        var publicMessage = await notificationService.GetpublicMessage(User.GetCurrentUserId());
        switch (res)
        {
            case NotificationEnum.HasMessage:
                TempData[WarningMessage] = "کاربر عزیز یک پیام دارید";
                return View();

            case NotificationEnum.NotFound:
                if (publicMessage == null!) return View();
                TempData[WarningMessage] =  publicMessage.Message;
                await notificationService.markSeenForPrivateMessage(User.GetCurrentUserId(), publicMessage.Id);
                return View();
        }



        return View();

    }
    
    #endregion

    #region ChangePassword

    [HttpPost("UserPanel/ChangePassword")]
    public async Task<IActionResult> ChangePassword(ChangePasswordUserViewModel changePassword)
    {
        if (!ModelState.IsValid)
            return View(changePassword);

        var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var user = await userService.GetUsersByIDAsync(currentUserId);


        if (!userService.ComparePasswordAsync(user.Password!, changePassword.OldPassword))
        {
            ModelState.AddModelError("oldPassword", "کلمه عبور فعلی صحیح نمیباشد");
            return View(changePassword);
        }

        await userService.ChangePasswordAsync(currentUserId, changePassword.NewPassword);

        ViewBag.IsSuccess = true;
        return View();


    }
    
    #endregion
    
    #region Profile Info

    [HttpGet]
    public async Task<IActionResult> UserInfo()
    {
        var model = await userService.GetUsersByIDAsync(User.GetCurrentUserId());
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

        await userService.EditUserAsync(model);
        return View();
    }

    #endregion
    
    #region UserNotification

    [HttpGet]
    public async Task<IActionResult> UserNotification()
    {
        var message = await notificationService.GetShowNotificationById(User.GetCurrentUserId());
        if (message != null!)
        {
            await notificationService.markSeenForPrivateMessage(User.GetCurrentUserId(), message.MessageId);
        }

        return View(message);
    }

    #endregion


    #region AddTransactionToWallet
    [HttpGet]
    public async Task<IActionResult> AddTransactionToWallet()
    {
        ViewData["UserId"]=User.GetCurrentUserId();
        return View();
    }

    #endregion
}