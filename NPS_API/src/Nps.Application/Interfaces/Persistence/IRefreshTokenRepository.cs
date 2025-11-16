using Nps.Domain.Entities;
namespace Nps.Application.Interfaces.Persistence;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken token);
    Task<RefreshToken?> GetValidTokenAsync(int userId, string token);
    Task RevokeAsync(RefreshToken token);
}
