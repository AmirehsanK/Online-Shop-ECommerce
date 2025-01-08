using Domain.Entities.Account;
using Domain.Entities.Permission;
using Domain.ViewModel.Permission;
using Domain.ViewModel.User;

namespace Domain.Interface;

public interface IPermissionRepository
{
    #region Permission

    Task<User> GetUserById(int userId);
    Task<Permission> GetUniquePermission(string permissionName);
    
    Task<List<Permission>> GetAllPermissionsAsync();

    #endregion

    #region Role

    Task<FilterUserWithRolesViewModel> GetUsersWithRolesAsync(FilterUserWithRolesViewModel filter);
    Task<Role> GetRoleByIdAsync(int id);
    Task<List<int>> GetSelectedPermissionIdsAsync(int roleId);
    Task AddRoleAsync(Role role);
    Task UpdateRoleAsync(Role role);
    Task AddRolePermissionsAsync(int roleId, IEnumerable<RolePermissionMapping> permissions);
    Task UpdateRolePermissionsAsync(int roleId, IEnumerable<RolePermissionMapping> permissions);
    Task SoftDeleteRoleAsync(int roleId);
    Task<UserWithRolesViewModel> GetUserWithRolesAsync(int userId);
    Task UpdateUserRolesAsync(int userId, List<string> selectedRoles);
    Task<bool> IsRoleNameUniqueAsync(string roleName, int? roleId = null);
    Task<List<Role>> GetAllRolesAsync();
    Task<List<string>> GetUserRolesAsync(int userId);
    Task<Role> GetRoleByNameAsync(string roleName);
    Task<User> GetUserByIdAsync(int userId);
    Task UpdateUserAsync(User user);

    #endregion


}