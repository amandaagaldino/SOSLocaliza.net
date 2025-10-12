using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sprint1.DTOs.Usuario;
using Sprint1.Infrastructure.Data;
using Sprint1.Infrastructure.Data.UseCase;
using Swashbuckle.AspNetCore.Annotations;

namespace Sprint1.Controllers;

[ApiController]
[Route("api/[controller]")]
[SwaggerTag("SOSLocaliza - EndPoint em relação a criação de usuario (CRUD)")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioUseCase _usuarioUseCase;
    private readonly ApplicationDbContext _context;

    public UsuarioController(
        IUsuarioUseCase usuarioUseCase,
        ApplicationDbContext context)
    {
        _usuarioUseCase = usuarioUseCase;
        _context = context;
    }

    [HttpGet("test-connection")]
    [SwaggerOperation(Summary = "Teste a conexao com o bando de dados")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> TestConnection()
    {
        try
        {
            var connection = _context.Database.GetDbConnection();
            string? databaseName = null;
            int count = 0;

            try
            {
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                databaseName = connection.Database;
                
                count = await _context.Usuarios.CountAsync();
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    await connection.CloseAsync();
                }
            }
            
            return Ok(new
            {
                success = true,
                message = "Conexão com Oracle Cloud (Autonomous Database) estabelecida com sucesso!",
                database = databaseName ?? "N/A",
                totalUsuarios = count,
                servidor = "Oracle Autonomous Database - São Paulo",
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            var connection = _context.Database.GetDbConnection();
            if (connection.State == System.Data.ConnectionState.Open)
            {
                await connection.CloseAsync();
            }

            return StatusCode(500, new
            {
                success = false,
                message = "Erro ao conectar com o banco de dados",
                error = ex.Message,
                innerError = ex.InnerException?.Message,
                stackTrace = ex.StackTrace,
                connectionState = connection.State.ToString(),
                timestamp = DateTime.UtcNow
            });
        }
    }


    [HttpPost]
    [SwaggerOperation(Summary = "Criar novo usuário", Description = "Infome o nome completo, email, data nascimento e CPF do usuário")]
    [ProducesResponseType(typeof(UsuarioResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateUsuarioDto dto)
    {
        try
        {
            var usuario = await _usuarioUseCase.CreateUsuarioAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, usuario);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Listar usuário por ID", Description = "Infome o ID do usuário e visualize suas informações")]
    [ProducesResponseType(typeof(UsuarioResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var usuario = await _usuarioUseCase.GetUsuarioByIdAsync(id);
        
        if (usuario == null)
            return NotFound(new { message = "Usuário não encontrado" });

        return Ok(usuario);
    }
    
    [HttpGet]
    [SwaggerOperation(Summary = "Listar usuarios", Description = "Lista todos os usuarios ativos")]
    [ProducesResponseType(typeof(List<UsuarioResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var usuarios = await _usuarioUseCase.GetAllUsuariosAsync();
        return Ok(usuarios);
    }


    [HttpPatch("{id}/email")]
    [SwaggerOperation(Summary = "Alterar email de um usuário", Description = "Infome o ID do usuário e altere o email")]
    [ProducesResponseType(typeof(UsuarioResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AlterarEmail(int id, [FromBody] AlterarEmailDto dto)
    {
        try
        {
            var usuario = await _usuarioUseCase.AlterarEmailUsuarioAsync(id, dto);
            return Ok(usuario);
        }
        catch (InvalidOperationException ex)
        {
            if (ex.Message.Contains("não encontrado"))
                return NotFound(new { message = ex.Message });
            
            return BadRequest(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }


    [HttpPatch("{id}/senha")]
    [SwaggerOperation(Summary = "Alterar a senha de um usuário", Description = "Infome o ID do usuário e altere a senha")]
    [ProducesResponseType(typeof(UsuarioResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AlterarSenha(int id, [FromBody] AlterarSenhaDto dto)
    {
        try
        {
            var usuario = await _usuarioUseCase.AlterarSenhaUsuarioAsync(id, dto);
            return Ok(usuario);
        }
        catch (InvalidOperationException ex)
        {
            if (ex.Message.Contains("não encontrado"))
                return NotFound(new { message = ex.Message });
            
            return BadRequest(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }


    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Remover um usuario", Description = "Remoção lógica de um usuário - Infome o ID do usuario")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _usuarioUseCase.DeleteUsuarioAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}