using Domain.Entities.Account;
using Domain.Entities.Permission;
using Domain.Interface;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Infra.Data.Repositories;

public class PermissionRepository(ApplicationDbContext context):IPermissionRepository
{
    #region Permission

    public async Task<User> GetUserById(int userId)
    {
        return await context.Users.Include(nameof(User.UserRoleMappings))
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<Permission> GetUniquePermission(string permissionName)
    {
        return await context.Permissions.Include(nameof(Permission.RolePermissionMappings)).FirstOrDefaultAsync(s=> s.UniqueName==permissionName);
    }
    
    public async Task<List<Permission>> GetAllPermissionsAsync()
    {
        return await context.Permissions.ToListAsync();
    }

    #endregion
    

    #region Role

    public async Task<Role> GetRoleByIdAsync(int id)
    {
        return await context.Roles.FindAsync(id);
    }

    public async Task<List<int>> GetSelectedPermissionIdsAsync(int roleId)
    {
        return await context.RolePermissionMappings
            .Where(rpm => rpm.RoleId == roleId)
            .Select(rpm => rpm.PermissionId)
            .ToListAsync();
    }

    public async Task AddRoleAsync(Role role)
    {
        context.Roles.Add(role);
        await context.SaveChangesAsync();
    }

    public async Task UpdateRoleAsync(Role role)
    {
        context.Roles.Update(role);
        await context.SaveChangesAsync();
    }

    public async Task AddRolePermissionsAsync(int roleId, IEnumerable<RolePermissionMapping> permissions)
    {
        context.RolePermissionMappings.AddRange(permissions);
        await context.SaveChangesAsync();
    }

    public async Task UpdateRolePermissionsAsync(int roleId, IEnumerable<RolePermissionMapping> permissions)
    {
        var existingMappings = await context.RolePermissionMappings
            .Where(rpm => rpm.RoleId == roleId)
            .ToListAsync();

        context.RolePermissionMappings.RemoveRange(existingMappings);
        context.RolePermissionMappings.AddRange(permissions);
        await context.SaveChangesAsync();
    }

    #endregion
    
    
}