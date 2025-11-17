using MediatR;
using Nps.Application.Dtos.Nps;
namespace Nps.Application.Nps.Result;

public record GetNpsResultQuery() : IRequest<NpsResultDto>;
