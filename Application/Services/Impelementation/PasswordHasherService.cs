using System.Security.Cryptography;
using Application.Services.Interfaces;

namespace Application.Services.Impelementation;

public class PasswordHasherService : IPasswordHasher
{
    private const int SaltSize = 16; 
    private const int KeySize = 32; 
    // as it's a key security parameter. Higher is more secure but slower.
    private const int Iterations = 10000;
    private static readonly HashAlgorithmName _hashAlgorithmName = HashAlgorithmName.SHA256;
    private const char Delimiter = ';';

    public async Task<string> EncodePasswordAsync(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentNullException(nameof(password));
        }

        return await Task.Run(() =>
        {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, _hashAlgorithmName, KeySize);
            return string.Join(Delimiter, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
        });
    }

    public async Task<bool> VerifyPasswordAsync(string passwordHash, string inputPassword)
    {
        if (string.IsNullOrEmpty(passwordHash) || string.IsNullOrEmpty(inputPassword))
        {
            return false; 
        }

        return await Task.Run(() =>
        {
            var elements = passwordHash.Split(Delimiter);
            if (elements.Length != 2)
            {
                return false; 
            }

            try
            {
                var salt = Convert.FromBase64String(elements[0]);
                var hash = Convert.FromBase64String(elements[1]);

                var hashInput = Rfc2898DeriveBytes.Pbkdf2(inputPassword, salt, Iterations, _hashAlgorithmName, KeySize);

                return CryptographicOperations.FixedTimeEquals(hash, hashInput);
            }
            catch (FormatException)
            {
                return false;
            }
        });
    }
}