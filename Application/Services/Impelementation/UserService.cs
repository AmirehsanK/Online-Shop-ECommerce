using Application.Security;
using Application.Services.Interfaces;
using Application.Tools;
using Domain.Entities.Account;
using Domain.Enums;
using Domain.Interface;
using Domain.ViewModel.User;
using Domain.ViewModel.User.Admin;

namespace Application.Services.Impelementation;

public class UserService(IUserRepository userRepository) : IUserService
{
    #region User List

    public async Task<List<UserListViewModel>> GetUserListAsync()
    {
        var users = await userRepository.GetAllAsync();
        return users.Where(u => u.IsDeleted == false).Select(u => new UserListViewModel
        {
            Id = u.Id,
            FirstName = u.FirstName,
            LastName = u.LastName,
            Email = u.Email,
            IsDeleted = u.IsDeleted
        }).ToList();
    }

    #endregion

    #region User Creation

    public async Task<CreateUserEnums> CreateUserAsync(CreateUserViewModel model)
    {
        model.Email = model.Email.ToLower().Trim();
        var exist = await userRepository.GetUserByEmailAsync(model.Email);
        if (exist != null!) return CreateUserEnums.EmailExist;
        var user = new User
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            IsAdmin = model.IsAdmin,
            EmailActiveCode = Guid.NewGuid().ToString("N"),
            CreateDate = DateTime.Now,
            IsDeleted = false,
            IsEmailActive = model.IsEmailActive,
            Password = model.Password
        };
        await userRepository.AddUserAsync(user);
        await userRepository.SaveChangesAsync();
        return CreateUserEnums.Success;
    }

    #endregion

    #region Roles

    public async Task<List<UserWithRolesViewModel>> GetAllUsersForRolesAsync()
    {
        return await userRepository.GetAllUsersForRolesAsync();
    }

    #endregion

    #region Email

    public async Task<bool> IsEmailExistAsync(string email)
    {
        return await userRepository.IsEmailExistAsync(email);
    }

    #endregion

    #region Email Activation

    public async Task<ActiveEmailEnum> EmailActivatorAsync(string emailActiveCode)
    {
        var user = await userRepository.GetUserByGUIDAsync(emailActiveCode);
        if (user == null!)
            return ActiveEmailEnum.Failed;
        user.IsEmailActive = true;
        user.EmailActiveCode = Guid.NewGuid().ToString("N");
        await userRepository.SaveChangesAsync();
        return ActiveEmailEnum.Success;
    }

    #endregion

    #region User Login

    public async Task<LoginUserEnum> LoginUserAsync(LoginUserViewModel model)
    {
        var user = await userRepository.GetUserByEmailAsync(model.Email);
        if (user != null!)
        {
            if (PasswordHasher.VerifyHashedPassword(user.Password, model.Password))
                return user.IsEmailActive ? LoginUserEnum.Success : LoginUserEnum.UserNotActive;

            return LoginUserEnum.PasswordInvalid;
        }

        return LoginUserEnum.EmailInvalid;
    }

    #endregion

    #region User Deletion

    public async Task DeleteUserAsync(int userid)
    {
        var user = await userRepository.GetUserByIdAsync(userid);
        user.IsDeleted = true;
        userRepository.UpdateUser(user);
        await userRepository.SaveChangesAsync();
    }

    #endregion

    #region User Details

    public async Task<UserDetailViewModel> GetUserDetailAsync(int userid)
    {
        var user = await userRepository.GetUserByIdAsync(userid);
        var detail = new UserDetailViewModel
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            IsAdmin = user.IsAdmin,
            CreatedDate = user.CreateDate,
            Id = userid,
            IsActive = user.IsEmailActive
        };
        return detail;
    }

    #endregion

    #region Password

    public async Task<bool> IsPasswordCorrectAsync(string email, string password)
    {
        var x = await userRepository.GetUserByEmailAsync(email);
        return PasswordHasher.VerifyHashedPassword(x.Password, password);
    }

    public async Task ChangePasswordAsync(int userId, string newPassword)
    {
        var user = await userRepository.GetUserByIdAsync(userId);

        if (user == null)
            throw new Exception("User not found");

        user.Password = PasswordHasher.HashPassword(newPassword);

        userRepository.UpdateUser(user);
        await userRepository.SaveChangesAsync();
    }

    public async Task<ForgetPasswordEnum> ForgotPasswordEmailSenderAsync(string email)
    {
        var user = await userRepository.GetUserByEmailAsync(email);
        if (user == null!)
            return ForgetPasswordEnum.UserNotFound;
        const string domainLink = "https://localhost:7271";
        var mailBody = $"<a href=\"{domainLink}/ForgetPassword/{user.EmailActiveCode}\"> فراموشی رمز عبور</a>";
        await EmailSender.SendEmail(user.Email, "فراموشی رمز عبور", mailBody);
        return ForgetPasswordEnum.Success;
    }

    public async Task<ForgetPasswordTokenCheckEnum> ForgotPasswordTokenCheckerAsync(string token)
    {
        var exist = await userRepository.IsExistUserByGuidAsync(token);
        return exist ? ForgetPasswordTokenCheckEnum.Success : ForgetPasswordTokenCheckEnum.Failed;
    }

    public async Task ResetPasswordAsync(string token, string newPassword)
    {
        var user = await userRepository.GetUserByGUIDAsync(token);
        user.Password = PasswordHasher.HashPassword(newPassword);
        userRepository.UpdateUser(user);
        await userRepository.SaveChangesAsync();
    }

    public bool ComparePasswordAsync(string hashedPassword, string providedPassword)
    {
        return PasswordHasher.VerifyHashedPassword(hashedPassword, providedPassword);
    }

    #endregion

    #region User Retrieval

    public async Task<User> GetUserByEmailAsync(string email)
    {
        return await userRepository.GetUserByEmailAsync(email);
    }

    public async Task<User> GetUserById(int userid)
    {
        return await userRepository.GetUserByIdAsync(userid);
    }

    #endregion

    #region User Registration

    public async Task RegisterUserAsync(RegisterUserViewModel model)
    {
        var user = new User
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            Password = PasswordHasher.HashPassword(model.Password),
            PhoneNumber = model.PhoneNumber,
            IsAdmin = false,
            IsEmailActive = false,
            IsDeleted = false,
            CreateDate = DateTime.UtcNow,
            EmailActiveCode = Guid.NewGuid().ToString("N")
        };
        const string domainLink = "https://localhost:7271";
        var mailBody = $"<a href=\"{domainLink}/EmailActive/{user.EmailActiveCode}\"> فعالسازی حساب کاربری </a>";
        await EmailSender.SendEmail(user.Email, "فعال سازی حساب کاربری", mailBody);
        await userRepository.AddUserAsync(user);
        await userRepository.SaveChangesAsync();
    }

    public async Task<RegisterUserEnum> RegisterUserValidationAsync(RegisterUserViewModel model)
    {
        if (await IsEmailExistAsync(model.Email)) return RegisterUserEnum.EmailUsed;

        return RegisterUserEnum.Success;
    }

    #endregion

    #region User Editing

    public async Task EditUserAsync(EditUserViewModel model)
    {
        var user = await userRepository.GetUserByIdAsync(model.Id);

        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.Email = model.Email;
        user.IsAdmin = model.IsAdmin;
        user.IsEmailActive = model.IsEmailActive;
        user.Address = model.Address;
        user.Address = model.Address;
        user.Password = !string.IsNullOrEmpty(model.Password?.Trim())
            ? PasswordHasher.HashPassword(model.Password)
            : user.Password;
        userRepository.UpdateUser(user);
        await userRepository.SaveChangesAsync();
    }

    public async Task<EditUserViewModel> GetUsersByIDAsync(int userid)
    {
        var user = await userRepository.GetUserByIdAsync(userid);
        var edit = new EditUserViewModel
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            IsAdmin = user.IsAdmin,
            IsEmailActive = user.IsEmailActive,
            Password = user.Password,
            Id = userid,
            Address = user.Address,
            PhoneNumber = user.PhoneNumber
        };
        return edit;
    }

    #endregion
}