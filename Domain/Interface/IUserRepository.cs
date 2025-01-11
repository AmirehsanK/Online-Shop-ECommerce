using Domain.Entities.Account;
using Domain.ViewModel.User;

namespace Domain.Interface;

public interface IUserRepository
{
    #region Save Changes

    Task SaveChangesAsync();

    #endregion

    #region User Retrieval

    Task<List<User>> GetAllAsync();
    Task<User> GetUserByIdAsync(int userid);
    Task<string> GetUserNameByIdAsync(int userid);
    Task<User> GetUserByEmailAsync(string email);
    Task<List<UserWithRolesViewModel>> GetAllUsersForRolesAsync();
    Task<User> GetUserByGUIDAsync(string guid);
    Task<bool> IsExistUserByGuidAsync(string guid);
    Task<bool> IsEmailExistAsync(string email);

    #endregion

    #region User Management

    Task AddUserAsync(User user);
    void UpdateUser(User user);

    #endregion
}