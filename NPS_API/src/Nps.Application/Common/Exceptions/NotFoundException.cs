namespace Nps.Application.Common.Exceptions;

/// <summary>
/// Representa un error de recurso no encontrado, por ejemplo cuando un
/// recurso solicitado no existe.
/// </summary>
public class NotFoundException : AppException
{
    /// <summary>
    /// Crea una nueva instancia de la clase <see cref="NotFoundException"/>.
    /// </summary>
    /// <param name="message">Mensaje descriptivo del error.</param>
    public NotFoundException(string message)
        : base(message)
    {
    }
}
