using Microsoft.AspNetCore.Mvc;
using Sprint1.Application.UseCase;
using Sprint1.DTOs.Auth;
using Swashbuckle.AspNetCore.Annotations;

namespace Sprint1.Controllers;

[ApiController]
[Route("api/[controller]")]
[SwaggerTag("Autenticação - Endpoints para login e geração de tokens JWT")]
public class AuthController : ControllerBase
{
    private readonly IAuthUseCase _authUseCase;

    public AuthController(IAuthUseCase authUseCase)
    {
        _authUseCase = authUseCase;
    }

    [HttpPost("login")]
    [SwaggerOperation(
        Summary = "Realizar login", 
        Description = "Autentica um usuário e retorna um token JWT válido")]
    [ProducesResponseType(typeof(TokenResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var tokenResponse = await _authUseCase.LoginAsync(loginDto);
        return Ok(tokenResponse);
    }
}

// Made with Bob
