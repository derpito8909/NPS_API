namespace Nps.Application.Common.Exceptions;

/// <summary>
/// Representa un error de conflicto, por ejemplo intentos de crear recursos duplicados.
/// </summary>
public class ConflictException : AppException
{
    /// <summary>
    /// Crea una nueva instancia de la clase <see cref="ConflictException"/>.
    /// </summary>
    /// <param name="message">Mensaje descriptivo del error.</param>
    public ConflictException(string message)
        : base(message)
    {
    }
}
