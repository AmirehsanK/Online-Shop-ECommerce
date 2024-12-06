using System.Security.Claims;
using Application.Services.Interfaces;
using Domain.Enums;
using Domain.ViewModel.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;

namespace Web.Controllers;

public class UserAuthenticationController : SiteBaseController
{
    #region ForgotPassword
    
    [HttpPost]
    public async Task<IActionResult> ForgotPassword(string email)
    {
        if (!ModelState.IsValid)
            return View();
        
        var result = await _userService.ForgotPasswordEmailSenderAsync(email);
        
        switch (result)
        {
            case ForgetPasswordEnum.Success:
                ViewData["IsSuccess"] = true;
                TempData[SuccesMessage] = "لینک تغییر رمز عبور با موفقیت به ایمیل شما ارسال شد";
                return View();

            case ForgetPasswordEnum.UserNotFound:
                ViewData["IsFailed"] = false;
                TempData[FailMessage] = "اکانتی با این ایمیل یافت نشد.";
                return View();
            
            case ForgetPasswordEnum.EmailSendFailed:
                ViewData["IsSuccess"] = false;
                TempData[FailMessage] ="ایمیل ارسال نشد لطفا دوباره تلاش کنید";
                return View();

            default:
                ViewData["IsSuccess"] = false;
                TempData[FailMessage] = "خطایی رخ داده است لطفا دوباره تلاش کنید";
                return View();
        }

        return View();
    }
    [HttpGet("ForgetPassword/{token}")]
    public async Task<IActionResult> ForgotPasswordChangePassword(string token)
    {
        var tokenEnum=await _userService.ForgotPasswordTokenCheckerAsync(token);
        TempData["Token"] = token;
        return tokenEnum switch
        {
            ForgetPasswordTokenCheckEnum.Success => View(),
            ForgetPasswordTokenCheckEnum.Failed => RedirectToAction("Login"),
            _ => RedirectToAction("Login")
        };
    }
    [HttpGet("ForgetPassword")]
    public async Task<IActionResult> ForgotPasswordChanger()
    {
        var token = TempData["Token"] as string;
        var user = await _userService.EmailActivatorAsync(token);
        return RedirectToAction("Login");
    }
    [HttpPost]
    public async Task<IActionResult> ForgotPasswordChanger(ForgetPasswordUserViewModel model)
    {
        var token = TempData["Token"] as string;
        await _userService.ResetPasswordAsync(token, model.NewPassword);
        TempData["ForgetPassword"] = true;
        return RedirectToAction("Login");
    }
    #endregion
    #region Logout

    public async Task<IActionResult> LogOut()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
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
        ViewBag.ForgotPassword = TempData["ForgotPassword"];
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
            case LoginUserEnum.UserNotActive:
                TempData[FailMessage] = "اکانت شما غیرفعال میباشد";
                return View(login);
            default: 
                return View(login);
        }
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
            case RegisterUserEnum.Success:
            {
                TempData["Name"] = register.FirstName;
                await _userService.RegisterUserAsync(register);
                return RedirectToAction(nameof(Success));
            }

            default: 
                return View("SignUp");
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
        TempData[SuccesMessage] = result == ActiveEmailEnum.Failed 
            ? "اکانت شما با موفقیت فعال سازی شد" 
            : null;
        return RedirectToAction(nameof(Login));
    }
    
    #endregion
}