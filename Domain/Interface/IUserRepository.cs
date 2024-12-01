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
        Task<List<Users>> GetAllAsync();

        Task<Users> GetUserByIdAsync(int userid);

        Task<Users> GetUserByEmailAsync(string email);
     

        Task<bool> IsEmailExistAsync(string email);

        Task AddUserAsync(Users user);
        void UpdateUser(Users user);

        Task SaveChangesAsync();

    }
}
