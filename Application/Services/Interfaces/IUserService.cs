using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Enums;
using Domain.ViewModel.User;
using Domain.ViewModel.User.Admin;

namespace Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserListViewModel>> GetUserListAsync();
        Task RegisterUserAsync(RegisterUserViewModel model);
        Task<LoginUserViewModel> LoginAsync(LoginUserViewModel loginUser);
        Task<CreateUserEnums> CreateUserAsync(CreateUserViewModel model);

        Task EditUserAsync(EditUserViewModel model);

        Task<EditUserViewModel> GetUsersByIDAsync(int userid);

        Task DeleteUserAsync(int userid);

        Task<UserDetailViewModel> GetUserDetailAsync(int userid);
    }
}