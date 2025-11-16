using FluentValidation;
using Nps.Application.Dtos;
namespace Nps.Application.Validation;

public class LoginRequestValidator : AbstractValidator<LoginRequestDto>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Username).NotEmpty().WithMessage("El usuario es obligatorio");
        RuleFor(x => x.Password).NotEmpty().WithMessage("La contraseña es obligatoria");
    }
}
