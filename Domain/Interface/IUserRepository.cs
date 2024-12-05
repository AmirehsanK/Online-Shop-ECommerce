using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Account;
using Domain.ViewModel;

namespace Domain.Interface
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();

        Task<User> GetUserByIdAsync(int userid);

        Task<User> GetUserByEmailAsync(string email);
        
        Task<User> GetUserByGUIDAsync(string guid);

        Task<bool> IsEmailExistAsync(string email);

        Task AddUserAsync(User user);
        void UpdateUser(User user);

        Task SaveChangesAsync();

    }
}
