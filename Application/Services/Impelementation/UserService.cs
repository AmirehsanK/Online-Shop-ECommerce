using Application.Security;
using Application.Services.Interfaces;
using Application.Tools;
using Domain.Entities.Account;
using Domain.Enums;
using Domain.Interface;
using Domain.ViewModel.User;
using Domain.ViewModel.User.Admin;
using Microsoft.Extensions.Configuration;

namespace Application.Services.Impelementation;

public class UserService(IUserRepository userRepository,
    IConfiguration configuration,
    IEmailSender emailSender,
    IPasswordHasher passwordHasher
    ) : IUserService
{
    private readonly string _domainLink = configuration["ApplicationSettings:DomainLink"]!;

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
            Password = await passwordHasher.EncodePasswordAsync(model.Password)
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
            if (await passwordHasher.VerifyPasswordAsync(user.Password, model.Password))
            {
                // The plain password is only ever known here, so this is where hashes from
                // older formats or weaker settings get upgraded.
                if (passwordHasher.NeedsRehash(user.Password))
                {
                    user.Password = await passwordHasher.EncodePasswordAsync(model.Password);
                    userRepository.UpdateUser(user);
                    await userRepository.SaveChangesAsync();
                }

                return user.IsEmailActive ? LoginUserEnum.Success : LoginUserEnum.UserNotActive;
            }

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
            Password = await passwordHasher.EncodePasswordAsync(model.Password),
            PhoneNumber = model.PhoneNumber,
            IsAdmin = false,
            IsEmailActive = false,
            IsDeleted = false,
            CreateDate = DateTime.UtcNow,
            EmailActiveCode = Guid.NewGuid().ToString("N")
        };
        var mailBody = $"<a href=\"{_domainLink}/EmailActive/{user.EmailActiveCode}\"> فعالسازی حساب کاربری </a>";
        await emailSender.SendEmailAsync(user.Email, "فعال سازی حساب کاربری", mailBody);
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
        user.Password = !string.IsNullOrEmpty(model.Password?.Trim())
            ? await passwordHasher.EncodePasswordAsync(model.Password)
            : user.Password;
        userRepository.UpdateUser(user);
        await userRepository.SaveChangesAsync();
    }

    public async Task<EditUserViewModel> GetUserForEditAsync(int userid)
    {
        var user = await userRepository.GetUserByIdAsync(userid);
        var edit = new EditUserViewModel
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            IsAdmin = user.IsAdmin,
            IsEmailActive = user.IsEmailActive,
            // Never send the stored hash to a form: it came back on save as a "new"
            // password and was hashed again, locking the user out.
            Password = null,
            Id = userid,
            Address = user.Address,
            PhoneNumber = user.PhoneNumber
        };
        return edit;
    }

    #endregion

    #region Profile

    /// <summary>
    /// Updates only what a customer may change about themselves. The profile form used to be
    /// bound straight into <see cref="EditUserAsync"/>, so a user could post IsAdmin=true or
    /// another person's email, and fields the form did not send (IsEmailActive) were reset
    /// to false - deactivating every account that saved its profile.
    /// </summary>
    public async Task UpdateProfileAsync(int userId, EditUserViewModel model)
    {
        var user = await userRepository.GetUserByIdAsync(userId);
        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.PhoneNumber = model.PhoneNumber;
        user.Address = model.Address;
        userRepository.UpdateUser(user);
        await userRepository.SaveChangesAsync();
    }

    #endregion
}
