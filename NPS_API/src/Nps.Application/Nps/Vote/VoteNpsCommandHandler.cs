using MediatR;
using Nps.Application.Dtos.Nps;
using Nps.Application.Interfaces.Persistence;
using Nps.Domain.Entities;
using Nps.Domain.Enums;
using Nps.Application.Common.Exceptions;

namespace Nps.Application.Nps.Vote;

/// <summary>
/// Maneja el caso de uso de registro de un voto NPS para un usuario.
/// Aplica las reglas de negocio de autorización y unicidad de voto.
/// </summary>
public class VoteNpsCommandHandler : IRequestHandler<VoteNpsCommand, VoteNpsResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly INpsRepository _npsRepository;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="VoteNpsCommandHandler"/>.
    /// </summary>
    /// <param name="userRepository">Repositorio de acceso a usuarios.</param>
    /// <param name="npsRepository">Repositorio de acceso a datos NPS.</param>
    public VoteNpsCommandHandler(
        IUserRepository userRepository,
        INpsRepository npsRepository)
    {
        _userRepository = userRepository;
        _npsRepository = npsRepository;
    }

    /// <summary>
    /// Ejecuta el registro de un voto NPS validando el usuario,
    /// la pregunta activa y que el usuario no haya votado antes.
    /// </summary>
    /// <param name="request">Comando con la información del usuario y el puntaje.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>
    /// Respuesta con la confirmación del voto y la clasificación
    /// (Promotor, Neutro o Detractor).
    /// </returns>
    /// <exception cref="AuthenticationException">
    /// Se lanza cuando el usuario no es válido.
    /// </exception>
    /// <exception cref="ForbiddenException">
    /// Se lanza cuando la cuenta está bloqueada o el usuario no tiene rol Voter.
    /// </exception>
    /// <exception cref="BusinessException">
    /// Se lanza cuando no existe una pregunta NPS activa.
    /// </exception>
    /// <exception cref="ConflictException">
    /// Se lanza cuando el usuario ya ha votado para la pregunta actual.
    /// </exception>
    public async Task<VoteNpsResponseDto> Handle(VoteNpsCommand request, CancellationToken cancellationToken)
    {

        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
            throw new AuthenticationException("Usuario no válido para votar.");

        if (user.IsLocked)
            throw new ForbiddenException("La cuenta está bloqueada; no puede votar.");

        if (user.Role != UserRole.Voter)
            throw new ForbiddenException("Solo usuarios con rol Votante pueden votar.");

        var question = await _npsRepository.GetActiveQuestionAsync();
        if (question is null || !question.IsActive)
            throw new BusinessException("No hay una pregunta NPS activa.");

        var alreadyVoted = await _npsRepository.HasUserVotedAsync(question.Id, user.Id);
        if (alreadyVoted)
            throw new ConflictException("El usuario ya ha votado para esta pregunta.");

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
