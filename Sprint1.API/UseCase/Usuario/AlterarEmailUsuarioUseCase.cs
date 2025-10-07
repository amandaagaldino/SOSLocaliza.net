using Sprint1.Domain.Repositories;
using Sprint1.DTOs.Usuario;

namespace Sprint1.UseCase.Usuario;

public class AlterarEmailUsuarioUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;

    public AlterarEmailUsuarioUseCase(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<UsuarioResponseDto> ExecuteAsync(int id, AlterarEmailDto dto)
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
}
