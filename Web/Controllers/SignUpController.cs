using Domain.Entities.Account;
using Domain.Interface;
using Domain.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Application.Security;
using Domain.ViewModel.User;

namespace Web.Controllers;

public class SignUpController : Controller
{
    
    IUserRepository _userRepository;
    public SignUpController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    [HttpGet("signup")]
    public IActionResult SignUp()
    {
        return View();
    }

    [HttpPost("signup")]
    public IActionResult Signup(RegisterUserViewModel register)
    {
        #region Validation

        if (!ModelState.IsValid) return View(register);
        //if (_userRepository.IsEmailExist(register.Email))
        //{
        //  ModelState.AddModelError("Email","ایمیل وارد شده تکراری میباشد");
        //return View(register);
        //}

        #endregion
        Users user = new Users()
        {
            CreateDate = DateTime.Now,
            Email = register.Email.Trim().ToLower(),
            IsAdmin = false,
            IsDeleted = false,
            Password = PasswordHasher.HashPassword(register.Password),
            FirstName = register.FirstName,
            LastName = register.LastName,
        };
        //_userRepository.AddUser(register);
        return View("Success", register);
    }
    
    
}