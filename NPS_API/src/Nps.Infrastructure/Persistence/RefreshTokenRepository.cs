using Dapper;
using Nps.Application.Interfaces.Persistence;
using Nps.Domain.Entities;

namespace Nps.Infrastructure.Persistence;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly DapperContext _context;

    public RefreshTokenRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task AddAsync(RefreshToken token)
    {
        const string sql = @"
            INSERT INTO RefreshTokens (UserId, Token, ExpiresAt, IsRevoked)
            VALUES (@UserId, @Token, @ExpiresAt, @IsRevoked);
        ";

        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(sql, new
        {
            token.UserId,
            token.Token,
            token.ExpiresAt,
            token.IsRevoked
        });
    }

    public async Task<RefreshToken?> GetValidTokenAsync(int userId, string tokenString)
    {
        const string sql = @"
            SELECT Id, UserId, Token, ExpiresAt, IsRevoked
            FROM RefreshTokens
            WHERE UserId = @UserId
              AND Token = @Token
              AND IsRevoked = 0
              AND ExpiresAt > SYSUTCDATETIME();
        ";

        using var connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<RefreshToken>(sql, new
        {
            UserId = userId,
            Token = tokenString
        });
    }

    public async Task RevokeAsync(RefreshToken token)
    {
        const string sql = @"
            UPDATE RefreshTokens
            SET IsRevoked = 1
            WHERE Id = @Id;
        ";

        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(sql, new { token.Id });
    }
}

