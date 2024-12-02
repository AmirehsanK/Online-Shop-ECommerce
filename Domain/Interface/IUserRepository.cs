using Domain.Entities.Account;

namespace Domain.Interface;

public interface IUserRepository
{
    Task<List<Users>> GetAllAsync();

    Task<Users> GetUserByIdAsync(int userid);

    Task<Users> GetUserByEmailAsync(string email);


    Task<bool> IsEmailExistAsync(string email);

    Task AddUserAsync(Users user);
    void UpdateUser(Users user);

    Task SaveChangesAsync();
}