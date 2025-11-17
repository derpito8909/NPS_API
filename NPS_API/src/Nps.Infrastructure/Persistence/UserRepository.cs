using Dapper;
using Nps.Application.Interfaces.Persistence;
using Nps.Domain.Entities;
using Nps.Domain.Enums;

namespace Nps.Infrastructure.Persistence;

public class UserRepository : IUserRepository
{
    private readonly DapperContext _context;

    public UserRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        const string sql = @"SELECT Id, Username, PasswordHash, Role, 
                                    FailedLoginAttempts, IsLocked, LastLoginAt
                             FROM Users
                             WHERE Username = @Username";

        using var connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Username = username });
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        const string sql = @"SELECT Id, Username, PasswordHash, Role, 
                                    FailedLoginAttempts, IsLocked, LastLoginAt
                             FROM Users
                             WHERE Id = @Id";

        using var connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
    }

    public async Task UpdateAsync(User user)
    {
        const string sql = @"
            UPDATE Users
            SET Username = @Username,
                PasswordHash = @PasswordHash,
                Role = @Role,
                FailedLoginAttempts = @FailedLoginAttempts,
                IsLocked = @IsLocked,
                LastLoginAt = @LastLoginAt
            WHERE Id = @Id";

        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(sql, new
        {
            user.Username,
            user.PasswordHash,
            Role = (int)user.Role,
            user.FailedLoginAttempts,
            user.IsLocked,
            user.LastLoginAt,
            user.Id
        });
    }

    public async Task AddAsync(User user)
    {
        const string sql = @"
            INSERT INTO Users (Username, PasswordHash, Role, FailedLoginAttempts, IsLocked, LastLoginAt)
            VALUES (@Username, @PasswordHash, @Role, @FailedLoginAttempts, @IsLocked, @LastLoginAt);

            SELECT CAST(SCOPE_IDENTITY() as int);";

        using var connection = _context.CreateConnection();
        var id = await connection.ExecuteScalarAsync<int>(sql, new
        {
            user.Username,
            user.PasswordHash,
            Role = (int)user.Role,
            user.FailedLoginAttempts,
            user.IsLocked,
            user.LastLoginAt
        });

        typeof(User)
            .GetProperty(nameof(User.Id))!
            .SetValue(user, id);
    }
    public async Task<User?> GetAnyAdminAsync()
    {
        const string sql = @"
        SELECT TOP 1 Id, Username, PasswordHash, Role,
                     FailedLoginAttempts, IsLocked, LastLoginAt
        FROM Users
        WHERE Role = @Role;
    ";

        using var connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Role = (int)UserRole.Admin });
    }
}

