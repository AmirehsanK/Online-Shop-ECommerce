using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Interfaces;
using Domain.Entities.Account;
using Domain.Interface;
using Domain.ViewModel.User;

namespace Application.Services.Impelementation
{
    public class UserService : IUserService
    {
        #region Ctor

        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        #endregion

        public async Task<IEnumerable<UserListViewModel>> GetUserListAsync()
        {
            var users = await _userRepository.GetAll();
            return users.Select(u => new UserListViewModel()
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Password = u.Password,
                IsAdmin = u.IsAdmin,
                IsEmailActive = u.IsEmailActive
            }).ToList();

        }
    }
}
