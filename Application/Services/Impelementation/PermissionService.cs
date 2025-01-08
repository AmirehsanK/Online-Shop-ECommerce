using Application.Services.Interfaces;
using Domain.Entities.Account;
using Domain.Entities.Permission;
using Domain.Interface;
using Domain.ViewModel.Permission;
using Domain.ViewModel.User;

namespace Application.Services.Impelementation;

public class PermissionService(IUserRepository userRepository,IPermissionRepository permissionRepository) : IPermissionService
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
        if (permission is null)
        {
            return false;
        }

        return user.UserRoleMappings.Any(s => permission.RolePermissionMappings.Any(p => p.RoleId == s.RoleId));
    }


    #endregion

    #region Role
    
    public async Task<FilterUserWithRolesViewModel> GetUsersWithRolesAsync(FilterUserWithRolesViewModel filter)
    {
        return await permissionRepository.GetUsersWithRolesAsync(filter);
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
    public async Task<List<int>> GetSelectedPermissionIdsAsync(int roleId)
    {
        return await permissionRepository.GetSelectedPermissionIdsAsync(roleId);
    }
    public async Task<Role> GetRoleByIdAsync(int id)
    {
        return await permissionRepository.GetRoleByIdAsync(id);
    }

    public async Task<UserWithRolesViewModel> GetUserWithRolesAsync(int userId)
    {
        var user = await userRepository.GetUserByIdAsync(userId);
        if (user == null)
        {
            return null;
        }

        var roles = user.UserRoleMappings?
            .Select(urm => urm.Role?.RoleName)
            .Where(roleName => roleName != null)
            .ToList() ?? new List<string>();

        return new UserWithRolesViewModel
        {
            UserId = user.Id,
            UserName = user.FirstName + " " + user.LastName,
            Roles = roles
        };
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

        var selectedPermissions  = viewModel.Permissions
            .Where(p => p.IsSelected)
            .ToList();


        List<int> selectedPermissionIds = new();

        foreach (var parent in selectedPermissions)
        {
            selectedPermissionIds.Add(parent.PermissionId);
            parent.Children
                .Where(s => s.IsSelected)
                .ToList()
                .ForEach(item =>  selectedPermissionIds.Add(item.PermissionId));
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
        if (selectedRoles == null || !selectedRoles.Any())
        {
            throw new ArgumentException("Selected roles list is null or empty.");
        }

        var user = await permissionRepository.GetUserById(userId);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        if (user.UserRoleMappings == null)
        {
            user.UserRoleMappings = new List<UserRoleMapping>();
        }

        user.UserRoleMappings.Clear();

        foreach (var roleName in selectedRoles)
        {
            var role = await permissionRepository.GetRoleByNameAsync(roleName);
            if (role == null)
            {
                throw new KeyNotFoundException($"Role '{roleName}' not found.");
            }

            user.UserRoleMappings.Add(new UserRoleMapping
            {
                RoleId = role.Id,
                UserId = user.Id
            });
        }

        await permissionRepository.UpdateUserAsync(user);
    }
    #endregion
}