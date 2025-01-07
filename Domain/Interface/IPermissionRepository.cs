using Domain.Entities.Account;
using Domain.Entities.Permission;

namespace Domain.Interface;

public interface IPermissionRepository
{
    #region Permission

    Task<User> GetUserById(int userId);
    Task<Permission> GetUniquePermission(string permissionName);
    
    Task<List<Permission>> GetAllPermissionsAsync();

    #endregion

    #region Role

    Task<Role> GetRoleByIdAsync(int id);
    Task<List<int>> GetSelectedPermissionIdsAsync(int roleId);
    Task AddRoleAsync(Role role);
    Task UpdateRoleAsync(Role role);
    Task AddRolePermissionsAsync(int roleId, IEnumerable<RolePermissionMapping> permissions);
    Task UpdateRolePermissionsAsync(int roleId, IEnumerable<RolePermissionMapping> permissions);
    
    #endregion
    

}