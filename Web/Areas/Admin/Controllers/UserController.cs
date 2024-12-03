using Application.Services.Interfaces;
using Domain.ViewModel.User;
using Domain.ViewModel.User.Admin;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers
{
    public class UserController : AdminBaseController
    {
        #region Ctor

        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        #region USerList
        [HttpGet]
        public async Task<IActionResult> UserList()
        
        {
            var models = await _userService.GetUserListAsync();
            return View(models);
        }

        #endregion

        #region CreateUser

        
        [HttpGet]

        public async Task<IActionResult> CreateUser()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> CreateUser(CreateUserViewModel model)
        {
            #region Validation

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            #endregion

            await _userService.CreateUserAsync(model);
            return View();
        }

        #endregion

        #region EditUser

        [HttpGet]
        public async Task<IActionResult> EditUser(int UserId)
        {
            var model = await _userService.GetUsersByIDAsync(UserId);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            #region Validation

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            #endregion

            await _userService.EditUserAsync(model);
            return RedirectToAction(nameof(UserList));

        }
        #endregion

        #region Userdetail

        [HttpGet]
        public async Task<IActionResult> UserDetail(int userId)
        {
          var model=  await _userService.GetUserDetailAsync(userId);
            return View(model);
        }

        #endregion

        #region DeleteSUer

        [HttpGet]
        public async Task<IActionResult> DeleteUser(int UserId)
        {
            await _userService.DeleteUserAsync(UserId);
            return RedirectToAction("UserList");
        }

        #endregion


    }
}
