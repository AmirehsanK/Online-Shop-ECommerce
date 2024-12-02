using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Security;
using Application.Services.Interfaces;
using Domain.Entities.Account;
using Domain.Interface;
using Domain.ViewModel;
using Domain.ViewModel.User;
using Domain.ViewModel.User.Admin;

namespace Application.Services.Impelementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserListViewModel>> GetUserListAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(u => new UserListViewModel
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                IsAdmin = u.IsAdmin,
                IsEmailActive = u.IsEmailActive
            }).ToList();
        }

        public async Task CreateUserAsync(CreateUserViewModel model)
        {
            // Validate input
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (await _userRepository.GetUserByEmailAsync(model.Email) != null)
                throw new InvalidOperationException("Email is already registered.");

            var user = new Users
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = PasswordHasher.HashPassword(model.Password),
                IsAdmin = model.IsAdmin,
                IsEmailActive = model.IsEmailActive,
                CreateDate = DateTime.UtcNow,
                EmailActiveCode = Guid.NewGuid().ToString("N"),
                IsDeleted = false
            };

            await _userRepository.AddUserAsync(user);
            await _userRepository.SaveChangesAsync();
        }

        public async Task<LoginUserViewModel> LoginAsync(LoginUserViewModel loginUser)
        {
            if (loginUser == null)
                throw new ArgumentNullException(nameof(loginUser));

            var user = await _userRepository.GetUserByEmailAsync(loginUser.Email);

            if (user == null || !PasswordHasher.VerifyHashedPassword(user.Password, loginUser.Password))
                return null; // Invalid login

            return new LoginUserViewModel
            {
                Email = user.Email
            };
        }

        public async Task RegisterUserAsync(RegisterUserViewModel model)
        {
            // Validate input
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (await _userRepository.GetUserByEmailAsync(model.Email) != null)
                throw new InvalidOperationException("Email is already registered.");

            var user = new Users
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = PasswordHasher.HashPassword(model.Password),
                IsAdmin = false,
                IsEmailActive = true,
                IsDeleted = false,
                CreateDate = DateTime.UtcNow,
                EmailActiveCode = Guid.NewGuid().ToString("N")
            };

            await _userRepository.AddUserAsync(user);
            await _userRepository.SaveChangesAsync();
        }
    }
}