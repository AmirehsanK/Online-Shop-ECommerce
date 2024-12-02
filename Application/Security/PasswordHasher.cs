using System.Security.Cryptography;

namespace Application.Security;

public class PasswordHasher
{
    private const int SaltByteSize = 24;
    private const int HashByteSize = 24;
    private const int HashingIterationsCount = 10101;


    public static string HashPassword(string password)
    {
        // http://stackoverflow.com/questions/19957176/asp-net-identity-password-hashing

        byte[] salt;
        byte[] buffer2;
        if (password == null) throw new ArgumentNullException("password");
        using (var bytes = new Rfc2898DeriveBytes(password, SaltByteSize, HashingIterationsCount))
        {
            salt = bytes.Salt;
            buffer2 = bytes.GetBytes(HashByteSize);
        }

        var dst = new byte[SaltByteSize + HashByteSize + 1];
        Buffer.BlockCopy(salt, 0, dst, 1, SaltByteSize);
        Buffer.BlockCopy(buffer2, 0, dst, SaltByteSize + 1, HashByteSize);
        return Convert.ToBase64String(dst);
    }

    public static bool VerifyHashedPassword(string hashedPassword, string password)
    {
        byte[] _passwordHashBytes;

        var _arrayLen = SaltByteSize + HashByteSize + 1;

        if (hashedPassword == null) return false;

        if (password == null) throw new ArgumentNullException("password");

        var src = Convert.FromBase64String(hashedPassword);

        if (src.Length != _arrayLen || src[0] != 0) return false;

        var _currentSaltBytes = new byte[SaltByteSize];
        Buffer.BlockCopy(src, 1, _currentSaltBytes, 0, SaltByteSize);

        var _currentHashBytes = new byte[HashByteSize];
        Buffer.BlockCopy(src, SaltByteSize + 1, _currentHashBytes, 0, HashByteSize);

        using (var bytes = new Rfc2898DeriveBytes(password, _currentSaltBytes, HashingIterationsCount))
        {
            _passwordHashBytes = bytes.GetBytes(SaltByteSize);
        }

        return AreHashesEqual(_currentHashBytes, _passwordHashBytes);
    }

    private static bool AreHashesEqual(byte[] firstHash, byte[] secondHash)
    {
        var _minHashLength = firstHash.Length <= secondHash.Length ? firstHash.Length : secondHash.Length;
        var xor = firstHash.Length ^ secondHash.Length;
        for (var i = 0; i < _minHashLength; i++)
            xor |= firstHash[i] ^ secondHash[i];
        return 0 == xor;
    }
}