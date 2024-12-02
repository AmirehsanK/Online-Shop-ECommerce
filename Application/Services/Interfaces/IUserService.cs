using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.ViewModel.User;
using Domain.ViewModel.User.Admin;

namespace Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserListViewModel>> GetUserListAsync();
        Task RegisterUserAsync(RegisterUserViewModel model);
        Task<LoginUserViewModel> LoginAsync(LoginUserViewModel loginUser);
        Task CreateUserAsync(CreateUserViewModel model);
    }
}