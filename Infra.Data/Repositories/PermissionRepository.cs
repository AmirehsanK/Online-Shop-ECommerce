using Domain.Entities.Account;
using Domain.Entities.Permission;
using Domain.Interface;
using Domain.ViewModel.User;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repositories;

public class PermissionRepository(ApplicationDbContext context) : IPermissionRepository
{

    #region Admin

    public async Task SeedSuperAdminAsync(User user)
    {
        if (await context.Users.AnyAsync(u => u.FirstName == "superadmin"))
        {
            return;
        }

        var superAdminUser = new User
        {
            FirstName = "superadmin",
            Email = "superadmin@example.com",
            Password = ("SuperAdminPassword123!"),
            UserRoleMappings = new List<UserRoleMapping>
            {
                new UserRoleMapping
                {
                    RoleId = (await context.Roles.FirstOrDefaultAsync(r => r.RoleName == "ادمین کل")).Id
                }
            }
        };
        await context.Users.AddAsync(superAdminUser);
        await context.SaveChangesAsync();
    }

    #endregion
    
    #region Permission

    public async Task<User> GetUserById(int userId)
    {
        return await context.Users
            .Include(u => u.UserRoleMappings)
            .ThenInclude(urm => urm.Role)
            .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted); 
    }

    public async Task<Permission> GetUniquePermission(string permissionName)
    {
        return await context.Permissions
            .Include(p => p.RolePermissionMappings)
            .FirstOrDefaultAsync(p => p.UniqueName == permissionName && !p.IsDeleted);
    }

    public async Task<List<Permission>> GetAllPermissionsAsync()
    {
        return await context.Permissions
            .Where(p => !p.IsDeleted)
            .ToListAsync();
    }

    public async Task RemoveAllRolePermissionsAsync(int roleId)
    {
        var rolePermissions = await context.RolePermissionMappings
            .Where(rp => rp.RoleId == roleId && !rp.IsDeleted)
            .ToListAsync();

        foreach (var rolePermission in rolePermissions)
        {
            rolePermission.IsDeleted = true;
            rolePermission.CreateDate = DateTime.UtcNow; 
        }

        await context.SaveChangesAsync();
    }

    public async Task AddRolePermissionAsync(RolePermissionMapping rolePermissionMapping)
    {
        rolePermissionMapping.IsDeleted = false;
        rolePermissionMapping.CreateDate = DateTime.UtcNow;
        context.RolePermissionMappings.Add(rolePermissionMapping);
        await context.SaveChangesAsync();
    }

    public async Task<Permission> GetPermissionByIdAsync(int permissionId)
    {
        return await context.Permissions
            .FirstOrDefaultAsync(p => p.Id == permissionId && !p.IsDeleted);
    }

    #endregion

    #region Role

    public async Task<FilterUserWithRolesViewModel> GetUsersWithRolesAsync(FilterUserWithRolesViewModel filter)
    {
        var query = context.Users
            .Include(u => u.UserRoleMappings)
            .ThenInclude(urm => urm.Role)
            .Where(u => !u.IsDeleted && u.UserRoleMappings.Any(urm => !urm.IsDeleted))
            .Select(u => new UserWithRolesViewModel
            {
                UserId = u.Id,
                UserName = u.FirstName + " " + u.LastName,
                Email = u.Email,
                Roles = u.UserRoleMappings
                    .Where(urm => !urm.IsDeleted)
                    .Select(urm => urm.Role.RoleName)
                    .ToList()
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
        return await context.Roles
            .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
    }

    public async Task<List<int>> GetSelectedPermissionIdsAsync(int roleId)
    {
        return await context.RolePermissionMappings
            .Where(rpm => rpm.RoleId == roleId && !rpm.IsDeleted)
            .Select(rpm => rpm.PermissionId)
            .ToListAsync();
    }

    public async Task AddRoleAsync(Role role)
    {
        role.IsDeleted = false;
        role.CreateDate = DateTime.UtcNow; 
        context.Roles.Add(role);
        await context.SaveChangesAsync();
    }

    public async Task UpdateRoleAsync(Role role)
    {
        role.IsDeleted = false;
        context.Roles.Update(role);
        await context.SaveChangesAsync();
    }

    public async Task AddRolePermissionsAsync(int roleId, IEnumerable<RolePermissionMapping> permissions)
    {
        foreach (var permission in permissions)
        {
            permission.IsDeleted = false; 
            permission.CreateDate = DateTime.UtcNow; 
        }

        context.RolePermissionMappings.AddRange(permissions);
        await context.SaveChangesAsync();
    }

    public async Task UpdateRolePermissionsAsync(int roleId, IEnumerable<RolePermissionMapping> permissions)
    {
        var existingMappings = await context.RolePermissionMappings
            .Where(rpm => rpm.RoleId == roleId && !rpm.IsDeleted) 
            .ToListAsync();

        foreach (var mapping in existingMappings)
        {
            mapping.IsDeleted = true;
            mapping.CreateDate = DateTime.UtcNow; 
        }

        foreach (var permission in permissions)
        {
            permission.IsDeleted = false; 
            permission.CreateDate = DateTime.UtcNow; 
        }

        context.RolePermissionMappings.AddRange(permissions);
        await context.SaveChangesAsync();
    }

    public async Task SoftDeleteRoleAsync(int roleId)
    {
        var role = await context.Roles
            .FirstOrDefaultAsync(r => r.Id == roleId && !r.IsDeleted);

        if (role != null)
        {
            role.IsDeleted = true;
            role.CreateDate = DateTime.UtcNow; 
            context.Roles.Update(role);
            await context.SaveChangesAsync();
        }
    }

    public async Task<UserWithRolesViewModel> GetUserWithRolesAsync(int userId)
    {
        var user = await context.Users
            .Include(u => u.UserRoleMappings)
            .ThenInclude(urm => urm.Role)
            .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);

        if (user == null)
        {
            Console.WriteLine("User not found.");
            return null;
        }

        var roles = user.UserRoleMappings
            .Where(urm => !urm.IsDeleted) 
            .Select(urm => urm.Role?.RoleName) 
            .Where(roleName => roleName != null)
            .ToList();

        return new UserWithRolesViewModel
        {
            UserId = user.Id,
            UserName = user.FirstName + " " + user.LastName,
            Email = user.Email,
            Roles = roles
        };
    }

    public async Task UpdateUserRolesAsync(int userId, List<string> selectedRoles)
    {
        var user = await context.Users
            .Include(u => u.UserRoleMappings)
            .ThenInclude(urm => urm.Role)
            .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);

        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        var existingMappings = user.UserRoleMappings
            .Where(urm => !urm.IsDeleted) 
            .ToList();

        foreach (var roleName in selectedRoles)
        {
            if (!existingMappings.Any(urm => urm.Role.RoleName == roleName))
            {
                var role = await context.Roles
                    .FirstOrDefaultAsync(r => r.RoleName == roleName && !r.IsDeleted); 

                if (role != null)
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
        }

        await context.SaveChangesAsync();
    }

    public async Task<bool> IsRoleNameUniqueAsync(string roleName, int? roleId = null)
    {
        return !await context.Roles
            .AnyAsync(r => r.RoleName == roleName && (!roleId.HasValue || r.Id != roleId.Value) && !r.IsDeleted);
    }

    public async Task<List<Role>> GetAllRolesAsync()
    {
        return await context.Roles
            .Where(r => !r.IsDeleted) 
            .ToListAsync();
    }

    public async Task<List<string>> GetUserRolesAsync(int userId)
    {
        return await context.UserRoleMappings
            .Where(urm => urm.UserId == userId && !urm.IsDeleted)
            .Select(urm => urm.Role.RoleName)
            .ToListAsync();
    }

    public async Task<Role> GetRoleByNameAsync(string roleName)
    {
        return await context.Roles
            .FirstOrDefaultAsync(r => r.RoleName == roleName && !r.IsDeleted); 
    }

    public async Task<User> GetUserByIdAsync(int userId)
    {
        var user = await context.Users
            .Include(u => u.UserRoleMappings)
            .ThenInclude(urm => urm.Role)
            .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted); 

        if (user != null && user.UserRoleMappings == null)
        {
            user.UserRoleMappings = new List<UserRoleMapping>();
        }

        return user;
    }

    public async Task UpdateUserAsync(User user)
    {
        user.IsDeleted = false; 
        context.Users.Update(user);
        await context.SaveChangesAsync();
    }

    #endregion
}