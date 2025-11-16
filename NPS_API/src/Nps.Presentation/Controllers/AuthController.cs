using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nps.Application.Dtos;
using Nps.Application.Auth.Login;

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
}
