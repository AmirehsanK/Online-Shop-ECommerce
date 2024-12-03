using Application.Security;
using Application.Services.Interfaces;
using Domain.Entities.Account;
using Domain.Interface;
using Domain.ViewModel;
using Domain.ViewModel.User;
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

    [HttpGet("login")]
    public IActionResult Login()
    {
        return View();
    }

    public async Task<IActionResult> UserLogin(LoginUserViewModel user)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("Login");
        }
        await _userService.LoginAsync(user);
        return RedirectToRoute("Dashboard");
    }

    #endregion

    #region Register

   
    [HttpGet("signup")]
    public IActionResult SignUp()
    {
        return View();
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignupUser(RegisterUserViewModel register)
    {
        #region Validation

        if (!ModelState.IsValid)
            return View("SignUp", register);
       // if(await _userRepository.IsEmailExistAsync(register.Email)){
           // ModelState.AddModelError("Email", "ایمیل وارد شده تکراری میباشد");
           // return View("SignUp", register);
       // }

        #endregion
        TempData["Name"] = register.FirstName;
        await _userService.RegisterUserAsync(register);
        return RedirectToAction(nameof(Success));
    }

    public IActionResult Success()
    {
        var x=TempData["Name"];
        return View(x);
    }
    #endregion
    
    #region ForgotPassword
    
    
    
    #endregion
}