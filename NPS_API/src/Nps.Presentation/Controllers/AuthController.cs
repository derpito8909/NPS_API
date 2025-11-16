using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nps.Application.Dtos;
using Nps.Application.Auth.Login;
using Nps.Application.Auth.Refresh;
using Microsoft.AspNetCore.Authorization;

namespace Nps.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto model)
    {
        var command = new LoginCommand(model.Username, model.Password);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponseDto>> Refresh([FromBody] RefreshTokenRequestDto model)
    {
        var command = new RefreshTokenCommand(model.RefreshToken);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
