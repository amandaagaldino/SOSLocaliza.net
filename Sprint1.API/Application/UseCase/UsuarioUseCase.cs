using Sprint1.Domain.Entities;
using Sprint1.Domain.Repositories;
using Sprint1.DTOs;
using Sprint1.DTOs.Usuario;
using Microsoft.Extensions.Logging;
using Sprint1.Domain.Exceptions;

namespace Sprint1.Infrastructure.Data.UseCase;

public class UsuarioUseCase : IUsuarioUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ILogger<UsuarioUseCase> _logger;

    public UsuarioUseCase(IUsuarioRepository usuarioRepository, ILogger<UsuarioUseCase> logger)
    {
        _usuarioRepository = usuarioRepository;
        _logger = logger;
    }

    public async Task<UsuarioResponseDto> CreateUsuarioAsync(CreateUsuarioDto dto)
    {
        _logger.LogInformation("Attempting to create user with email: {Email}", dto.Email);
        
        // Verificar se email ja existe
        if (await _usuarioRepository.EmailExistsAsync(dto.Email))
        {
            _logger.LogWarning("Failed to create user. Email already exists: {Email}", dto.Email);
            throw new EmailDuplicadoException(dto.Email);
        }

        // Verificar se cpf ja existe
        if (await _usuarioRepository.CpfExistsAsync(dto.Cpf))
        {
            _logger.LogWarning("Failed to create user. CPF already exists: {Cpf}", dto.Cpf);
            throw new CpfDuplicadoException(dto.Cpf);
        }

        var usuario = new Domain.Entities.Usuario(
            dto.NomeCompleto,
            dto.Email,
            dto.Senha,
            dto.DataNascimento,
            dto.Cpf
        );

        var usuarioCriado = await _usuarioRepository.AddAsync(usuario);
        
        _logger.LogInformation("User created successfully with ID: {UserId}", usuarioCriado.Id);

        return new UsuarioResponseDto
        {
            Id = usuarioCriado.Id,
            NomeCompleto = usuarioCriado.NomeCompleto,
            Email = usuarioCriado.Email,
            DataNascimento = usuarioCriado.DataNascimento,
            Cpf = usuarioCriado.Cpf,
            DataCriacao = usuarioCriado.DataCriacao,
            DataAtualizacao = usuarioCriado.DataAtualizacao,
            Ativo = usuarioCriado.Ativo
        };
    }

    public async Task<UsuarioResponseDto?> GetUsuarioByIdAsync(int id)
    {
        _logger.LogInformation("Fetching user with ID: {UserId}", id);
        
        var usuario = await _usuarioRepository.GetByIdAsync(id);
        
        if (usuario == null)
        {
            _logger.LogWarning("User not found with ID: {UserId}", id);
            throw new UsuarioNotFoundException(id);
        }

        _logger.LogInformation("User found with ID: {UserId}", id);
        
        return new UsuarioResponseDto
        {
            Id = usuario.Id,
            NomeCompleto = usuario.NomeCompleto,
            Email = usuario.Email,
            DataNascimento = usuario.DataNascimento,
            Cpf = usuario.Cpf,
            DataCriacao = usuario.DataCriacao,
            DataAtualizacao = usuario.DataAtualizacao,
            Ativo = usuario.Ativo
        };
    }

    public async Task<List<UsuarioResponseDto>> GetAllUsuariosAsync()
    {
        var usuarios = await _usuarioRepository.GetAllAsync();

        return usuarios.Select(usuario => new UsuarioResponseDto
        {
            Id = usuario.Id,
            NomeCompleto = usuario.NomeCompleto,
            Email = usuario.Email,
            DataNascimento = usuario.DataNascimento,
            Cpf = usuario.Cpf,
            DataCriacao = usuario.DataCriacao,
            DataAtualizacao = usuario.DataAtualizacao,
            Ativo = usuario.Ativo
        }).ToList();
    }

    public async Task<PagedResult<UsuarioResponseDto>> GetUsuariosPagedAsync(UsuarioQueryParameters parameters)
    {
        _logger.LogInformation("Fetching paged users with parameters: Page={Page}, PageSize={PageSize}, SortBy={SortBy}",
            parameters.Page, parameters.PageSize, parameters.SortBy);

        var (usuarios, totalCount) = await _usuarioRepository.GetPagedAsync(parameters);

        var usuarioDtos = usuarios.Select(usuario => new UsuarioResponseDto
        {
            Id = usuario.Id,
            NomeCompleto = usuario.NomeCompleto,
            Email = usuario.Email,
            DataNascimento = usuario.DataNascimento,
            Cpf = usuario.Cpf,
            DataCriacao = usuario.DataCriacao,
            DataAtualizacao = usuario.DataAtualizacao,
            Ativo = usuario.Ativo
        }).ToList();

        return new PagedResult<UsuarioResponseDto>(usuarioDtos, totalCount, parameters.Page, parameters.PageSize);
    }

    public async Task<UsuarioResponseDto> AlterarEmailUsuarioAsync(int id, AlterarEmailDto dto)
    {
        _logger.LogInformation("Attempting to change email for user ID: {UserId} to {NewEmail}", id, dto.Email);
        
        var usuario = await _usuarioRepository.GetByIdAsync(id);
        
        if (usuario == null)
        {
            _logger.LogWarning("Failed to change email. User not found with ID: {UserId}", id);
            throw new UsuarioNotFoundException(id);
        }

        // Verificar se email ja existe em outro usuario
        var usuarioComEmail = await _usuarioRepository.GetByEmailAsync(dto.Email);
        if (usuarioComEmail != null && usuarioComEmail.Id != id)
        {
            _logger.LogWarning("Failed to change email. Email already in use: {Email}", dto.Email);
            throw new EmailDuplicadoException(dto.Email);
        }

        usuario.AlterarEmail(dto.Email);

        var usuarioAtualizado = await _usuarioRepository.UpdateAsync(usuario);
        
        _logger.LogInformation("Email changed successfully for user ID: {UserId}", id);

        return new UsuarioResponseDto
        {
            Id = usuarioAtualizado.Id,
            NomeCompleto = usuarioAtualizado.NomeCompleto,
            Email = usuarioAtualizado.Email,
            DataNascimento = usuarioAtualizado.DataNascimento,
            Cpf = usuarioAtualizado.Cpf,
            DataCriacao = usuarioAtualizado.DataCriacao,
            DataAtualizacao = usuarioAtualizado.DataAtualizacao,
            Ativo = usuarioAtualizado.Ativo
        };
    }

    public async Task<UsuarioResponseDto> AlterarSenhaUsuarioAsync(int id, AlterarSenhaDto dto)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(id);
        
        if (usuario == null)
            throw new UsuarioNotFoundException(id);

        // Verificar senha atual
        if (usuario.Senha != dto.SenhaAtual)
            throw new InvalidOperationException("Senha atual incorreta");

        usuario.AlterarSenha(dto.NovaSenha);

        var usuarioAtualizado = await _usuarioRepository.UpdateAsync(usuario);

        return new UsuarioResponseDto
        {
            Id = usuarioAtualizado.Id,
            NomeCompleto = usuarioAtualizado.NomeCompleto,
            Email = usuarioAtualizado.Email,
            DataNascimento = usuarioAtualizado.DataNascimento,
            Cpf = usuarioAtualizado.Cpf,
            DataCriacao = usuarioAtualizado.DataCriacao,
            DataAtualizacao = usuarioAtualizado.DataAtualizacao,
            Ativo = usuarioAtualizado.Ativo
        };
    }

    public async Task DeleteUsuarioAsync(int id)
    {
        _logger.LogInformation("Attempting to delete user with ID: {UserId}", id);
        
        var usuario = await _usuarioRepository.GetByIdAsync(id);
        
        if (usuario == null)
        {
            _logger.LogWarning("Failed to delete user. User not found with ID: {UserId}", id);
            throw new UsuarioNotFoundException(id);
        }

        await _usuarioRepository.DeleteAsync(usuario);
        
        _logger.LogInformation("User deleted successfully with ID: {UserId}", id);
    }
}