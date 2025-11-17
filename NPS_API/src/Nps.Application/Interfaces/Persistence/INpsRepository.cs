using Nps.Domain.Entities;
namespace Nps.Application.Interfaces.Persistence;

/// <summary>
/// Define las operaciones de acceso a datos asociadas al módulo NPS
/// (preguntas, votos y estadísticas).
/// </summary>
public interface INpsRepository
{
    /// <summary>
    /// Obtiene la pregunta NPS actualmente activa.
    /// </summary>
    /// <returns>
    /// Instancia de <see cref="NpsQuestion"/> si existe una pregunta activa;
    /// en caso contrario, <c>null</c>.
    /// </returns>
    Task<NpsQuestion?> GetActiveQuestionAsync();

    /// <summary>
    /// Indica si un usuario ya ha registrado un voto para una pregunta NPS concreta.
    /// </summary>
    /// <param name="questionId">Identificador de la pregunta NPS.</param>
    /// <param name="userId">Identificador del usuario.</param>
    /// <returns>
    /// <c>true</c> si el usuario ya ha votado; en caso contrario, <c>false</c>.
    /// </returns>
    Task<bool> HasUserVotedAsync(int questionId, int userId);

    /// <summary>
    /// Registra un nuevo voto NPS en la base de datos.
    /// </summary>
    /// <param name="vote">Entidad de voto a persistir.</param>
    Task AddVoteAsync(NpsVote vote);

    /// <summary>
    /// Obtiene las estadísticas necesarias para el cálculo del NPS.
    /// </summary>
    /// <param name="questionId">Identificador de la pregunta NPS.</param>
    /// <returns>
    /// Una tupla con el total de votos, cantidad de promotores, neutrales y detractores.
    /// </returns>
    Task<(int Total, int Promoters, int Neutrals, int Detractors)> GetNpsStatsAsync(int questionId);
}
