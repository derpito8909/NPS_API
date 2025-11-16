using Nps.Domain.Enums;
namespace Nps.Domain.Entities;

public class User
{
    private User() { }

    public User(string username, string passwordHash, UserRole role)
    {
        SetUsername(username);
        SetPasswordHash(passwordHash);
        Role = role;
        FailedLoginAttempts = 0;
        IsLocked = false;
    }
    public int Id { get; private set; }
    public string Username { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public UserRole Role { get; private set; }
    public int FailedLoginAttempts { get; private set; }
    public bool IsLocked { get; private set; }
    public DateTime? LastLoginAt { get; private set; }

    public void SetUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("El nombre de usuario es obligatorio.", nameof(username));

        Username = username.Trim();
    }

    public void SetPasswordHash(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("El hash de la contraseña es obligatorio.", nameof(passwordHash));

        PasswordHash = passwordHash;
    }
    public void RegisterFailedLoginAttempt(int maxAttemptsToLock = 3)
    {
        if (IsLocked) return;

        FailedLoginAttempts++;

        if (FailedLoginAttempts >= maxAttemptsToLock)
            IsLocked = true;
    }
    public void ResetFailedLoginAttempts()
    {
        FailedLoginAttempts = 0;
        IsLocked = false;
    }

    public void SetLastLogin(DateTime dateTimeUtc)
    {
        LastLoginAt = dateTimeUtc;
    }
}