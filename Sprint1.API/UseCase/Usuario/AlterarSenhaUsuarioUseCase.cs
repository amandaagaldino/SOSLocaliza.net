using Sprint1.Domain.Repositories;
using Sprint1.DTOs.Usuario;

namespace Sprint1.UseCase.Usuario;

public class AlterarSenhaUsuarioUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;

    public AlterarSenhaUsuarioUseCase(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<UsuarioResponseDto> ExecuteAsync(int id, AlterarSenhaDto dto)
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
}
