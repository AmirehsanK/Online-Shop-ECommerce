using Application.Services.Interfaces;
using Domain.Enums;
using Domain.ViewModel.User.Admin;
using Microsoft.AspNetCore.Mvc;
using Web.Attributes;
using Infra.Data.Statics;

namespace Web.Areas.Admin.Controllers;

[InvokePermission(PermissionName.UserManagement)]
public class UserController : AdminBaseController
{
    #region Ctor

    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    #endregion

    #region UserList

    [HttpGet]
    [InvokePermission(PermissionName.UserList)]
    public async Task<IActionResult> UserList()
    {
        var models = await _userService.GetUserListAsync();
        return View(models);
    }

    #endregion

    #region UserDetail

    [HttpGet]
    [InvokePermission(PermissionName.UserList)]
    public async Task<IActionResult> UserDetail(int userId)
    {
        var model = await _userService.GetUserDetailAsync(userId);
        return View(model);
    }

    #endregion

    #region DeleteUser

    [HttpGet]
    [InvokePermission(PermissionName.DeleteUser)]
    public async Task<IActionResult> DeleteUser(int UserId)
    {
        await _userService.DeleteUserAsync(UserId);
        return RedirectToAction("UserList");
    }

    #endregion

    #region CreateUser

    [HttpGet]
    [InvokePermission(PermissionName.CreateUser)]
    public IActionResult CreateUser()
    {
        return View();
    }

    [HttpPost]
    [InvokePermission(PermissionName.CreateUser)]
    public async Task<IActionResult> CreateUser(CreateUserViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var res = await _userService.CreateUserAsync(model);
        switch (res)
        {
            case CreateUserEnums.Success:
                TempData[SuccessMessage] = "کاربر مورد نظر با موفقیت اضافه شد";
                return RedirectToAction(nameof(UserList));
            case CreateUserEnums.EmailExist:
                TempData[ErrorMessage] = "ایمیل وارد شده موجود میباشد";
                return View();
        }

        return View();
    }

    #endregion

    #region EditUser

    [HttpGet]
    [InvokePermission(PermissionName.UpdateUser)]
    public async Task<IActionResult> EditUser(int UserId)
    {
        var model = await _userService.GetUsersByIDAsync(UserId);
        return View(model);
    }

    [HttpPost]
    [InvokePermission(PermissionName.UpdateUser)]
    public async Task<IActionResult> EditUser(EditUserViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        await _userService.EditUserAsync(model);
        TempData[SuccessMessage] = "اطلاعات با موفیت ویرایش شد";
        return RedirectToAction(nameof(UserList));
    }

    #endregion
}