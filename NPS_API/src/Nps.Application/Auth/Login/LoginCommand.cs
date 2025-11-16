using MediatR;
using Nps.Application.Dtos;
namespace Nps.Application.Auth.Login;

public record LoginCommand(string Username, string Password) : IRequest<LoginResult>;
