using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Account;
using Domain.Enums;
using Domain.ViewModel.User.Admin;

namespace Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserListViewModel>> GetUserListAsync();
        Task CreateUserAsync(CreateUserViewModel model);

        Task EditUserAsync(EditUserViewModel model);

        Task<EditUserViewModel> GetUsersByIDAsync(int userid);

        Task DeleteUserAsync(int userid);

        Task<UserDetailViewModel> GetUserDetailAsync(int userid);
    }
}
