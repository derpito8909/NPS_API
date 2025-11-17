using MediatR;
using Nps.Application.Dtos.Login;
using Nps.Application.Interfaces.Persistence;
using Nps.Application.Interfaces.Security;
using Nps.Domain.Entities;
using Nps.Application.Common.Exceptions;

namespace Nps.Application.Auth.Login;

/// <summary>
/// manejador para el comando de inicio de sesión.
/// </summary>

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IPasswordHasher _passwordHasher;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="LoginCommandHandler"/>.
    /// </summary>
    /// <param name="userRepository">Repositorio de acceso a usuarios.</param>
    /// <param name="refreshTokenRepository">Repositorio de acceso a tokens de actualización.</param>
    /// <param name="jwtTokenService">Servicio para generación de tokens JWT.</param>
    /// <param name="passwordHasher">Servicio para verificación de contraseñas.</param>
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

    /// <summary>
    /// Maneja el comando de inicio de sesión.
    /// </summary>
    /// <param name="request">Comando con las credenciales de inicio de sesión.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Respuesta con los tokens de acceso y actualización, y datos del usuario.</returns>
    /// <exception cref="AuthenticationException">Se lanza cuando las credenciales son inválidas.</exception>
    /// <exception cref="ForbiddenException">Se lanza cuando la cuenta está bloqueada.</exception>
    public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username);

        if (user is null)
        {
            throw new AuthenticationException("Usuario o contraseña inválidos.");
        }

        if (user.IsLocked)
        {
            throw new ForbiddenException("La cuenta está bloqueada por múltiples intentos fallidos.");
        }

        var passwordOk = _passwordHasher.Verify(request.Password, user.PasswordHash);

        if (!passwordOk)
        {
            user.RegisterFailedLoginAttempt();
            await _userRepository.UpdateAsync(user);

            throw new AuthenticationException("Usuario o contraseña inválidos.");
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
