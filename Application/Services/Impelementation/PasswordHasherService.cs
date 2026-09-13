using System.Globalization;
using System.Security.Cryptography;
using Application.Security;
using Application.Services.Interfaces;

namespace Application.Services.Impelementation;

/// <summary>
/// The one place passwords are hashed and checked.
///
/// The app used to have two hashers with incompatible formats: sign-up and login used
/// <see cref="PasswordHasher"/> (PBKDF2-SHA1), while password reset and change used a
/// "salt;hash" PBKDF2-SHA256 format. A reset therefore wrote a hash that login could not
/// read, and changing a password never matched the old one. New hashes use a
/// self-describing format that carries its own iteration count; both older formats are
/// still accepted so existing accounts keep working, and <see cref="NeedsRehash"/> lets
/// login upgrade them.
/// </summary>
public class PasswordHasherService : IPasswordHasher
{
    private const string Prefix = "pbkdf2-sha256";
    private const int SaltSize = 16;
    private const int KeySize = 32;

    // OWASP's current recommendation for PBKDF2-HMAC-SHA256.
    public const int Iterations = 600_000;

    private const int SemicolonFormatIterations = 10_000;

    public Task<string> EncodePasswordAsync(string password)
    {
        if (string.IsNullOrEmpty(password))
            throw new ArgumentNullException(nameof(password));

        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, KeySize);
        return Task.FromResult(
            $"{Prefix}${Iterations.ToString(CultureInfo.InvariantCulture)}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}");
    }

    public Task<bool> VerifyPasswordAsync(string passwordHash, string inputPassword)
    {
        if (string.IsNullOrEmpty(passwordHash) || string.IsNullOrEmpty(inputPassword))
            return Task.FromResult(false);

        try
        {
            return Task.FromResult(Verify(passwordHash, inputPassword));
        }
        catch (FormatException)
        {
            return Task.FromResult(false);
        }
    }

    public bool NeedsRehash(string passwordHash)
    {
        var parts = passwordHash.Split('$');
        return parts.Length != 4
               || parts[0] != Prefix
               || !int.TryParse(parts[1], NumberStyles.None, CultureInfo.InvariantCulture, out var iterations)
               || iterations < Iterations;
    }

    private static bool Verify(string passwordHash, string inputPassword)
    {
        if (passwordHash.StartsWith(Prefix + "$", StringComparison.Ordinal))
        {
            var parts = passwordHash.Split('$');
            if (parts.Length != 4
                || !int.TryParse(parts[1], NumberStyles.None, CultureInfo.InvariantCulture, out var iterations)
                || iterations <= 0)
                return false;

            return FixedTimeMatch(inputPassword, parts[2], parts[3], iterations);
        }

        var semicolon = passwordHash.Split(';');
        if (semicolon.Length == 2)
            return FixedTimeMatch(inputPassword, semicolon[0], semicolon[1], SemicolonFormatIterations);

        // Oldest format: accounts created through sign-up before the formats were unified.
        return PasswordHasher.VerifyHashedPassword(passwordHash, inputPassword);
    }

    private static bool FixedTimeMatch(string password, string saltBase64, string hashBase64, int iterations)
    {
        var salt = Convert.FromBase64String(saltBase64);
        var expected = Convert.FromBase64String(hashBase64);
        var actual = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, expected.Length);
        return CryptographicOperations.FixedTimeEquals(expected, actual);
    }
}
