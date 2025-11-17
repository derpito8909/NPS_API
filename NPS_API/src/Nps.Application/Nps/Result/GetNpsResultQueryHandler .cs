using MediatR;
using Nps.Application.Dtos.Nps;
using Nps.Application.Interfaces.Persistence;

namespace Nps.Application.Nps.Result;


/// <summary>
/// Maneja la consulta del resultado NPS para la pregunta activa.
/// Devuelve el detalle de votos y el valor calculado del NPS.
/// </summary>
public class GetNpsResultQueryHandler
    : IRequestHandler<GetNpsResultQuery, NpsResultDto>
{
    private readonly INpsRepository _npsRepository;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="GetNpsResultQueryHandler"/>.
    /// </summary>
    /// <param name="npsRepository">Repositorio de acceso a datos NPS.</param>
    public GetNpsResultQueryHandler(INpsRepository npsRepository)
    {
        _npsRepository = npsRepository;
    }

    /// <summary>
    /// Obtiene el resultado NPS de la pregunta activa, incluyendo el total
    /// de votos, la distribución por tipo y el valor del NPS.
    /// </summary>
    /// <param name="request">Consulta sin parámetros adicionales.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Objeto <see cref="NpsResultDto"/> con el detalle del NPS.</returns>
    public async Task<NpsResultDto> Handle(GetNpsResultQuery request, CancellationToken cancellationToken)
    {
        var question = await _npsRepository.GetActiveQuestionAsync();
        if (question is null || !question.IsActive)
        {
            return new NpsResultDto
            {
                Question = "No hay pregunta activa.",
                TotalVotes = 0,
                Promoters = 0,
                Neutrals = 0,
                Detractors = 0,
                Nps = 0
            };
        }

        var (total, promoters, neutrals, detractors) = await _npsRepository.GetNpsStatsAsync(question.Id);

        double nps = total == 0
            ? 0
            : (promoters - detractors) * 100.0 / total;

        return new NpsResultDto
        {
            Question = question.QuestionText,
            TotalVotes = total,
            Promoters = promoters,
            Neutrals = neutrals,
            Detractors = detractors,
            Nps = nps
        };
    }
}
