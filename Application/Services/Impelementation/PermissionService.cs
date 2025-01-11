using Application.Services.Interfaces;
using Domain.Entities.Permission;
using Domain.Interface;
using Domain.ViewModel.Permission;
using Domain.ViewModel.User;

namespace Application.Services.Impelementation;

public class PermissionService(
    IUserRepository userRepository,
    IPermissionRepository permissionRepository) : IPermissionService
{
    
    #region Permission
    public async Task<bool> CheckUserPermissionAsync(int userId, string permissionName)
    {
        var user = await permissionRepository.GetUserById(userId);
        if (user is null)
        {
            return false;
        }
        var permission = await permissionRepository.GetUniquePermission(permissionName);
        return permission is not null && user.UserRoleMappings.Any(s => permission.RolePermissionMappings.Any(p => p.RoleId == s.RoleId));
    }

    public async Task<List<PermissionSelectionViewModel>> GetPermissionsHierarchyAsync()
    {
        var permissions = await permissionRepository.GetAllPermissionsAsync();
        var permissionHierarchy = permissions
            .Select(p => new PermissionSelectionViewModel
            {
                PermissionId = p.Id,
                ParentId = p.ParentId ?? 0,
                DisplayName = p.DisplayName
            })
            .ToList();

        return BuildPermissionTree(permissionHierarchy);
    }

    private static List<PermissionSelectionViewModel> BuildPermissionTree(List<PermissionSelectionViewModel> permissions)
    {
        var lookup = permissions.ToLookup(p => p.ParentId);
        foreach (var permission in permissions)
        {
            permission.Children = lookup[permission.PermissionId].ToList();
        }
        return lookup[0].ToList();
    }

    public async Task<List<int>> GetSelectedPermissionIdsAsync(int roleId)
    {
        return await permissionRepository.GetSelectedPermissionIdsAsync(roleId);
    }
    #endregion

    #region Role
    public async Task<bool> CanEditOrDeleteRoleAsync(int roleId)
    {
        var role = await permissionRepository.GetRoleByIdAsync(roleId);
        if (role == null!)
        {
            return false;
        }
        return role.RoleName != "ادمین کل";
    }

    public async Task<FilterUserWithRolesViewModel> GetUsersWithRolesAsync(FilterUserWithRolesViewModel filter)
    {
        return await permissionRepository.GetUsersWithRolesAsync(filter);
    }

    public async Task<Role> GetRoleByIdAsync(int id)
    {
        return await permissionRepository.GetRoleByIdAsync(id);
    }

    public async Task<UserWithRolesViewModel> GetUserWithRolesAsync(int userId)
    {
        return await permissionRepository.GetUserWithRolesAsync(userId);
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

        await permissionRepository.RemoveAllRolePermissionsAsync(role.Id);

        var selectedPermissions = viewModel.Permissions
            .Where(p => p.IsSelected)
            .ToList();

        List<int> selectedPermissionIds = new();

        foreach (var parent in selectedPermissions)
        {
            selectedPermissionIds.Add(parent.PermissionId);
            parent.Children
                .Where(s => s.IsSelected)
                .ToList()
                .ForEach(item => selectedPermissionIds.Add(item.PermissionId));
        }

        foreach (var permissionId in selectedPermissionIds)
        {
            var permission = await permissionRepository.GetPermissionByIdAsync(permissionId);
            if (permission == null)
            {
                throw new KeyNotFoundException($"Permission with ID {permissionId} not found.");
            }
            var rolePermissionMapping = new RolePermissionMapping(role.Id, permissionId);
            await permissionRepository.AddRolePermissionAsync(rolePermissionMapping);
        }
    }

    public async Task SoftDeleteRoleAsync(int roleId)
    {
        await permissionRepository.SoftDeleteRoleAsync(roleId);
    }

    public async Task<List<string>> GetUserRolesAsync(int userId)
    {
        return await permissionRepository.GetUserRolesAsync(userId);
    }

    public async Task<bool> IsRoleNameUniqueAsync(string roleName, int? roleId = null)
    {
        return await permissionRepository.IsRoleNameUniqueAsync(roleName, roleId);
    }

    public async Task<List<Role>> GetAllRolesAsync()
    {
        return await permissionRepository.GetAllRolesAsync();
    }

    public async Task UpdateUserRolesAsync(int userId, List<string> selectedRoles)
    {
        var user = await permissionRepository.GetUserByIdAsync(userId);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        var existingMappings = user.UserRoleMappings
            .Where(urm => !urm.IsDeleted)
            .ToList();

        foreach (var mapping in existingMappings.Where(mapping => !selectedRoles.Contains(mapping.Role.RoleName)))
        {
            mapping.IsDeleted = true;
            mapping.CreateDate = DateTime.UtcNow;
        }

        foreach (var roleName in selectedRoles.Where(roleName => !existingMappings.Any(urm => urm.Role.RoleName == roleName)))
        {
            var role = await permissionRepository.GetRoleByNameAsync(roleName);
            if (role != null!)
            {
                user.UserRoleMappings.Add(new UserRoleMapping
                {
                    RoleId = role.Id,
                    UserId = user.Id,
                    IsDeleted = false,
                    CreateDate = DateTime.UtcNow
                });
            }
        }

        await permissionRepository.UpdateUserAsync(user);
    }
    #endregion
    
}