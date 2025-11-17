namespace Nps.Application.Dtos.Nps;

public class NpsResultDto
{
    public string Question { get; set; } = default!;
    public int TotalVotes { get; set; }
    public int Promoters { get; set; }
    public int Neutrals { get; set; }
    public int Detractors { get; set; }
    public double Nps { get; set; }
}
