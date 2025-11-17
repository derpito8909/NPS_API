using FluentValidation;
using Nps.Application.Dtos.Nps;

namespace Nps.Application.Validation;

public class VoteNpsRequestValidator
    : AbstractValidator<VoteNpsRequestDto>
{
    public VoteNpsRequestValidator()
    {
        RuleFor(x => x.Score)
            .InclusiveBetween(0, 10)
            .WithMessage("El puntaje debe estar entre 0 y 10.");
    }
}