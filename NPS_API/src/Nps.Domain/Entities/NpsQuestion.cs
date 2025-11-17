namespace Nps.Domain.Entities;

/// <summary>
/// Representa una pregunta NPS configurada en el sistema.
/// </summary>
public class NpsQuestion
{
    private NpsQuestion() { }

    /// <summary>Crea una nueva pregunta NPS con el texto especificado y estado activo o inactivo.</summary>
    /// <param name="questionText">Texto de la pregunta.</param>
    /// <param name="isActive">Indica si la pregunta está activa.</param>
    public NpsQuestion(string questionText, bool isActive = true)
    {
        SetText(questionText);
        IsActive = isActive;
    }

    /// <summary>Id de la pregunta.</summary>
    public int Id { get; private set; }
    /// <summary>Texto de la pregunta mostrada al usuario.</summary>
    public string QuestionText { get; private set; } = null!;
    /// <summary>Indica si esta es la pregunta NPS activa.</summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Establece el texto de la pregunta NPS.
    /// </summary>
    /// <param name="text"></param>
    /// <exception cref="ArgumentException">Se lanza si el texto está vacío o es nulo.</exception>
    public void SetText(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("El texto de la pregunta es obligatorio.", nameof(text));

        QuestionText = text.Trim();
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
