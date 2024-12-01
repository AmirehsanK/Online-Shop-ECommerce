using Domain.Entities.Account;
using Domain.Interface;
using Infra.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class LoginController : Controller
{
    // GET
    IUserRepository _userRepository;
    public LoginController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    [HttpGet("login")]
    public IActionResult Login()
    {
        return View();
    }

    public IActionResult UserLogin(Users user)
    {
        return View(user);
    }
}