using Nps.Domain.Entities;
namespace Nps.Application.Interfaces.Persistence;

public interface INpsRepository
{
    Task<NpsQuestion?> GetActiveQuestionAsync();
    Task<bool> HasUserVotedAsync(int questionId, int userId);
    Task AddVoteAsync(NpsVote vote);
    Task<(int total, int promoters, int detractors)> GetNpsStatsAsync(int questionId);
}
