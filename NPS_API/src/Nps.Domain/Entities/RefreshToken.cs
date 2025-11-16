namespace Nps.Domain.Entities;

public class RefreshToken
{
    private RefreshToken() { }

    public RefreshToken(int userId, string token, DateTime expiresAt)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentException("El token no puede ser vacío.", nameof(token));

        if (expiresAt <= DateTime.UtcNow)
            throw new ArgumentException("La fecha de expiración debe ser futura.", nameof(expiresAt));

        UserId = userId;
        Token = token;
        ExpiresAt = expiresAt;
        IsRevoked = false;
    }

    public int Id { get; private set; }
    public int UserId { get; private set; }
    public string Token { get; private set; } = null!;
    public DateTime ExpiresAt { get; private set; }
    public bool IsRevoked { get; private set; }

    public void Revoke()
    {
        IsRevoked = true;
    }

    public bool IsActive() => !IsRevoked && ExpiresAt > DateTime.UtcNow;
}
