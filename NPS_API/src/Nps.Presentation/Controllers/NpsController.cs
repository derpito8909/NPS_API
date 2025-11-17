using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nps.Application.Dtos.Nps;
using Nps.Application.Nps.Result;
using Nps.Application.Nps.Vote;

namespace Nps.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NpsController : ControllerBase
{
    private readonly IMediator _mediator;

    public NpsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("vote")]
    [Authorize(Roles = "Voter")]
    public async Task<ActionResult<VoteNpsResponseDto>> Vote([FromBody] VoteNpsRequestDto model)
    {
        var userIdString =
            User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ??
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
        {
            return Unauthorized("No se pudo identificar al usuario desde el token.");
        }

        var command = new VoteNpsCommand(userId, model.Score);
        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpGet("result")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<NpsResultDto>> GetResult()
    {
        var query = new GetNpsResultQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
