using Microsoft.AspNetCore.Mvc;
using Sprint1.DTOs;
using Sprint1.DTOs.Usuario;
using Sprint1.Infrastructure.Data.UseCase;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Http;
using Sprint1.Utils;
using Microsoft.AspNetCore.Authorization;

namespace Sprint1.Controllers;

[ApiController]
[Route("api/[controller]")]
[SwaggerTag("SOSLocaliza - EndPoint em relação a criação de usuario (CRUD)")]
[Authorize] // Todos os endpoints requerem autenticação
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioUseCase _usuarioUseCase;
    private readonly TestConnectionUseCase _testConnectionUseCase;

    public UsuarioController(
        IUsuarioUseCase usuarioUseCase,
        TestConnectionUseCase testConnectionUseCase)
    {
        _usuarioUseCase = usuarioUseCase;
        _testConnectionUseCase = testConnectionUseCase;
    }

    [HttpGet("test-connection")]
    [SwaggerOperation(Summary = "Testar conexão com o banco de dados")]
    [ProducesResponseType(typeof(TestConnectionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TestConnectionDto), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> TestConnection()
    {
        var result = await _testConnectionUseCase.ExecuteAsync();
        
        if (result.Success)
        {
            return Ok(result);
        }
        else
        {
            return StatusCode(500, result);
        }
    }


    [HttpPost]
    [AllowAnonymous] // Permitir criação de usuário sem autenticação
    [SwaggerOperation(Summary = "Criar novo usuário", Description = "Infome o nome completo, email, data nascimento e CPF do usuário")]
    [ProducesResponseType(typeof(UsuarioResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateUsuarioDto dto)
    {
        var usuario = await _usuarioUseCase.CreateUsuarioAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, usuario);
    }
    
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Listar usuário por ID com HATEOAS", Description = "Infome o ID do usuário e visualize suas informações com links de navegação")]
    [ProducesResponseType(typeof(UsuarioResourceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var usuario = await _usuarioUseCase.GetUsuarioByIdAsync(id);
        var resource = usuario.AddLinks(HttpContext);
        return Ok(resource);
    }
    
    [HttpGet]
    [SwaggerOperation(
        Summary = "Listar usuarios com paginação, ordenação, filtros e HATEOAS",
        Description = "Lista usuarios com suporte a paginação, ordenação, filtros por nome, email, CPF e status, incluindo links HATEOAS")]
    [ProducesResponseType(typeof(PagedResult<UsuarioResourceDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] UsuarioQueryParameters parameters)
    {
        var result = await _usuarioUseCase.GetUsuariosPagedAsync(parameters);
        var resourceResult = result.AddLinksToPagedResult(HttpContext, parameters);
        
        // Adicionar links de paginação ao response header
        var paginationLinks = HateoasHelper.GeneratePaginationLinks(HttpContext, parameters, result.TotalPages);
        Response.Headers["X-Pagination-Links"] = System.Text.Json.JsonSerializer.Serialize(paginationLinks);
        
        return Ok(resourceResult);
    }

    [HttpGet("all")]
    [SwaggerOperation(Summary = "Listar todos os usuarios", Description = "Lista todos os usuarios ativos sem paginação")]
    [ProducesResponseType(typeof(List<UsuarioResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllWithoutPagination()
    {
        var usuarios = await _usuarioUseCase.GetAllUsuariosAsync();
        return Ok(usuarios);
    }


    [HttpPatch("{id}/email")]
    [SwaggerOperation(Summary = "Alterar email de um usuário com HATEOAS", Description = "Infome o ID do usuário e altere o email")]
    [ProducesResponseType(typeof(UsuarioResourceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AlterarEmail(int id, [FromBody] AlterarEmailDto dto)
    {
        var usuario = await _usuarioUseCase.AlterarEmailUsuarioAsync(id, dto);
        var resource = usuario.AddLinks(HttpContext);
        return Ok(resource);
    }


    [HttpPatch("{id}/senha")]
    [SwaggerOperation(Summary = "Alterar a senha de um usuário com HATEOAS", Description = "Infome o ID do usuário e altere a senha")]
    [ProducesResponseType(typeof(UsuarioResourceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AlterarSenha(int id, [FromBody] AlterarSenhaDto dto)
    {
        var usuario = await _usuarioUseCase.AlterarSenhaUsuarioAsync(id, dto);
        var resource = usuario.AddLinks(HttpContext);
        return Ok(resource);
    }


    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")] // Apenas Admin pode deletar
    [SwaggerOperation(Summary = "Remover um usuario (Admin)", Description = "Remoção lógica de um usuário - Apenas Admin - Infome o ID do usuario")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete(int id)
    {
        await _usuarioUseCase.DeleteUsuarioAsync(id);
        return NoContent();
    }
}