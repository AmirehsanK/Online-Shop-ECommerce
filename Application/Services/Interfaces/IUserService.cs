using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities.Account;
using Domain.Enums;
using Domain.ViewModel.User;
using Domain.ViewModel.User.Admin;

namespace Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserListViewModel>> GetUserListAsync();

        Task<bool> IsPasswordCorrectAsync(string email, string password);

        bool ComparePasswordAsync(string hashedPassword, string providedPassword);

        Task ChangePasswordAsync(int userId, string newPassword);

        Task<ForgetPasswordEnum> ForgotPasswordEmailSenderAsync(string email);
        
        Task<ForgetPasswordTokenCheckEnum> ForgotPasswordTokenCheckerAsync(string token);
        Task ResetPasswordAsync(string token, string newPassword);        
        Task<User> GetUserByEmailAsync(string email);

        Task<bool> IsEmailExistAsync(string email);
        Task RegisterUserAsync(RegisterUserViewModel model);
        
        Task<CreateUserEnums> CreateUserAsync(CreateUserViewModel model);

        Task EditUserAsync(EditUserViewModel model);

        Task<EditUserViewModel> GetUsersByIDAsync(int userid);
        
        Task<ActiveEmailEnum> EmailActivatorAsync(string emailActiveCode);
        
        Task<LoginUserEnum> LoginUserAsync(LoginUserViewModel model);
        
        Task<RegisterUserEnum> RegisterUserValidationAsync(RegisterUserViewModel model);

        Task DeleteUserAsync(int userid);

        Task<UserDetailViewModel> GetUserDetailAsync(int userid);
        Task<User> GetUserById(int userid);

    }
}