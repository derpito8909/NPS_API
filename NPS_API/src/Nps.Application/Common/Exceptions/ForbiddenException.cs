namespace Nps.Application.Common.Exceptions;

/// <summary>
/// Representa un error de acceso prohibido, por ejemplo cuando un usuario
/// intenta acceder a un recurso sin los permisos necesarios.
/// </summary>
public class ForbiddenException : AppException
{
    /// <summary>
    /// Crea una nueva instancia de la clase <see cref="ForbiddenException"/>.
    /// </summary>
    /// <param name="message">Mensaje descriptivo del error.</param>
    public ForbiddenException(string message)
        : base(message)
    {
    }
}
