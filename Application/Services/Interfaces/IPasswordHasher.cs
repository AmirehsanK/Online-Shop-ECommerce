namespace Application.Services.Interfaces;

public interface IPasswordHasher
{
    Task<string> EncodePasswordAsync(string password);
    Task<bool> VerifyPasswordAsync(string passwordHash, string inputPassword);

    /// <summary>
    /// True when <paramref name="passwordHash"/> was produced by an older scheme or weaker
    /// settings, so it should be replaced the next time the plain password is known.
    /// </summary>
    bool NeedsRehash(string passwordHash);
}
