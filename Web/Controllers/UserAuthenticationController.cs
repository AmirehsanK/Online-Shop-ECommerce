using System.Security.Claims;
using Application.Security;
using Application.Services.Interfaces;
using Domain.Entities.Account;
using Domain.Interface;
using Domain.ViewModel;
using Domain.ViewModel.User;
using Domain.ViewModel.User.Admin;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class UserAuthenticationController : Controller
{
    #region ctor

    private IUserService _userService;
    public UserAuthenticationController(IUserService userService)
    {
        _userService= userService;
    }


    #endregion
    
    #region Login

    [Route("Login")]
    public IActionResult Login()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }

    [Route("Login")]
    [HttpPost]
    public async Task<IActionResult> Login(LoginUserViewModel login)
    {
        if (!ModelState.IsValid)
        {
            return View(login);
        }

        var user = await _userService.GetUserByEmailAsync(login.Email);
        if (user == null)
        {
            ModelState.AddModelError("UserNameOrEmail","کاربری یافت نشد");
            return View(login);
        }
        if(!await _userService.IsPasswordCorrectAsync(login.Email,login.Password))
        {
            ModelState.AddModelError("UserNameOrEmail", "کاربری یافت نشد");
            return View(login);
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim("IsAdmin", user.IsAdmin.ToString())
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        var properties = new AuthenticationProperties
        {
            IsPersistent = login.RememberMe
        };

        await HttpContext.SignInAsync(principal, properties);

        return Redirect("/");
           
    }

    #endregion

    #region Register

   
    [HttpGet("signup")]
    public IActionResult SignUp()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignupUser(RegisterUserViewModel register)
    {
        #region Validation

        if (!ModelState.IsValid)
            return View("SignUp", register);
        if(await _userService.IsEmailExistAsync(register.Email)){
           ModelState.AddModelError("Email", "ایمیل وارد شده تکراری میباشد");
           return View("SignUp", register);
        }

        #endregion
        TempData["Name"] = register.FirstName;
        await _userService.RegisterUserAsync(register);
        return RedirectToAction(nameof(Success));
    }
    [Authorize]
    [Route("Success")]
    public IActionResult Success()
    {
        var x=TempData["Name"];
        return View(x);
    }
    #endregion
    //TODO Forgot Password
    #region ForgotPassword
    
    
    
    #endregion
    
    
    #region Logout
    public IActionResult LogOut()
    {
        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Redirect("/");
    }
    #endregion
}