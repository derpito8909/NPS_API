using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nps.Application.Dtos.Nps;
using Nps.Application.Nps.Result;
using Nps.Application.Nps.Vote;
using Nps.Application.Nps.GetActiveQuestion;

namespace Nps.Presentation.Controllers;

/// <summary>
/// Controlador encargado de manejar las operaciones relacionadas con el NPS,
/// como votar y obtener resultados.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class NpsController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="NpsController"/>.
    /// </summary>
    /// <param name="mediator">Instancia de IMediator para enviar comandos y consultas.</param>
    public NpsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Maneja la solicitud de votación NPS.
    /// </summary>
    /// <param name="model">Datos de la votación NPS.</param>
    /// <returns>Respuesta con el resultado de la votación.</returns>
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

    /// <summary>
    /// Maneja la solicitud para obtener el resultado del NPS.
    /// </summary>
    /// <returns>Respuesta con el resultado del NPS.</returns>
    [HttpGet("result")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<NpsResultDto>> GetResult()
    {
        var query = new GetNpsResultQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Obtiene la pregunta NPS activa para mostrar al usuario votante.
    /// </summary>
    /// <returns>Pregunta activa o mensaje informativo si no hay.</returns>
    [HttpGet("active")]
    [Authorize(Roles = "Admin,Voter")]
    public async Task<ActionResult<ActiveNpsQuestionDto>> GetActiveQuestion()
    {
        var query = new GetActiveNpsQuestionQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
