using MediatR;
using Nps.Application.Interfaces.Persistence;
using Nps.Application.Dtos.Nps;

namespace Nps.Application.Nps.GetActiveQuestion;

/// <summary>
/// Maneja la consulta de pregunta NPS activa.
/// </summary>
public class GetActiveNpsQuestionQueryHandler
    : IRequestHandler<GetActiveNpsQuestionQuery, ActiveNpsQuestionDto>
{
    private readonly INpsRepository _npsRepository;

    public GetActiveNpsQuestionQueryHandler(INpsRepository npsRepository)
    {
        _npsRepository = npsRepository;
    }

    public async Task<ActiveNpsQuestionDto> Handle(GetActiveNpsQuestionQuery request, CancellationToken cancellationToken)
    {
        var question = await _npsRepository.GetActiveQuestionAsync();

        if (question is null || !question.IsActive)
        {
            return new ActiveNpsQuestionDto
            {
                Question = "No hay una pregunta NPS activa en este momento."
            };
        }

        return new ActiveNpsQuestionDto
        {
            Question = question.QuestionText
        };
    }
}

