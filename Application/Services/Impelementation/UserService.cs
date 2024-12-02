using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Security;
using Application.Services.Interfaces;
using Domain.Entities.Account;
using Domain.Enums;
using Domain.Interface;
using Domain.ViewModel.User.Admin;

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

        public async Task<List<UserListViewModel>> GetUserListAsync()
        {
            var users = await _userRepository.GetAllAsync();
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

        public async Task CreateUserAsync(CreateUserViewModel model)
        {
            var user = new Users()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = PasswordHasher.HashPassword(model.Password),
                IsAdmin = model.IsAdmin,
                IsEmailActive = model.IsEmailActive,
                CreateDate = DateTime.Now,
                EmailActiveCode = Guid.NewGuid().ToString("N"),
                IsDeleted = false

            };
            await _userRepository.AddUserAsync(user);
            await _userRepository.SaveChangesAsync();

        }
    }
}
