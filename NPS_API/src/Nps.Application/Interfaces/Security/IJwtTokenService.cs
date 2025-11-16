using Nps.Domain.Entities;
namespace Nps.Application.Interfaces.Security;

public record AccessTokenResult(string Token, DateTime ExpiresAt);
public record RefreshTokenResult(string Token, DateTime ExpiresAt);
public interface IJwtTokenService
{
    AccessTokenResult GenerateAccessToken(User user);
    RefreshTokenResult GenerateRefreshToken();
}
