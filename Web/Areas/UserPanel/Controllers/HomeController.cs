using System.Security.Claims;
using Application.Security;
using Application.Services.Interfaces;
using Domain.Entities.Account;
using Domain.ViewModel.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.UserPanel.Controllers
{
    [Authorize]
    public class HomeController(IUserService userService) : UserPanelBaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("UserPanel/ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordUserViewModel changePassword)
        {
            if (!ModelState.IsValid)
                return View(changePassword);

            int currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var user = await userService.GetUsersByIDAsync(currentUserId);

            
            if (!await userService.ComparePasswordAsync(user.Password, changePassword.OldPassword))
            {
                ModelState.AddModelError("oldPassword", "کلمه عبور فعلی صحیح نمیباشد");
                return View(changePassword);
            }
            
            await userService.ChangePasswordAsync(currentUserId, changePassword.NewPassword);
            
            ViewBag.IsSuccess = true;
            return View();
        }
    }
}