using Nps.Domain.Entities;
namespace Nps.Application.Interfaces.Persistence;

/// <summary>
/// Define las operaciones de acceso a datos para la entidad <see cref="User"/>.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Obtiene un usuario por su nombre de usuario.
    /// </summary>
    /// <param name="username">Nombre de usuario a buscar.</param>
    /// <returns>Instancia de <see cref="User"/> o <c>null</c> si no existe.</returns>
    Task<User?> GetByUsernameAsync(string username);
    /// <summary>
    /// Obtiene un usuario por su identificador.
    /// </summary>
    /// <param name="id">Identificador del usuario.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Instancia de <see cref="User"/> o <c>null</c> si no existe.</returns>
    Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken);
    /// <summary>
    /// Obtiene un usuario con rol administrador, si existe alguno.
    /// </summary>
    /// <returns>Instancia de <see cref="User"/> o <c>null</c> si no se encuentra un administrador.</returns>
    Task<User?> GetAnyAdminAsync();
    /// <summary>
    /// Actualiza la información persistida del usuario.
    /// </summary>
    /// <param name="user">Usuario a actualizar.</param>
    Task UpdateAsync(User user);
    /// <summary>
    /// Inserta un nuevo usuario en la base de datos.
    /// </summary>
    /// <param name="user">Usuario a insertar. El identificador se actualiza después del guardado.</param>
    Task AddAsync(User user);
}
