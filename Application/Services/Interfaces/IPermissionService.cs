using Domain.Entities.Permission;
using Domain.ViewModel.Permission;

namespace Application.Services.Interfaces;

public interface IPermissionService
{
   #region Permission

   Task<bool> CheckUserPermissionAsync(int userId,string permissionName);
   Task<List<PermissionSelectionViewModel>> GetPermissionsHierarchyAsync();

   #endregion

   #region Role

   Task<Role> GetRoleByIdAsync(int id);
   Task<List<int>> GetSelectedPermissionIdsAsync(int roleId);
   Task AddRoleAsync(RolePermissionsViewModel viewModel);
   Task UpdateRoleAsync(RolePermissionsViewModel viewModel);

   #endregion
   
}