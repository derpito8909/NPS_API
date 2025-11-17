using MediatR;
using Nps.Application.Dtos.Nps;
using Nps.Application.Interfaces.Persistence;
using Nps.Domain.Entities;
using Nps.Domain.Enums;

namespace Nps.Application.Nps.Vote;

public class VoteNpsCommandHandler : IRequestHandler<VoteNpsCommand, VoteNpsResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly INpsRepository _npsRepository;

    public VoteNpsCommandHandler(
        IUserRepository userRepository,
        INpsRepository npsRepository)
    {
        _userRepository = userRepository;
        _npsRepository = npsRepository;
    }

    public async Task<VoteNpsResponseDto> Handle(VoteNpsCommand request, CancellationToken cancellationToken)
    {

        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user is null || user.IsLocked)
            throw new UnauthorizedAccessException("Usuario no válido para votar.");

        if (user.Role != UserRole.Voter)
            throw new UnauthorizedAccessException("Solo usuarios con rol Votante pueden votar.");

        var question = await _npsRepository.GetActiveQuestionAsync();
        if (question is null || !question.IsActive)
            throw new InvalidOperationException("No hay una pregunta NPS activa.");

        var alreadyVoted = await _npsRepository.HasUserVotedAsync(question.Id, user.Id);
        if (alreadyVoted)
            throw new InvalidOperationException("El usuario ya ha votado para esta pregunta.");

        var vote = new NpsVote(question.Id, user.Id, request.Score);
        await _npsRepository.AddVoteAsync(vote);

        string classification = request.Score switch
        {
            >= 9 => "Promotor",
            7 or 8 => "Neutro",
            _ => "Detractor"
        };

        return new VoteNpsResponseDto
        {
            Message = "Voto registrado correctamente.",
            Score = request.Score,
            Classification = classification
        };
    }
}
