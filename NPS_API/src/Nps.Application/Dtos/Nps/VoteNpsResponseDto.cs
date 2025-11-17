namespace Nps.Application.Dtos.Nps;

public class VoteNpsResponseDto
{
    public string Message { get; set; } = default!;
    public int Score { get; set; }
    public string Classification { get; set; } = default!;
}
