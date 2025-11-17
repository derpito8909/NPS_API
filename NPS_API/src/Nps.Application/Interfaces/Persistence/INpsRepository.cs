using Nps.Domain.Entities;
namespace Nps.Application.Interfaces.Persistence;

public interface INpsRepository
{
    Task<NpsQuestion?> GetActiveQuestionAsync();
    Task<bool> HasUserVotedAsync(int questionId, int userId);
    Task AddVoteAsync(NpsVote vote);
    Task<(int Total, int Promoters, int Neutrals, int Detractors)> GetNpsStatsAsync(int questionId);
}
