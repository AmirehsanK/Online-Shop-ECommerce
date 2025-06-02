using Domain.Enums;

namespace Application.Services.Interfaces;

public interface IPasswordService
{
    Task<bool> IsPasswordCorrectAsync(string email, string password);
    Task<bool> ComparePasswordAsync(string hashedPassword, string providedPassword);
    Task ChangePasswordAsync(int userId, string newPassword);
    Task<ForgetPasswordEnum> ForgotPasswordEmailSenderAsync(string email);
    Task<ForgetPasswordTokenCheckEnum> ForgotPasswordTokenCheckerAsync(string token);
    Task<bool> ResetPasswordAsync(string token, string newPassword);
}