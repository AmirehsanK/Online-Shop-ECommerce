using Domain.Entities.Account;
using Domain.Entities.Permission;
using Domain.Interface;
using Domain.ViewModel.User;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

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
    public async Task RemoveAllRolePermissionsAsync(int roleId)
    {
        var rolePermissions = await context.RolePermissionMappings
            .Where(rp => rp.RoleId == roleId)
            .ToListAsync();

        context.RolePermissionMappings.RemoveRange(rolePermissions);
        await context.SaveChangesAsync();
    }
    public async Task AddRolePermissionAsync(RolePermissionMapping rolePermissionMapping)
    {
        context.RolePermissionMappings.Add(rolePermissionMapping);
        await context.SaveChangesAsync();
    }
    public async Task<Permission> GetPermissionByIdAsync(int permissionId)
    {
        return await context.Permissions
            .FirstOrDefaultAsync(p => p.Id == permissionId);
    }

    #endregion
    

    #region Role

    public async Task<FilterUserWithRolesViewModel> GetUsersWithRolesAsync(FilterUserWithRolesViewModel filter)
    {
        var query = context.Users
            .Include(u => u.UserRoleMappings)
            .ThenInclude(urm => urm.Role)
            .Where(u => !u.IsDeleted && u.UserRoleMappings.Any()) 
            .Select(u => new UserWithRolesViewModel
            {
                UserId = u.Id,
                UserName = u.FirstName + " " + u.LastName,
                Email = u.Email,
                Roles = u.UserRoleMappings.Select(urm => urm.Role.RoleName).ToList()
            });

        if (!string.IsNullOrEmpty(filter.UserName))
        {
            query = query.Where(u => u.UserName.Contains(filter.UserName));
        }

        if (!string.IsNullOrEmpty(filter.Email))
        {
            query = query.Where(u => u.Email.Contains(filter.Email));
        }

        var pagingResult = await filter.Paging(query);
        var result = new FilterUserWithRolesViewModel
        {
            Entities = pagingResult.Entities,
            Skip = pagingResult.Skip,
            Page = pagingResult.Page,
            PageCount = pagingResult.PageCount,
            StartPage = pagingResult.StartPage,
            TakeEntity = pagingResult.TakeEntity,
            EndPage = pagingResult.EndPage,
            UserName = filter.UserName,
            Email = filter.Email
        };

        return result;
    }
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
    public async Task SoftDeleteRoleAsync(int roleId)
    {
        var role = await context.Roles.FindAsync(roleId);
        if (role != null)
        {
            role.IsDeleted = true;
            context.Roles.Update(role);
            await context.SaveChangesAsync();
        }
    }
    public async Task<UserWithRolesViewModel> GetUserWithRolesAsync(int userId)
    {
        var user = await context.Users
            .Include(u => u.UserRoleMappings)
            .ThenInclude(urm => urm.Role)
            .FirstOrDefaultAsync(u => u.Id == userId);

        return new UserWithRolesViewModel
        {
            UserId = user.Id,
            UserName = user.FirstName + " " + user.LastName,
            Email = user.Email,
            Roles = user.UserRoleMappings.Select(urm => urm.Role.RoleName).ToList()
        };
    }

    public async Task UpdateUserRolesAsync(int userId, List<string> selectedRoles)
    {
        var user = await context.Users
            .Include(u => u.UserRoleMappings)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }
        user.UserRoleMappings.Clear();
        foreach (var roleName in selectedRoles)
        {
            var role = await context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
            if (role != null)
            {
                user.UserRoleMappings.Add(new UserRoleMapping
                {
                    RoleId = role.Id,
                    UserId = user.Id
                });
            }
        }

        await context.SaveChangesAsync();
    }
    public async Task<bool> IsRoleNameUniqueAsync(string roleName, int? roleId = null)
    {
        return !await context.Roles.AnyAsync(r => r.RoleName == roleName && (!roleId.HasValue || r.Id != roleId.Value));
    }
    public async Task<List<Role>> GetAllRolesAsync()
    {
        return await context.Roles.Where(u=>u.IsDeleted==false).ToListAsync();
    }
    public async Task<List<string>> GetUserRolesAsync(int userId)
    {
        return await context.UserRoleMappings
            .Where(urm => urm.UserId == userId)
            .Select(urm => urm.Role.RoleName)
            .ToListAsync();
    }
    public async Task<Role> GetRoleByNameAsync(string roleName)
    {
        return await context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
    }
    public async Task<User> GetUserByIdAsync(int userId)
    {
        var user = await context.Users
            .Include(u => u.UserRoleMappings)
            .ThenInclude(urm => urm.Role)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user != null && user.UserRoleMappings == null)
        {
            user.UserRoleMappings = new List<UserRoleMapping>();
        }

        return user;
    }
    public async Task UpdateUserAsync(User user)
    {
        context.Users.Update(user);
        await context.SaveChangesAsync();
    }
    
    #endregion
    
    
}