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
//using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Application.Tools;

namespace Application.Services.Impelementation
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        public async Task<List<UserListViewModel>> GetUserListAsync()
        {
            var users = await userRepository.GetAllAsync();
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
            var Exist = await userRepository.GetUserByEmailAsync(model.Email);
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
                await userRepository.AddUserAsync(user);
                await userRepository.SaveChangesAsync();
                return CreateUserEnums.Success;

            }

            return CreateUserEnums.EmailExist;


        }

        public async Task<bool> IsEmailExistAsync(string email)
        {
            return await userRepository.IsEmailExistAsync(email);
        }

        #region Password
        
        public async Task<bool> IsPasswordCorrectAsync(string email,string password)
        {
            var x =await userRepository.GetUserByEmailAsync(email);
            return PasswordHasher.VerifyHashedPassword(x.Password, password);
        }

        public async Task ChangePasswordAsync(int userId, string newPassword)
        {
            var user = await userRepository.GetUserByIdAsync(userId);

            if (user == null)
                throw new Exception("User not found");

            user.Password = PasswordHasher.HashPassword(newPassword);

            userRepository.UpdateUser(user) ;
            await userRepository.SaveChangesAsync();
        }


        public async Task<ForgetPasswordEnum> ForgotPasswordEmailSenderAsync(string email)
        {
            var user = await userRepository.GetUserByEmailAsync(email);
            if (user == null)
                return ForgetPasswordEnum.UserNotFound;
            var domainLink = "https://localhost:7271";
            var mailbody = $"<a href=\"{domainLink}/ForgetPassword/{user.EmailActiveCode}\"> فراموشی رمز عبور</a>";
            await EmailSender.SendEmail(user.Email,"فراموشی رمز عبور",mailbody );
            return ForgetPasswordEnum.Success;
            
        }

        public async Task<ForgetPasswordTokenCheckEnum> ForgotPasswordTokenCheckerAsync(string token)
        {
            var exist = await userRepository.IsExistUserByGuidAsync(token);
            if (exist)
                return ForgetPasswordTokenCheckEnum.Success;
            return ForgetPasswordTokenCheckEnum.Failed;
        }

        public async Task ResetPasswordAsync(string token, string newPassword)
        {
            var user = await userRepository.GetUserByGUIDAsync(token);
            user.Password = PasswordHasher.HashPassword(newPassword);
            userRepository.UpdateUser(user);
            await userRepository.SaveChangesAsync();
        }

        public bool ComparePasswordAsync(string hashedPassword, string providedPassword)
        {
            return PasswordHasher.VerifyHashedPassword(hashedPassword, providedPassword);
        }
        
        #endregion

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await userRepository.GetUserByEmailAsync(email);
        }

        
        public async Task RegisterUserAsync(RegisterUserViewModel model)
        {
            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = PasswordHasher.HashPassword(model.Password),
                PhoneNumber = model.PhoneNumber,
                IsAdmin = false,
                IsEmailActive = false,
                IsDeleted = false,
                CreateDate = DateTime.UtcNow,
                EmailActiveCode = Guid.NewGuid().ToString("N")

            };
            var domainLink = "https://localhost:7271";
            string mailbody = $"<a href=\"{domainLink}/EmailActive/{user.EmailActiveCode}\"> فعالسازی حساب کاربری </a>";
            await EmailSender.SendEmail(user.Email,"فعال سازی حساب کاربری",mailbody );
            await userRepository.AddUserAsync(user);
            await userRepository.SaveChangesAsync();
        }
        
        
        
        public async Task EditUserAsync(EditUserViewModel model)
        {
            
            var user = await userRepository.GetUserByIdAsync(model.Id);
          

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.IsAdmin= model.IsAdmin;
            user.IsEmailActive= model.IsEmailActive;
            user.Address = model.Address;
            user.Address=model.Address;
            user.Password = !string.IsNullOrEmpty(model.Password?.Trim())
                ? PasswordHasher.HashPassword(model.Password) : user.Password;
            userRepository.UpdateUser(user);
            await userRepository.SaveChangesAsync();

        }

        public async Task<EditUserViewModel> GetUsersByIDAsync(int userid)
        {
            var user= await userRepository.GetUserByIdAsync(userid);
            var edit = new EditUserViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                IsAdmin = user.IsAdmin,
                IsEmailActive = user.IsEmailActive,
                Password = user.Password,
                Id = userid,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,

            };
            return edit;
        }

        public async Task<ActiveEmailEnum> EmailActivatorAsync(string emailActiveCode)
        {
            var user= await userRepository.GetUserByGUIDAsync(emailActiveCode);
            if (user == null)
                return ActiveEmailEnum.Failed;
            
            user.IsEmailActive = true;
            user.EmailActiveCode=Guid.NewGuid().ToString("N");
            await userRepository.SaveChangesAsync();
            return ActiveEmailEnum.Success;
        }
        
        public async Task<LoginUserEnum> LoginUserAsync(LoginUserViewModel model)
        {
            var user = await userRepository.GetUserByEmailAsync(model.Email);
            if (user != null)
            {
                //if (user.Password == PasswordHasher.HashPassword(model.Password))
                if (PasswordHasher.VerifyHashedPassword(user.Password,model.Password))
                {
                    if (user.IsEmailActive)
                    {
                        return LoginUserEnum.Success;
                    }
                    else
                    {
                        return LoginUserEnum.UserNotActive;
                    }
                }
                else
                {
                    return LoginUserEnum.PasswordInvalid;
                }
            }
            else
            {
                return LoginUserEnum.EmailInvalid;
            }
            
        }

        public async Task<RegisterUserEnum> RegisterUserValidationAsync(RegisterUserViewModel model)
        {
            
            if (await IsEmailExistAsync(model.Email))
            {
                return RegisterUserEnum.EmailUsed;
            }
            else
            {
                return RegisterUserEnum.Success;
            }
        }

        public async Task DeleteUserAsync(int userid)
        {
            var user = await userRepository.GetUserByIdAsync(userid);
            user.IsDeleted = true;
            userRepository.UpdateUser(user);
           await userRepository.SaveChangesAsync();

        }

        public async Task<UserDetailViewModel> GetUserDetailAsync(int userid)
        {
            var user = await userRepository.GetUserByIdAsync(userid);
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

        public async Task<User> GetUserById(int userid)
        {
            return await userRepository.GetUserByIdAsync(userid);
        }

    }
}