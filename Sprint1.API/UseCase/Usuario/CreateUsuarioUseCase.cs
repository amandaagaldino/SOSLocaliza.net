using Sprint1.Domain.Entities;
using Sprint1.Domain.Repositories;
using Sprint1.DTOs.Usuario;

namespace Sprint1.UseCase.Usuario;

public class CreateUsuarioUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;

    public CreateUsuarioUseCase(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<UsuarioResponseDto> ExecuteAsync(CreateUsuarioDto dto)
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
}
