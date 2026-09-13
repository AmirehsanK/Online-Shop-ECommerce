using Application.Security;
using Application.Services.Impelementation;
using Application.Services.Interfaces;
using Domain.ViewModel.User;
using Domain.ViewModel.User.Admin;
using Infra.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Tests;

public class PasswordTests
{
    private static readonly PasswordHasherService Hasher = new();

    private static IConfiguration Config() => new ConfigurationBuilder()
        .AddInMemoryCollection(new Dictionary<string, string?> { ["ApplicationSettings:DomainLink"] = "https://shop.test" })
        .Build();

    [Fact]
    public async Task A_new_hash_verifies_the_right_password_only()
    {
        var hash = await Hasher.EncodePasswordAsync("correct horse");

        Assert.True(await Hasher.VerifyPasswordAsync(hash, "correct horse"));
        Assert.False(await Hasher.VerifyPasswordAsync(hash, "wrong horse"));
        Assert.False(Hasher.NeedsRehash(hash));
    }

    [Fact]
    public async Task Hashes_from_both_older_formats_still_verify_and_are_flagged_for_upgrade()
    {
        var signUpFormat = PasswordHasher.HashPassword("legacy");
        var resetFormat = "c2FsdHNhbHRzYWx0c2FsdA==;" + Convert.ToBase64String(
            System.Security.Cryptography.Rfc2898DeriveBytes.Pbkdf2("legacy", Convert.FromBase64String("c2FsdHNhbHRzYWx0c2FsdA=="),
                10_000, System.Security.Cryptography.HashAlgorithmName.SHA256, 32));

        foreach (var hash in new[] { signUpFormat, resetFormat })
        {
            Assert.True(await Hasher.VerifyPasswordAsync(hash, "legacy"));
            Assert.False(await Hasher.VerifyPasswordAsync(hash, "other"));
            Assert.True(Hasher.NeedsRehash(hash));
        }
    }

    [Theory]
    [InlineData("")]
    [InlineData("plain-text-password")]
    [InlineData("pbkdf2-sha256$abc$!!$??")]
    public async Task Malformed_hashes_are_a_failed_check_not_an_exception(string hash)
    {
        Assert.False(await Hasher.VerifyPasswordAsync(hash, "anything"));
    }

    [Fact]
    public async Task Signing_in_upgrades_an_old_hash()
    {
        using var db = new TestDatabase();
        var user = await db.AddUserAsync("old@test", PasswordHasher.HashPassword("secret-1"));

        await using (var context = db.NewContext())
        {
            var users = new UserService(new UserRepository(context), Config(), new NullEmailSender(), Hasher);
            Assert.Equal(Domain.Enums.LoginUserEnum.Success,
                await users.LoginUserAsync(new LoginUserViewModel { Email = "old@test", Password = "secret-1" }));
        }

        await using var check = db.NewContext();
        var stored = (await check.Users.SingleAsync(u => u.Id == user.Id)).Password;
        Assert.StartsWith("pbkdf2-sha256$", stored);
        Assert.True(await Hasher.VerifyPasswordAsync(stored, "secret-1"));
    }

    /// <summary>Reset used to write a format that sign-in could not read.</summary>
    [Fact]
    public async Task After_a_password_reset_the_new_password_signs_in_and_the_link_stops_working()
    {
        using var db = new TestDatabase();
        var user = await db.AddUserAsync("forgot@test", PasswordHasher.HashPassword("old-password"));

        await using (var context = db.NewContext())
        {
            var passwords = new PasswordService(Config(), new UserRepository(context), Hasher, new NullEmailSender());
            Assert.True(await passwords.ResetPasswordAsync(user.EmailActiveCode, "new-password"));
        }

        await using (var context = db.NewContext())
        {
            var users = new UserService(new UserRepository(context), Config(), new NullEmailSender(), Hasher);
            Assert.Equal(Domain.Enums.LoginUserEnum.Success,
                await users.LoginUserAsync(new LoginUserViewModel { Email = "forgot@test", Password = "new-password" }));

            var passwords = new PasswordService(Config(), new UserRepository(context), Hasher, new NullEmailSender());
            Assert.False(await passwords.ResetPasswordAsync(user.EmailActiveCode, "attacker-password"));
        }
    }

    [Fact]
    public async Task Admin_created_users_get_a_hashed_password()
    {
        using var db = new TestDatabase();

        await using (var context = db.NewContext())
        {
            var users = new UserService(new UserRepository(context), Config(), new NullEmailSender(), Hasher);
            await users.CreateUserAsync(new CreateUserViewModel { Email = "staff@test", Password = "staff-password" });
        }

        await using var check = db.NewContext();
        var stored = (await check.Users.SingleAsync(u => u.Email == "staff@test")).Password;
        Assert.NotEqual("staff-password", stored);
        Assert.True(await Hasher.VerifyPasswordAsync(stored, "staff-password"));
    }

    [Fact]
    public async Task The_edit_form_never_carries_the_stored_hash()
    {
        using var db = new TestDatabase();
        var user = await db.AddUserAsync("edit@test", await Hasher.EncodePasswordAsync("pw-123456"));

        await using var context = db.NewContext();
        var users = new UserService(new UserRepository(context), Config(), new NullEmailSender(), Hasher);
        Assert.Null((await users.GetUserForEditAsync(user.Id)).Password);
    }

    [Fact]
    public async Task Updating_a_profile_cannot_grant_admin_change_email_or_deactivate_the_account()
    {
        using var db = new TestDatabase();
        var user = await db.AddUserAsync("me@test");

        await using (var context = db.NewContext())
        {
            var users = new UserService(new UserRepository(context), Config(), new NullEmailSender(), Hasher);
            await users.UpdateProfileAsync(user.Id, new EditUserViewModel
            {
                FirstName = "New",
                LastName = "Name",
                Address = "Somewhere",
                PhoneNumber = "09129999999",
                Email = "someone-else@test",
                IsAdmin = true,
                IsEmailActive = false,
                Password = "hijack"
            });
        }

        await using var check = db.NewContext();
        var saved = await check.Users.SingleAsync(u => u.Id == user.Id);
        Assert.Equal("New", saved.FirstName);
        Assert.Equal("me@test", saved.Email);
        Assert.False(saved.IsAdmin);
        Assert.True(saved.IsEmailActive);
        Assert.Equal("not-a-real-hash", saved.Password);
    }

    private sealed class NullEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string recipient, string subject, string body) => Task.CompletedTask;
    }
}
