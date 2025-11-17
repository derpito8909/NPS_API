using MediatR;
using Nps.Application.Dtos.Login;
using Nps.Application.Interfaces.Persistence;
using Nps.Application.Interfaces.Security;
using Nps.Domain.Entities;
using Nps.Application.Common.Exceptions;

namespace Nps.Application.Auth.Refresh;

/// <summary>
/// Maneja el comando para refrescar el token de acceso utilizando un token de actualización válido.
/// </summary>
public class RefreshTokenCommandHandler
    : IRequestHandler<RefreshTokenCommand, LoginResponseDto>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtTokenService;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="RefreshTokenCommandHandler"/>.
    /// </summary>
    /// <param name="refreshTokenRepository">Repositorio de acceso a tokens de actualización.</param>
    /// <param name="userRepository">Repositorio de acceso a usuarios.</param>
    /// <param name="jwtTokenService">Servicio para generación de tokens JWT.</param>
    public RefreshTokenCommandHandler(
        IRefreshTokenRepository refreshTokenRepository,
        IUserRepository userRepository,
        IJwtTokenService jwtTokenService)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
    }

    /// <summary>
    /// Maneja la solicitud de refresco de token.
    /// </summary>
    /// <param name="request">Comando de refresco de token.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Respuesta de inicio de sesión con nuevos tokens.</returns>
    public async Task<LoginResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var storedToken = await _refreshTokenRepository.GetValidTokenAsync(request.RefreshToken);

        if (storedToken is null || !storedToken.IsActive())
        {
            throw new AuthenticationException("Refresh token inválido o expirado.");
        }

        var user = await _userRepository.GetByIdAsync(storedToken.UserId);

        if (user is null)
        {
            throw new AuthenticationException("Usuario no válido para refrescar sesión.");
        }

        if (user.IsLocked)
        {
            throw new ForbiddenException("La cuenta está bloqueada; no se puede refrescar la sesión.");
        }

        storedToken.Revoke();
        await _refreshTokenRepository.RevokeAsync(storedToken);

        var accessToken = _jwtTokenService.GenerateAccessToken(user);
        var newRefresh = _jwtTokenService.GenerateRefreshToken();

        var newRefreshEntity = new RefreshToken(
            userId: user.Id,
            token: newRefresh.Token,
            expiresAt: newRefresh.ExpiresAt
        );

        await _refreshTokenRepository.AddAsync(newRefreshEntity);

        return new LoginResponseDto
        {
            AccessToken = accessToken.Token,
            AccessTokenExpiresAt = accessToken.ExpiresAt,
            RefreshToken = newRefresh.Token,
            RefreshTokenExpiresAt = newRefresh.ExpiresAt,
            Username = user.Username,
            Role = user.Role.ToString()
        };
    }
}

