using Nps.Domain.Entities;
namespace Nps.Application.Interfaces.Persistence;
/// <summary>
/// Define las operaciones de acceso a datos para la entidad <see cref="RefreshToken"/>.
/// </summary>
public interface IRefreshTokenRepository
{
    /// <summary>
    /// Agrega un nuevo token de actualización a la base de datos.
    /// </summary>
    /// <param name="token">Token de actualización a agregar.</param>
    Task AddAsync(RefreshToken token);

    /// <summary>
    /// Obtiene un token de actualización válido por su valor.
    /// </summary>
    /// <param name="token">Valor del token de actualización.</param>
    /// <returns>Instancia de <see cref="RefreshToken"/> si es válido; de lo contrario, <c>null</c>.</returns>
    Task<RefreshToken?> GetValidTokenAsync(string token);

    /// <summary>
    /// Revoca un token de actualización, marcándolo como inválido.
    /// </summary>
    /// <param name="token">Token de actualización a revocar.</param>
    Task RevokeAsync(RefreshToken token);
}
