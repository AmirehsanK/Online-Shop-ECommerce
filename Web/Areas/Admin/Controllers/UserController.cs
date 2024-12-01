using Application.Services.Interfaces;
using Domain.ViewModel.User;
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

        [HttpGet]
        public async Task<IActionResult> UserList(UserListViewModel model)
        {
            var models = await _userService.GetUserListAsync();
            return View(models);
        }
    }
}
