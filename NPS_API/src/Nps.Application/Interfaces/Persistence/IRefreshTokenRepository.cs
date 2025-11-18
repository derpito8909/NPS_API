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
    /// <param name="cancellationToken">Token de cancelación.</param>
    Task AddAsync(RefreshToken token, CancellationToken cancellationToken);

    /// <summary>
    /// Obtiene un refresh token por su valor exacto.
    /// No filtra por estado; la lógica de "activo" se delega a la entidad.
    /// </summary>
    Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);

    /// <summary>
    /// Revoca un token de actualización, marcándolo como inválido.
    /// </summary>
    /// <param name="token">Token de actualización a revocar.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    Task RevokeAsync(RefreshToken token, CancellationToken cancellationToken);
}
