using Domain.Entities.Permission;
using Domain.ViewModel.Permission;
using Domain.ViewModel.User;

namespace Application.Services.Interfaces;

public interface IPermissionService
{

    #region Permission

    Task<List<PermissionSelectionViewModel>> GetPermissionsHierarchyAsync();
    Task<bool> CheckUserPermissionAsync(int userId, string permissionName);

    #endregion

    #region Role

    Task<FilterUserWithRolesViewModel> GetUsersWithRolesAsync(FilterUserWithRolesViewModel filter);
    Task<UserWithRolesViewModel> GetUserWithRolesAsync(int userId);
    Task<Role> GetRoleByIdAsync(int id);
    Task<List<Role>> GetAllRolesAsync();
    Task<List<string>> GetUserRolesAsync(int userId);
    Task<List<int>> GetSelectedPermissionIdsAsync(int roleId);
    Task<bool> CanEditOrDeleteRoleAsync(int roleId);
    Task<bool> IsRoleNameUniqueAsync(string roleName, int? roleId = null);
    Task AddRoleAsync(RolePermissionsViewModel viewModel);
    Task UpdateRoleAsync(RolePermissionsViewModel viewModel);
    Task UpdateUserRolesAsync(int userId, List<string> selectedRoles);
    Task SoftDeleteRoleAsync(int roleId);

    #endregion

}