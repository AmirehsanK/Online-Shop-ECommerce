using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Account;
using Domain.Interface;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repositories
{
    public class UserRepository:IUserRepository
    {
        #region Ctor

        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion
    
        public async Task<IEnumerable<Users>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<Users> GetUserById(int userid)
        {
            return await _context.Users.FirstOrDefaultAsync(u=> u.Id==userid);
        }

        public async Task<Users?> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> IsEmailActive(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user.IsEmailActive == true)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> IsEmailExist(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user!=null)
            {
                return true;
            }
            return false;
        }

        public async Task AddUser(Users user)
        {
            await _context.Users.AddAsync(user);
        }

        public void UpdateUser(Users user)
        {
             _context.Update(user);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
