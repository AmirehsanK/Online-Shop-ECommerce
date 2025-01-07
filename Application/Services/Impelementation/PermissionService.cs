using Application.Services.Interfaces;
using Domain.Entities.Account;
using Domain.Entities.Permission;
using Domain.Interface;
using Domain.ViewModel.Permission;

namespace Application.Services.Impelementation;

public class PermissionService(IUserRepository userRepository,IPermissionRepository permissionRepository) : IPermissionService
{
    public async Task<bool> CheckUserPermissionAsync(int userId, string permissionName)
    {
        var user = await permissionRepository.GetUserById(userId);
        if (user is null)
        {
            return false;
        }

        var permission = await permissionRepository.GetUniquePermission(permissionName);
        if (permission is null)
        {
            return false;
        }

        return user.UserRoleMappings.Any(s => permission.RolePermissionMappings.Any(p => p.RoleId == s.RoleId));
    }
    public async Task<List<PermissionSelectionViewModel>> GetPermissionsHierarchyAsync()
    {
        var allPermissions = await permissionRepository.GetAllPermissionsAsync();
        
        var permissionsHierarchy = allPermissions
            .Where(p => p.ParentId == null)
            .Select(p => new PermissionSelectionViewModel
            {
                PermissionId = p.Id,
                DisplayName = p.DisplayName,
                ParentId = p.ParentId,
                Children = GetChildPermissions(p.Id, allPermissions)
            })
            .ToList();

        return permissionsHierarchy;
    }
    private List<PermissionSelectionViewModel> GetChildPermissions(int parentId, List<Permission> allPermissions)
    {
        return allPermissions
            .Where(p => p.ParentId == parentId)
            .Select(p => new PermissionSelectionViewModel
            {
                PermissionId = p.Id,
                DisplayName = p.DisplayName,
                ParentId = p.ParentId,
                Children = GetChildPermissions(p.Id, allPermissions)
            })
            .ToList();
    }

    // Role-related methods
    public async Task<Role> GetRoleByIdAsync(int id)
    {
        return await permissionRepository.GetRoleByIdAsync(id);
    }

    public async Task<List<int>> GetSelectedPermissionIdsAsync(int roleId)
    {
        return await permissionRepository.GetSelectedPermissionIdsAsync(roleId);
    }

    public async Task AddRoleAsync(RolePermissionsViewModel viewModel)
    {
        var role = new Role { RoleName = viewModel.RoleName };
        await permissionRepository.AddRoleAsync(role);

        var selectedPermissions = viewModel.Permissions
            .Where(p => p.IsSelected)
            .Select(p => new RolePermissionMapping(role.Id, p.PermissionId));
        await permissionRepository.AddRolePermissionsAsync(role.Id, selectedPermissions);
    }

    public async Task UpdateRoleAsync(RolePermissionsViewModel viewModel)
    {
        var role = await permissionRepository.GetRoleByIdAsync(viewModel.RoleId);
        if (role == null)
        {
            throw new KeyNotFoundException("Role not found.");
        }

        role.RoleName = viewModel.RoleName;
        await permissionRepository.UpdateRoleAsync(role);

        var selectedPermissions = viewModel.Permissions
            .Where(p => p.IsSelected)
            .Select(p => new RolePermissionMapping(role.Id, p.PermissionId));
        await permissionRepository.UpdateRolePermissionsAsync(role.Id, selectedPermissions);
    }
}