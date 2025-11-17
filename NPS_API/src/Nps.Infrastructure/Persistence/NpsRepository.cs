using Dapper;
using Nps.Domain.Entities;
using Nps.Application.Interfaces.Persistence;

namespace Nps.Infrastructure.Persistence;

public class NpsRepository : INpsRepository
{
    private readonly DapperContext _context;

    public NpsRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<NpsQuestion?> GetActiveQuestionAsync()
    {
        const string sql = @"
            SELECT TOP 1 Id, QuestionText, IsActive
            FROM NpsQuestions
            WHERE IsActive = 1;
        ";

        using var connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<NpsQuestion>(sql);
    }

    public async Task<bool> HasUserVotedAsync(int questionId, int userId)
    {
        const string sql = @"
            SELECT COUNT(1)
            FROM NpsVotes
            WHERE QuestionId = @QuestionId
              AND UserId = @UserId;
        ";

        using var connection = _context.CreateConnection();
        var count = await connection.ExecuteScalarAsync<int>(sql, new { QuestionId = questionId, UserId = userId });
        return count > 0;
    }

    public async Task AddVoteAsync(NpsVote vote)
    {
        const string sql = @"
            INSERT INTO NpsVotes (QuestionId, UserId, Score, CreatedAt)
            VALUES (@QuestionId, @UserId, @Score, @CreatedAt);
        ";

        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(sql, new
        {
            vote.QuestionId,
            vote.UserId,
            vote.Score,
            vote.CreatedAt
        });
    }

    private sealed class NpsStatsRow
    {
        public int Total { get; set; }
        public int Promoters { get; set; }
        public int Neutrals { get; set; }
        public int Detractors { get; set; }
    }

    public async Task<(int Total, int Promoters, int Neutrals, int Detractors)> GetNpsStatsAsync(int questionId)
    {
        const string sql = @"
            SELECT 
                COUNT(*) AS Total,
                SUM(CASE WHEN Score >= 9 THEN 1 ELSE 0 END) AS Promoters,
                SUM(CASE WHEN Score BETWEEN 7 AND 8 THEN 1 ELSE 0 END) AS Neutrals,
                SUM(CASE WHEN Score <= 6 THEN 1 ELSE 0 END) AS Detractors
            FROM NpsVotes
            WHERE QuestionId = @QuestionId;
        ";

        using var connection = _context.CreateConnection();
        var row = await connection.QuerySingleAsync<NpsStatsRow>(sql, new { QuestionId = questionId });

        return (row.Total, row.Promoters, row.Neutrals, row.Detractors);
    }
}
