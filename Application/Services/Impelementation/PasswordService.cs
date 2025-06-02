using Application.Services.Interfaces;
using Application.Tools;
using Domain.Enums;
using Domain.Interface;
using Microsoft.Extensions.Configuration;

namespace Application.Services.Impelementation;

public class PasswordService(
    IConfiguration configuration,
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IEmailSender emailSender)
    : IPasswordService
{
    private readonly string _domainLink = configuration["ApplicationSettings:DomainLink"]!;

    public async Task<bool> IsPasswordCorrectAsync(string email, string password)
    {
        var x = await userRepository.GetUserByEmailAsync(email);
        return await passwordHasher.VerifyPasswordAsync(x.Password, password);
    }

    public async Task ChangePasswordAsync(int userId, string newPassword)
    {
        var user = await userRepository.GetUserByIdAsync(userId);

        if (user == null)
            throw new Exception("User not found");

        user.Password = await passwordHasher.EncodePasswordAsync(newPassword);

        userRepository.UpdateUser(user);
        await userRepository.SaveChangesAsync();
    }

    public async Task<ForgetPasswordEnum> ForgotPasswordEmailSenderAsync(string email)
    {
        var user = await userRepository.GetUserByEmailAsync(email);
        if (user == null!)
            return ForgetPasswordEnum.UserNotFound;
        var mailBody = $"<a href=\"{_domainLink}/ForgetPassword/{user.EmailActiveCode}\"> فراموشی رمز عبور</a>";
        try
        {
            await emailSender.SendEmailAsync(user.Email, " فراموشی رمز عبور", mailBody);
            return ForgetPasswordEnum.Success;
        }
        catch (Exception ex)
        {
            return ForgetPasswordEnum.EmailSendFailed;
        }
    }

    public async Task<ForgetPasswordTokenCheckEnum> ForgotPasswordTokenCheckerAsync(string token)
    {
        var exist = await userRepository.IsExistUserByGuidAsync(token);
        return exist ? ForgetPasswordTokenCheckEnum.Success : ForgetPasswordTokenCheckEnum.Failed;
    }

    public async Task<bool> ResetPasswordAsync(string token, string newPassword)
    {
        var user = await userRepository.GetUserByGUIDAsync(token);
        if (user == null)
            return false;
        user.Password = await passwordHasher.EncodePasswordAsync(newPassword);
        userRepository.UpdateUser(user);
        await userRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ComparePasswordAsync(string hashedPassword, string providedPassword)
    {
        return await passwordHasher.VerifyPasswordAsync(hashedPassword, providedPassword);
    }
}