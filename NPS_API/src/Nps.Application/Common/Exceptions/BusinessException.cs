namespace Nps.Application.Common.Exceptions;

/// <summary>
/// Representa un error de negocio, por ejemplo violaciones a reglas de negocio.
/// </summary>
public class BusinessException : AppException
{
    /// <summary>
    /// Crea una nueva instancia de la clase <see cref="BusinessException"/>.
    /// </summary>
    /// <param name="message">Mensaje descriptivo del error.</param>
    public BusinessException(string message)
        : base(message)
    {
    }
}