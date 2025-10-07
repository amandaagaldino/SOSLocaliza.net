using Sprint1.Domain.Repositories;
using Sprint1.DTOs.Usuario;

namespace Sprint1.UseCase.Usuario;

public class GetUsuarioByIdUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;

    public GetUsuarioByIdUseCase(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<UsuarioResponseDto?> ExecuteAsync(int id)
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
}
