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
            return users.Where(u=> u.IsDeleted==false).Select(u => new UserListViewModel()
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                IsDeleted = u.IsDeleted,
                
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
