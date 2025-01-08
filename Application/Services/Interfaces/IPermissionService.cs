using Domain.Entities.Permission;
using Domain.ViewModel.Permission;
using Domain.ViewModel.User;

namespace Application.Services.Interfaces;

public interface IPermissionService
{
   #region Permission

   Task<bool> CheckUserPermissionAsync(int userId,string permissionName);
   Task<List<PermissionSelectionViewModel>> GetPermissionsHierarchyAsync();

   #endregion

   #region Role

   Task<FilterUserWithRolesViewModel> GetUsersWithRolesAsync(FilterUserWithRolesViewModel filter);
   Task<UserWithRolesViewModel> GetUserWithRolesAsync(int userId);
   Task<Role> GetRoleByIdAsync(int id);
   Task<List<int>> GetSelectedPermissionIdsAsync(int roleId);
   Task AddRoleAsync(RolePermissionsViewModel viewModel);
   Task UpdateRoleAsync(RolePermissionsViewModel viewModel);
   Task SoftDeleteRoleAsync(int roleId);
   Task<List<string>> GetUserRolesAsync(int userId);
   Task<bool> IsRoleNameUniqueAsync(string roleName, int? roleId = null);
   Task<List<Role>> GetAllRolesAsync();
   Task UpdateUserRolesAsync(int userId, List<string> selectedRoles);

   #endregion

}