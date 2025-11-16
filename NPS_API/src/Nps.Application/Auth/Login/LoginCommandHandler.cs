using MediatR;
using Nps.Application.Dtos;
using Nps.Application.Interfaces.Persistence;
using Nps.Application.Interfaces.Security;
using Nps.Domain.Entities;
namespace Nps.Application.Auth.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IPasswordHasher _passwordHasher;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IJwtTokenService jwtTokenService,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _jwtTokenService = jwtTokenService;
        _passwordHasher = passwordHasher;
    }

    public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username);

        if (user is null || user.IsLocked)
        {
            throw new UnauthorizedAccessException("Usuario no existente o bloqueado");
        }

        var passwordOk = _passwordHasher.Verify(request.Password, user.PasswordHash);

        if (!passwordOk)
        {
            user.RegisterFailedLoginAttempt();
            await _userRepository.UpdateAsync(user);

            throw new UnauthorizedAccessException("Usuario o contraseña inválidos");
        }

        user.ResetFailedLoginAttempts();
        user.SetLastLogin(DateTime.UtcNow);
        await _userRepository.UpdateAsync(user);

        var accessToken = _jwtTokenService.GenerateAccessToken(user);
        var refreshTokenResult = _jwtTokenService.GenerateRefreshToken();

        var refreshToken = new RefreshToken(
            userId: user.Id,
            token: refreshTokenResult.Token,
            expiresAt: refreshTokenResult.ExpiresAt
        );

        await _refreshTokenRepository.AddAsync(refreshToken);

        return new LoginResponseDto
        {
            AccessToken = accessToken.Token,
            AccessTokenExpiresAt = accessToken.ExpiresAt,
            RefreshToken = refreshTokenResult.Token,
            RefreshTokenExpiresAt = refreshTokenResult.ExpiresAt,
            Username = user.Username,
            Role = user.Role.ToString()
        };
    }
}
