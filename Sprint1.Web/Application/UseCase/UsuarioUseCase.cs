using Sprint1.Domain.Entities;
using Sprint1.Domain.Repositories;
using Sprint1.DTOs.Usuario;

namespace Sprint1.Infrastructure.Data.UseCase;

public class UsuarioUseCase : IUsuarioUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioUseCase(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<UsuarioResponseDto> CreateUsuarioAsync(CreateUsuarioDto dto)
    {
        // Verificar se email ja existe
        if (await _usuarioRepository.EmailExistsAsync(dto.Email))
        {
            throw new InvalidOperationException("Email já está em uso");
        }

        // Verificar se cpf ja existe
        if (await _usuarioRepository.CpfExistsAsync(dto.Cpf))
        {
            throw new InvalidOperationException("CPF já está em uso");
        }

        var usuario = new Domain.Entities.Usuario(
            dto.NomeCompleto,
            dto.Email,
            dto.Senha,
            dto.DataNascimento,
            dto.Cpf
        );

        var usuarioCriado = await _usuarioRepository.AddAsync(usuario);

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
        var usuario = await _usuarioRepository.GetByIdAsync(id);
        
        if (usuario == null)
            return null;

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

    public async Task<UsuarioResponseDto> AlterarEmailUsuarioAsync(int id, AlterarEmailDto dto)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(id);
        
        if (usuario == null)
            throw new InvalidOperationException("Usuário não encontrado");

        // Verificar se email ja existe em outro usuario
        var usuarioComEmail = await _usuarioRepository.GetByEmailAsync(dto.Email);
        if (usuarioComEmail != null && usuarioComEmail.Id != id)
        {
            throw new InvalidOperationException("Email já está em uso por outro usuário");
        }

        usuario.AlterarEmail(dto.Email);

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

    public async Task<UsuarioResponseDto> AlterarSenhaUsuarioAsync(int id, AlterarSenhaDto dto)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(id);
        
        if (usuario == null)
            throw new InvalidOperationException("Usuário não encontrado");

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
        var usuario = await _usuarioRepository.GetByIdAsync(id);
        
        if (usuario == null)
            throw new InvalidOperationException("Usuário não encontrado");

        await _usuarioRepository.DeleteAsync(usuario);
    }
}

