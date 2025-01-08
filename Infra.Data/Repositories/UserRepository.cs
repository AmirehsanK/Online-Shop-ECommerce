using Domain.Entities.Account;
using Domain.Interface;
using Domain.ViewModel.User;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repositories;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    public async Task<List<User>> GetAllAsync()
    {
        return await context.Users.ToListAsync();
    }

    public async Task<User> GetUserByIdAsync(int userid)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Id == userid);
    }
    public async Task<List<UserWithRolesViewModel>> GetAllUsersForRolesAsync()
    {
        return await context.Users
            .Where(u => !u.IsDeleted)
            .Select(u => new UserWithRolesViewModel
            {
                UserId = u.Id,
                UserName = u.FirstName + " " + u.LastName,
                Email = u.Email
            })
            .ToListAsync();
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User> GetUserByGUIDAsync(string guid)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.EmailActiveCode == guid);
    }

    public async Task<bool> IsExistUserByGuidAsync(string guid)
    {
        return await context.Users.AnyAsync(u => u.EmailActiveCode == guid);
    }


    public async Task<bool> IsEmailExistAsync(string email)
    {
        return await context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task AddUserAsync(User user)
    {
        await context.Users.AddAsync(user);
    }

    public void UpdateUser(User user)
    {
        context.Update(user);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

    public async Task<string> GetUserNameByIdAsync(int userid)
    {
        var user= await context.Users.FirstOrDefaultAsync(u=> u.Id==userid);
        return user.FirstName + " " + user.LastName;
    }
    
}