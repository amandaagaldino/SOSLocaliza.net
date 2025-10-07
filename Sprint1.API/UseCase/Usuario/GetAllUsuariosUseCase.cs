using Sprint1.Domain.Repositories;
using Sprint1.DTOs.Usuario;

namespace Sprint1.UseCase.Usuario;

public class GetAllUsuariosUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;

    public GetAllUsuariosUseCase(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<List<UsuarioResponseDto>> ExecuteAsync()
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
}
