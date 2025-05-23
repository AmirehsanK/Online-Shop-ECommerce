using System.Security.Claims;
using Application.Services.Interfaces;
using Application.Tools;
using Domain.Enums;
using Domain.ViewModel.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class UserAuthenticationController(IUserService userService, IConfiguration configuration) : SiteBaseController
{
    #region Logout

    public async Task<IActionResult> LogOut()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Redirect("/");
    }

    #endregion

    #region ForgotPassword

    [HttpGet("ForgotPassword")]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost("ForgotPassword")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordMailUserViewModel mailViewModel)
    {
        if (!ModelState.IsValid)
            return View();

        var result = await userService.ForgotPasswordEmailSenderAsync(mailViewModel.Email);
        switch (result)
        {
            case ForgetPasswordEnum.Success:
                TempData[SuccessMessage] = "لینک تغییر رمز عبور با موفقیت به ایمیل شما ارسال شد";
                return RedirectToAction("Login");

            case ForgetPasswordEnum.UserNotFound:
                ViewBag.message = "حساب کاربری با این ایمیل یافت نشد";
                return View();

            case ForgetPasswordEnum.EmailSendFailed:
                ViewBag.message = "خطایی رخ داده است لطفا دوباره تلاش کنید";
                return View();

            default:
                ViewBag.message = "خطایی رخ داده است لطفا دوباره تلاش کنید";
                return View();
        }
    }

    [HttpGet("ForgetPassword/{token}")]
    public async Task<IActionResult> ForgotPasswordChangePassword(string token)
    {
        var tokenEnum = await userService.ForgotPasswordTokenCheckerAsync(token);
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
        var user = await userService.EmailActivatorAsync(token!);
        return RedirectToAction("Login");
    }

    [HttpPost("ForgetPassword")]
    public async Task<IActionResult> ForgotPasswordChanger(ForgetPasswordUserViewModel model)
    {
        await userService.ResetPasswordAsync(model.ActivationCode, model.NewPassword);
        TempData[SuccessMessage] = "رمز عبور با موفقیت تغییر یافت";
        return RedirectToAction("Login");
    }

    #endregion

    #region Login

    [HttpGet("Login")]
    public IActionResult Login()
    {
        if (User.Identity!.IsAuthenticated) return RedirectToAction("Index", "Home");
        return View();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserViewModel login)
    {
        #region Validation

        if (!ModelState.IsValid) return View(login);

        var googleRecaptchaToken = Request.Form["g-recaptcha-response"].ToString();
        var secretKey = configuration["ReCaptchaSettings:SecretKey"]!;
        var verificationUrl = configuration["ReCaptchaSettings:VerificationUrl"]!;
        var isValid = await VerifyRecaptcha.VerifyRecaptchaV3(googleRecaptchaToken, secretKey, verificationUrl);
        if (!isValid)
        {
            TempData[ErrorMessage] = "کپچا را کامل کنید";
            return View();
        }

        #endregion

        var result = await userService.LoginUserAsync(login);
        switch (result)
        {
            case LoginUserEnum.PasswordInvalid:
            {
                TempData[ErrorMessage] = "نام کاربری یا رمز عبور شما اشتباه است";
                return View(login);
            }
            case LoginUserEnum.EmailInvalid:
            {
                TempData[ErrorMessage] = "نام کاربری یا رمز عبور شما اشتباه است";
                return View(login);
            }
            case LoginUserEnum.Success:
            {
                var user = await userService.GetUserByEmailAsync(login.Email);
                var claims = new List<Claim>
                {
                    new(ClaimTypes.Name, user.FirstName + " " + user.LastName),
                    new(ClaimTypes.Email, user.Email),
                    new(ClaimTypes.MobilePhone, user.PhoneNumber),
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
                TempData[SuccessMessage] = "کاربر گرامی به سایت خوش امدید";
                return Redirect("/");
            }
            case LoginUserEnum.UserNotActive:
                TempData[ErrorMessage] = "اکانت شما غیرفعال میباشد";
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
        if (User.Identity!.IsAuthenticated) return RedirectToAction("Index", "Home");

        return View();
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignupUser(RegisterUserViewModel register)
    {
        #region Validation

        var googleRecaptchaToken = Request.Form["g-recaptcha-response"].ToString();
        var secretKey = configuration["ReCaptchaSettings:SecretKey"]!;
        var verificationUrl = configuration["ReCaptchaSettings:VerificationUrl"]!;
        var isValid = await VerifyRecaptcha.VerifyRecaptchaV3(googleRecaptchaToken, secretKey, verificationUrl);
        if (!isValid)
        {
            TempData[ErrorMessage] = "کپچا را کامل کنید";
            return View("SignUp");
        }

        #endregion

        var result = await userService.RegisterUserValidationAsync(register);
        switch (result)
        {
            case RegisterUserEnum.EmailUsed:
            {
                TempData[ErrorMessage] = "ایمیل تکراری است";
                return View("SignUp");
            }
            case RegisterUserEnum.Success:
            {
                TempData["Name"] = register.FirstName;
                await userService.RegisterUserAsync(register);
                return RedirectToAction(nameof(Success));
            }

            default:
                return View("SignUp");
        }
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
        var result = await userService.EmailActivatorAsync(emailActiveCode);
        TempData[SuccessMessage] = result == ActiveEmailEnum.Failed
            ? "اکانت شما با موفقیت فعال سازی شد"
            : null;
        return RedirectToAction(nameof(Login));
    }

    #endregion
}