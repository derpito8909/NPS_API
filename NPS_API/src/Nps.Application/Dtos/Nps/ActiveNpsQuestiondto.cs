namespace Nps.Application.Dtos.Nps;
/// <summary>
/// Representa la pregunta NPS activa que se muestra al votante.
/// </summary>
public class ActiveNpsQuestionDto
{
    /// <summary>
    /// Texto de la pregunta NPS.
    /// Si no hay una pregunta activa, contiene un mensaje informativo.
    /// </summary>
    public string Question { get; set; } = string.Empty;
}
