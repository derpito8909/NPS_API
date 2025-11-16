using MediatR;
using Nps.Application.Dtos;
using Nps.Application.Interfaces.Persistence;
using Nps.Application.Interfaces.Security;
using Nps.Domain.Entities;

namespace Nps.Application.Auth.Refresh;

public class RefreshTokenCommandHandler
    : IRequestHandler<RefreshTokenCommand, LoginResponseDto>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtTokenService;

    public RefreshTokenCommandHandler(
        IRefreshTokenRepository refreshTokenRepository,
        IUserRepository userRepository,
        IJwtTokenService jwtTokenService)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<LoginResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var storedToken = await _refreshTokenRepository.GetValidTokenAsync(request.RefreshToken);

        if (storedToken is null || !storedToken.IsActive())
        {
            throw new UnauthorizedAccessException("Refresh token inválido o expirado.");
        }

        var user = await _userRepository.GetByIdAsync(storedToken.UserId);

        if (user is null || user.IsLocked)
        {
            throw new UnauthorizedAccessException("Usuario no válido para refrescar sesión.");
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

