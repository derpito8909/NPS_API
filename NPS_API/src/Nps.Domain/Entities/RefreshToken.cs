namespace Nps.Domain.Entities;

/// <summary>
/// Representa un refresh token asociado a un usuario, utilizado
/// para renovar el token de acceso sin pedir credenciales nuevamente.
/// </summary>
public class RefreshToken
{
    private RefreshToken() { }

    /// <summary>
    /// Crea una nueva instancia de refresh token para un usuario específico.
    /// </summary>
    /// <param name="userId">Identificador del usuario.</param>
    /// <param name="token">Valor del token.</param>
    /// <param name="expiresAt">Fecha y hora de expiración (UTC).</param>
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

    /// <summary>
    /// Identificador único del refresh token en la base de datos.
    /// </summary>
    public int Id { get; private set; }
    /// <summary>
    /// Identificador del usuario propietario del token.
    /// </summary>
    public int UserId { get; private set; }
    /// <summary>
    /// Valor del token que se envía al cliente.
    /// </summary>
    public string Token { get; private set; } = null!;
    /// <summary>
    /// Fecha y hora de expiración del token (UTC).
    /// </summary>
    public DateTime ExpiresAt { get; private set; }
    /// <summary>
    /// Indica si el token ha sido revocado y ya no puede utilizarse.
    /// </summary>
    public bool IsRevoked { get; private set; }

    /// <summary>
    /// Marca el refresh token como revocado para evitar que pueda reutilizarse.
    /// </summary>
    public void Revoke()
    {
        IsRevoked = true;
    }

    /// <summary>
    /// Indica si el refresh token se encuentra activo (no revocado y no expirado).
    /// </summary>
    /// <returns><c>true</c> si el token está activo; en caso contrario, <c>false</c>.</returns>
    public bool IsActive() => !IsRevoked && ExpiresAt > DateTime.UtcNow;
}
