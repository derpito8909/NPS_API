using MediatR;
using Nps.Application.Dtos.Nps;

namespace Nps.Application.Nps.Vote;

public record VoteNpsCommand(int UserId, int Score) : IRequest<VoteNpsResponseDto>;