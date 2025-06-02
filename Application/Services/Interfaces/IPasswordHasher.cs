namespace Application.Services.Interfaces;

public interface IPasswordHasher
{
    Task<string> EncodePasswordAsync(string password);
    Task<bool> VerifyPasswordAsync(string passwordHash, string inputPassword);
}