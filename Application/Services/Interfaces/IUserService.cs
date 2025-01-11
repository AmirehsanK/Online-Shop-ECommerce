using Domain.Entities.Account;
using Domain.Enums;
using Domain.ViewModel.User;
using Domain.ViewModel.User.Admin;

namespace Application.Services.Interfaces;

public interface IUserService
{
    #region User List

    Task<List<UserListViewModel>> GetUserListAsync();

    #endregion

    #region Roles

    Task<List<UserWithRolesViewModel>> GetAllUsersForRolesAsync();

    #endregion

    #region Email

    Task<bool> IsEmailExistAsync(string email);

    #endregion

    #region Email Activation

    Task<ActiveEmailEnum> EmailActivatorAsync(string emailActiveCode);

    #endregion

    #region User Login

    Task<LoginUserEnum> LoginUserAsync(LoginUserViewModel model);

    #endregion

    #region User Deletion

    Task DeleteUserAsync(int userid);

    #endregion

    #region User Details

    Task<UserDetailViewModel> GetUserDetailAsync(int userid);

    #endregion

    #region Password Management

    Task<bool> IsPasswordCorrectAsync(string email, string password);
    bool ComparePasswordAsync(string hashedPassword, string providedPassword);
    Task ChangePasswordAsync(int userId, string newPassword);
    Task<ForgetPasswordEnum> ForgotPasswordEmailSenderAsync(string email);
    Task<ForgetPasswordTokenCheckEnum> ForgotPasswordTokenCheckerAsync(string token);
    Task ResetPasswordAsync(string token, string newPassword);

    #endregion

    #region User Retrieval

    Task<User> GetUserByEmailAsync(string email);
    Task<User> GetUserById(int userid);

    #endregion

    #region User Registration

    Task RegisterUserAsync(RegisterUserViewModel model);
    Task<CreateUserEnums> CreateUserAsync(CreateUserViewModel model);
    Task<RegisterUserEnum> RegisterUserValidationAsync(RegisterUserViewModel model);

    #endregion

    #region User Editing

    Task EditUserAsync(EditUserViewModel model);
    Task<EditUserViewModel> GetUsersByIDAsync(int userid);

    #endregion
}