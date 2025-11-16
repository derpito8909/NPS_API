using MediatR;
using Nps.Application.Dtos;
namespace Nps.Application.Auth.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResult>
{
    public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        return new LoginResult
        {
            Success = true,
            Message = $"Usuario {request.Username} autenticado (falso, solo prueba)."
        };
    }
}
