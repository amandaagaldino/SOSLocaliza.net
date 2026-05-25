using System.Net;
using System.Text.Json;
using Sprint1.Domain.Exceptions;
using Sprint1.DTOs;

namespace Sprint1.Middleware;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    public GlobalExceptionHandlerMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlerMiddleware> logger,
        IHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var errorResponse = exception switch
        {
            UsuarioNotFoundException notFoundEx => new ErrorResponse(
                type: "UsuarioNotFound",
                message: notFoundEx.Message,
                statusCode: (int)HttpStatusCode.NotFound,
                path: context.Request.Path
            ),

            EmailDuplicadoException emailDupEx => new ErrorResponse(
                type: "EmailDuplicado",
                message: emailDupEx.Message,
                statusCode: (int)HttpStatusCode.BadRequest,
                path: context.Request.Path
            ),

            CpfDuplicadoException cpfDupEx => new ErrorResponse(
                type: "CpfDuplicado",
                message: cpfDupEx.Message,
                statusCode: (int)HttpStatusCode.BadRequest,
                path: context.Request.Path
            ),

            ArgumentException argEx => new ErrorResponse(
                type: "ValidationError",
                message: argEx.Message,
                statusCode: (int)HttpStatusCode.BadRequest,
                path: context.Request.Path
            ),

            InvalidOperationException invOpEx => new ErrorResponse(
                type: "InvalidOperation",
                message: invOpEx.Message,
                statusCode: (int)HttpStatusCode.BadRequest,
                path: context.Request.Path
            ),

            _ => new ErrorResponse(
                type: "InternalServerError",
                message: _environment.IsDevelopment() 
                    ? exception.Message 
                    : "Ocorreu um erro interno no servidor. Por favor, tente novamente mais tarde.",
                statusCode: (int)HttpStatusCode.InternalServerError,
                path: context.Request.Path
            )
        };

        // Adicionar stack trace apenas em desenvolvimento
        if (_environment.IsDevelopment())
        {
            errorResponse.StackTrace = exception.StackTrace;
        }

        context.Response.StatusCode = errorResponse.StatusCode;

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        var json = JsonSerializer.Serialize(errorResponse, options);
        await context.Response.WriteAsync(json);
    }
}

// Made with Bob
