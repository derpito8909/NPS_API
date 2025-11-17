using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Nps.Application.Common.Exceptions;

namespace Nps.Presentation.Middleware;

/// <summary>
/// Middleware encargado de capturar excepciones no controladas y
/// convertirlas en respuestas HTTP consistentes en formato JSON.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ExceptionHandlingMiddleware"/>.
    /// </summary>
    /// <param name="next">Delegado que representa el siguiente middleware en la tubería.</param>
    /// <param name="logger">Instancia del logger para registrar errores.</param>
    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Ejecuta el middleware y controla cualquier excepción que se produzca
    /// en la cadena de procesamiento de la petición HTTP.
    /// </summary>
    /// <param name="context">Contexto HTTP actual.</param>
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// Procesa la excepción capturada, registrándola y devolviendo
    /// una respuesta HTTP adecuada al tipo de error.
    /// </summary>
    /// <param name="context">Contexto HTTP.</param>
    /// <param name="ex">Excepción capturada.</param>
    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        _logger.LogError(ex, "Error no controlado");

        var statusCode = HttpStatusCode.InternalServerError;
        string title = "Error interno del servidor";

        switch (ex)
        {
            case AuthenticationException:
                statusCode = HttpStatusCode.Unauthorized; // 401
                title = "Error de autenticación";
                break;

            case ForbiddenException:
                statusCode = HttpStatusCode.Forbidden; // 403
                title = "Acceso denegado";
                break;

            case NotFoundException:
                statusCode = HttpStatusCode.NotFound; // 404
                title = "Recurso no encontrado";
                break;

            case ConflictException:
                statusCode = HttpStatusCode.Conflict; // 409
                title = "Conflicto en la operación";
                break;

            case BusinessException:
                statusCode = HttpStatusCode.BadRequest; // 400
                title = "Error de negocio";
                break;

            case UnauthorizedAccessException:
                statusCode = HttpStatusCode.Unauthorized;
                title = "No autorizado";
                break;
        }

        var problem = new
        {
            type = $"https://httpstatuses.io/{(int)statusCode}",
            title,
            status = (int)statusCode,
            detail = ex.Message,
            traceId = context.TraceIdentifier
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var json = JsonSerializer.Serialize(problem);
        await context.Response.WriteAsync(json);
    }
}
