using MediatR;
using Nps.Application.Dtos.Nps;
using Nps.Application.Interfaces.Persistence;

namespace Nps.Application.Nps.Result;

public class GetNpsResultQueryHandler
    : IRequestHandler<GetNpsResultQuery, NpsResultDto>
{
    private readonly INpsRepository _npsRepository;

    public GetNpsResultQueryHandler(INpsRepository npsRepository)
    {
        _npsRepository = npsRepository;
    }

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
