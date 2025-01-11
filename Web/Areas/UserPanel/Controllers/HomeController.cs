using System.Security.Claims;
using Application.Services.Interfaces;
using Application.Tools;
using Domain.Enums;
using Domain.ViewModel.Favorites;
using Domain.ViewModel.User;
using Domain.ViewModel.User.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.UserPanel.Controllers;

[Authorize]
public class HomeController(IUserService userService,
    INotificationService notificationService,
    ICommentService commentService,
    IDiscountService discountService,
    IFavoritesService favoritesService)
    : UserPanelBaseController
{
    
    #region Index

    public async Task<IActionResult> Index()
    {
        var res = await notificationService.GetNotificationById(User.GetCurrentUserId());
        var publicMessage = await notificationService.GetpublicMessage(User.GetCurrentUserId());
        var favoriteList = await favoritesService.GetFavoriteProductsAsync(User.GetCurrentUserId());

        ViewBag.Favorites = favoriteList;

        switch (res)
        {
            case NotificationEnum.HasMessage:
                TempData[WarningMessage] = "کاربر عزیز یک پیام دارید";
                return View();

            case NotificationEnum.NotFound:
                if (publicMessage == null!) return View();
                TempData[WarningMessage] = publicMessage.Message;
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

    #region UserNotification

    [HttpGet]
    public async Task<IActionResult> UserNotification()
    {
        var message = await notificationService.GetShowNotificationById(User.GetCurrentUserId());
        if (message != null!)
            await notificationService.markSeenForPrivateMessage(User.GetCurrentUserId(), message.MessageId);

        return View(message);
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

        if (!ModelState.IsValid) return View(model);

        #endregion

        await userService.EditUserAsync(model);
        return View();
    }

    #endregion

    #region Comments

    public async Task<IActionResult> Comments()
    {
        var comments=await commentService.GetCommentsByUserIdAsync(User.GetCurrentUserId());
        return View(comments);
    }

    #endregion

    #region Gift Codes

    public async Task<IActionResult> GiftCodes()
    {
        var model = await discountService.GetUserGiftCodes(User.GetCurrentUserId());
        return View(model);
    }

    #endregion

    #region Favorite List

    [HttpGet]
    public async Task<IActionResult> FavoritesList()
    {
        var favoriteProducts = await favoritesService.GetFavoriteProductsAsync(User.GetCurrentUserId());
        return View(favoriteProducts);
    }
    [HttpPost]
    public async Task<IActionResult> AddFavorite(int productId)
    {
        var model = new AddFavoriteProductViewModel()
        {
            ProductId = productId,
            UserId = User.GetCurrentUserId()
        };
        if (!ModelState.IsValid)
            return BadRequest("یک مشکلی پیش آمده!");
        try
        {
            await favoritesService.AddFavoriteAsync(model.UserId, model.ProductId);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest("این کالا در لیست مورد علاقه شما حضور دارد!");
        }
        catch (Exception)
        {
            return StatusCode(500,"یک مشکلی پیش آمده!");
        }
    }

    [HttpPost]
    public async Task<IActionResult> RemoveFavorite(int productId)
    {
        var model = new RemoveFavoriteProductViewModel()
        {
            UserId = User.GetCurrentUserId(),
            ProductId = productId,
        };
        if (!ModelState.IsValid)
            TempData[ErrorMessage] = "مشکلی در حذف کالا پیش آمد!";
        await favoritesService.RemoveFavoriteAsync(model.UserId, model.ProductId);
        return RedirectToAction(nameof(FavoritesList));
    }

    #endregion
    
}