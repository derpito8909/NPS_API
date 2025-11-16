using MediatR;
using Nps.Application.Dtos;
namespace Nps.Application.Auth.Refresh;

public record RefreshTokenCommand(string RefreshToken) : IRequest<LoginResponseDto>;