namespace Nps.Domain.Entities;

public class NpsQuestion
{
    private NpsQuestion() { }

    public NpsQuestion(string questionText, bool isActive = true)
    {
        SetText(questionText);
        IsActive = isActive;
    }

    public int Id { get; private set; }
    public string QuestionText { get; private set; } = null!;
    public bool IsActive { get; private set; }

    public void SetText(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("El texto de la pregunta es obligatorio.", nameof(text));

        QuestionText = text.Trim();
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
