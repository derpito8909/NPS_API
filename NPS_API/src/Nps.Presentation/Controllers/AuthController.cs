using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nps.Application.Dtos.Login;
using Nps.Application.Auth.Login;
using Nps.Application.Auth.Refresh;
using Microsoft.AspNetCore.Authorization;

namespace Nps.Presentation.Controllers;

/// <summary>
/// Controlador encargado de manejar las operaciones de autenticación,
/// como el inicio de sesión y la renovación de tokens.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;


    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="AuthController"/>.
    /// </summary>
    /// <param name="mediator">Instancia de IMediator para enviar comandos y consultas.</param>
    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Maneja la solicitud de inicio de sesión.
    /// </summary>
    /// <param name="model">Datos de inicio de sesión.</param>
    /// <returns>Respuesta con los tokens de autenticación.</returns>
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto model)
    {
        var command = new LoginCommand(model.Username, model.Password);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Maneja la solicitud de renovación de token.
    /// </summary>
    /// <param name="model">Datos de la solicitud de renovación de token.</param>
    /// <returns>Respuesta con el nuevo token de autenticación.</returns>
    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponseDto>> Refresh([FromBody] RefreshTokenRequestDto model)
    {
        var command = new RefreshTokenCommand(model.RefreshToken);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
