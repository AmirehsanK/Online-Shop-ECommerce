using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Security;
using Application.Services.Interfaces;
using Domain.Entities.Account;
using Domain.Enums;
using Domain.Interface;
using Domain.ViewModel;
using Domain.ViewModel.User;
using Domain.ViewModel.User.Admin;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Linq;

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
            return users.Where(u=> u.IsDeleted==false).Select(u => new UserListViewModel()
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                IsDeleted = u.IsDeleted,
                
            }).ToList();
        }

        public async Task<CreateUserEnums> CreateUserAsync(CreateUserViewModel model)
        {
            model.Email = model.Email.ToLower().Trim();
            var Exist = await _userRepository.GetUserByEmailAsync(model.Email);
            if (Exist == null)
            {
                var user = new User()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    IsAdmin = model.IsAdmin,
                    EmailActiveCode = Guid.NewGuid().ToString("N"),
                    CreateDate = DateTime.Now,
                    IsDeleted = false,
                    IsEmailActive = model.IsEmailActive,
                    Password = model.Password
                };
                await _userRepository.AddUserAsync(user);
                await _userRepository.SaveChangesAsync();
                return CreateUserEnums.Success;

            }

            return CreateUserEnums.EmailExist;


        }

        public async Task<bool> IsEmailExistAsync(string email)
        {
            return await _userRepository.IsEmailExistAsync(email);
        }

        public async Task<bool> IsPasswordCorrectAsync(string email,string password)
        {
            var x =await _userRepository.GetUserByEmailAsync(email);
            return PasswordHasher.VerifyHashedPassword(x.Password, password);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }


        public async Task<LoginUserViewModel> LoginAsync(LoginUserViewModel loginUser)
        {
            return loginUser;
        }

        public async Task RegisterUserAsync(RegisterUserViewModel model)
        {
            // Validate input
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (await _userRepository.GetUserByEmailAsync(model.Email) != null)
                throw new InvalidOperationException("Email is already registered.");

            var user = new User
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

        public async Task EditUserAsync(EditUserViewModel model)
        {
            
            var user = await _userRepository.GetUserByIdAsync(model.Id);
          

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.IsAdmin= model.IsAdmin;
            user.IsEmailActive= model.IsEmailActive;
            user.Password = !string.IsNullOrEmpty(model.Password?.Trim())
                ? PasswordHasher.HashPassword(model.Password) : user.Password;
            _userRepository.UpdateUser(user);
            await _userRepository.SaveChangesAsync();

        }

        public async Task<EditUserViewModel> GetUsersByIDAsync(int userid)
        {
            var user= await _userRepository.GetUserByIdAsync(userid);
            var edit = new EditUserViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                IsAdmin = user.IsAdmin,
                IsEmailActive = user.IsEmailActive,
                Password = user.Password,
                Id = userid,

            };
            return edit;
        }

        public async Task DeleteUserAsync(int userid)
        {
            var user = await _userRepository.GetUserByIdAsync(userid);
            user.IsDeleted = true;
            _userRepository.UpdateUser(user);
           await _userRepository.SaveChangesAsync();

        }

        public async Task<UserDetailViewModel> GetUserDetailAsync(int userid)
        {
            var user = await _userRepository.GetUserByIdAsync(userid);
            var Detail = new UserDetailViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                IsAdmin = user.IsAdmin,
                CreatedDate = user.CreateDate,
                Id = userid,
                IsActive = user.IsEmailActive
            };
            return Detail;
        }
    }
}