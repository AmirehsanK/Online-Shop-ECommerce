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
        Task<IEnumerable<Users>> GetAll();

        Task<Users> GetUserById(int userid);

        Task<Users> GetUserByEmail(string email);
        Task<bool> IsEmailActive(int id);

        Task<bool> IsEmailExist(string email);

        Task AddUser(Users user);
        void UpdateUser(Users user);

        Task Save();

    }
}
