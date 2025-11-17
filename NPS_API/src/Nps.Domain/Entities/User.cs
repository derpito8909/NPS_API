using Nps.Domain.Enums;
namespace Nps.Domain.Entities;

/// <summary>
/// Representa un usuario del sistema NPS con credenciales, rol y
/// estado de seguridad (intentos fallidos, bloqueo, último ingreso).
/// </summary>
public class User
{
    private User() { }

    /// <summary>
    /// Crea un nuevo usuario con los datos mínimos requeridos.
    /// </summary>
    /// <param name="username">Nombre de usuario.</param>
    /// <param name="passwordHash">Hash de la contraseña.</param>
    /// <param name="role">Rol asignado al usuario.</param>
    public User(string username, string passwordHash, UserRole role)
    {
        SetUsername(username);
        SetPasswordHash(passwordHash);
        Role = role;
        FailedLoginAttempts = 0;
        IsLocked = false;
    }
    /// <summary>
    /// Identificador único del usuario en la base de datos.
    /// </summary>
    public int Id { get; private set; }
    /// <summary>
    /// Nombre de usuario utilizado para iniciar sesión.
    /// </summary>
    public string Username { get; private set; } = null!;
    /// <summary>
    /// Hash de la contraseña del usuario. 
    /// Nunca debe almacenarse la contraseña en texto plano.
    /// </summary>
    public string PasswordHash { get; private set; } = null!;
    /// <summary>
    /// Rol asignado al usuario (por ejemplo, Administrador o Votante).
    /// </summary>
    public UserRole Role { get; private set; }
    /// <summary>
    /// Número de intentos fallidos de inicio de sesión.
    /// </summary>
    public int FailedLoginAttempts { get; private set; }
    /// <summary>
    /// Indica si la cuenta se encuentra bloqueada por seguridad.
    /// </summary>
    public bool IsLocked { get; private set; }
    /// <summary>
    /// Fecha y hora del último inicio de sesión exitoso (UTC).
    /// </summary>
    public DateTime? LastLoginAt { get; private set; }

    public void SetUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("El nombre de usuario es obligatorio.", nameof(username));

        Username = username.Trim();
    }
    /// <summary>
    /// Actualiza el hash de la contraseña del usuario.
    /// </summary>
    /// <param name="passwordHash">Nuevo hash de contraseña.</param>
    public void SetPasswordHash(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("El hash de la contraseña es obligatorio.", nameof(passwordHash));

        PasswordHash = passwordHash;
    }
    /// <summary>
    /// Registra un intento fallido de inicio de sesión y 
    /// bloquea la cuenta cuando se supera el umbral configurado.
    /// </summary>
    public void RegisterFailedLoginAttempt(int maxAttemptsToLock = 3)
    {
        if (IsLocked) return;

        FailedLoginAttempts++;

        if (FailedLoginAttempts >= maxAttemptsToLock)
            IsLocked = true;
    }
    /// <summary>
    /// Restablece el contador de intentos fallidos de inicio de sesión.
    /// Se utiliza después de un acceso exitoso.
    /// </summary>
    public void ResetFailedLoginAttempts()
    {
        FailedLoginAttempts = 0;
        IsLocked = false;
    }
    /// <summary>
    /// Actualiza la fecha y hora del último inicio de sesión exitoso.
    /// </summary>
    /// <param name="dateTimeUtc">Fecha y hora en UTC.</param>
    public void SetLastLogin(DateTime dateTimeUtc)
    {
        LastLoginAt = dateTimeUtc;
    }
}