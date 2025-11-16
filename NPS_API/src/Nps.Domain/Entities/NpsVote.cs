namespace Nps.Domain.Entities;

public class NpsVote
{
    private NpsVote() { }

    public NpsVote(int questionId, int userId, int score)
    {
        QuestionId = questionId;
        UserId = userId;
        SetScore(score);
        CreatedAt = DateTime.UtcNow;
    }

    public int Id { get; private set; }
    public int QuestionId { get; private set; }
    public int UserId { get; private set; }
    public int Score { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public void SetScore(int score)
    {
        if (score < 0 || score > 10)
            throw new ArgumentOutOfRangeException(nameof(score), "El puntaje debe estar entre 0 y 10.");

        Score = score;
    }
}