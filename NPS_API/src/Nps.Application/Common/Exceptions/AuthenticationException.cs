namespace Nps.Application.Common.Exceptions;

/// <summary>
/// Representa un error de autenticación, por ejemplo credenciales inválidas
/// o tokens no válidos.
/// </summary>
public class AuthenticationException : AppException
{
    /// <summary>
    /// Crea una nueva instancia de la clase <see cref="AuthenticationException"/>.
    /// </summary>
    /// <param name="message">Mensaje descriptivo del error.</param>
    public AuthenticationException(string message)
        : base(message)
    {
    }
}
