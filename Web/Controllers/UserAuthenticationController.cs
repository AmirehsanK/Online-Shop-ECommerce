using System.Security.Claims;
using Application.Services.Interfaces;
using Domain.Enums;
using Domain.ViewModel.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class UserAuthenticationController : SiteBaseController
{
    //TODO Forgot Password


    #region Logout

    public IActionResult LogOut()
    {
        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Redirect("/");
    }

    #endregion

    #region ctor

    private readonly IUserService _userService;

    public UserAuthenticationController(IUserService userService)
    {
        _userService = userService;
    }

    #endregion

    #region Login

    [Route("Login")]
    public IActionResult Login()
    {
        if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");

        return View();
    }

    [Route("Login")]
    [HttpPost]
    public async Task<IActionResult> Login(LoginUserViewModel login)
    {
        #region Validation

        if (!ModelState.IsValid) return View(login);

        #endregion

        var result = await _userService.LoginUserAsync(login);
        switch (result)
        {
            case LoginUserEnum.PasswordInvalid:
            {
                TempData[FailMessage] = "نام کاربری یا رمز عبور شما اشتباه است";
                return View(login);
            }
                break;
            case LoginUserEnum.EmailInvalid:
            {
                TempData[FailMessage] = "نام کاربری یا رمز عبور شما اشتباه است";
                return View(login);
            }
                break;
            case LoginUserEnum.Success:
            {
                var user = await _userService.GetUserByEmailAsync(login.Email);
                var claims = new List<Claim>
                {
                    new(ClaimTypes.Name, user.FirstName ?? "کاربر"),
                    new(ClaimTypes.Email, user.Email),
                    new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new("IsAdmin", user.IsAdmin.ToString())
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                var properties = new AuthenticationProperties
                {
                    IsPersistent = login.RememberMe
                };

                await HttpContext.SignInAsync(principal, properties);
                TempData[SuccesMessage] = "کاربر گرامی به سایت خوش امدید";
                return Redirect("/");
            }
                break;
            case LoginUserEnum.UserNotActive:
                TempData[FailMessage] = "اکانت شما غیرفعال میباشد";
                return View(login);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return View(login);
    }

    #endregion

    #region Register

    [HttpGet("signup")]
    public IActionResult SignUp()
    {
        if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");

        return View();
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignupUser(RegisterUserViewModel register)
    {
        #region Validation

        var result = await _userService.RegisterUserValidationAsync(register);
        switch (result)
        {
            case RegisterUserEnum.EmailUsed:
            {
                TempData[FailMessage] = "ایمیل تکراری است";
                return View("SignUp");
            }
                break;
            case RegisterUserEnum.Success:
            {
                TempData["Name"] = register.FirstName;
                await _userService.RegisterUserAsync(register);
                return RedirectToAction(nameof(Success));
            }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        #endregion
    }

    [Route("Success")]
    public IActionResult Success()
    {
        var userName = TempData["Name"];
        return View(userName);
    }

    [Route("EmailActive/{emailActiveCode}")]
    public async Task<IActionResult> EmailActive(string emailActiveCode)
    {
        var result = await _userService.EmailActivatorAsync(emailActiveCode);
        if (result == ActiveEmailEnum.Failed)
        {
            TempData[SuccesMessage] = "اکانت شما با موفقیت فعال سازی شد";
            return RedirectToAction(nameof(Login));
        }
        else
        {
            return RedirectToAction(nameof(Login));
        }
    }
    
    #endregion
}