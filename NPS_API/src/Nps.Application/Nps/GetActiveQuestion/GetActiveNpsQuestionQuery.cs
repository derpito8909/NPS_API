using MediatR;
using Nps.Application.Dtos.Nps;

namespace Nps.Application.Nps.GetActiveQuestion;

/// <summary>
/// Consulta para obtener la pregunta NPS actualmente activa.
/// </summary>
public record GetActiveNpsQuestionQuery() : IRequest<ActiveNpsQuestionDto>;
