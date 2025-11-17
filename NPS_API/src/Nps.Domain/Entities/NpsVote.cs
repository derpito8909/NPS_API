namespace Nps.Domain.Entities;

/// <summary>
/// Representa un voto emitido por un usuario para una pregunta NPS específica.
/// </summary>
public class NpsVote
{
    private NpsVote() { }

    /// <summary>
    /// Crea un nuevo voto NPS asegurando que el puntaje esté en el rango válido.
    /// </summary>
    /// <param name="questionId">Id de la pregunta NPS.</param>
    /// <param name="userId">Id del usuario.</param>
    /// <param name="score">Puntaje de 0 a 10.</param>
    public NpsVote(int questionId, int userId, int score)
    {
        QuestionId = questionId;
        UserId = userId;
        SetScore(score);
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>Id del voto.</summary>
    public int Id { get; private set; }
    /// <summary>Id de la pregunta NPS asociada.</summary>
    public int QuestionId { get; private set; }
    /// <summary>Id del usuario que emitió el voto.</summary>
    public int UserId { get; private set; }
    /// <summary>Puntaje asignado por el usuario (0 a 10).</summary>
    public int Score { get; private set; }
    /// <summary>Fecha y hora de creación del voto (UTC).</summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Establece el puntaje del voto, asegurando que esté entre 0 y 10.
    /// </summary>
    /// /// <exception cref="ArgumentOutOfRangeException">Se lanza si el puntaje está fuera de rango.</exception>
    public void SetScore(int score)
    {
        if (score < 0 || score > 10)
            throw new ArgumentOutOfRangeException(nameof(score), "El puntaje debe estar entre 0 y 10.");

        Score = score;
    }
}