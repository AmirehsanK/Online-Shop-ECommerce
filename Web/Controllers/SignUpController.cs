using Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class SignUpController : Controller
{
    
    IUserRepository _userRepository;
    public SignUpController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public IActionResult SignUp()
    {
        return View();
    }
}