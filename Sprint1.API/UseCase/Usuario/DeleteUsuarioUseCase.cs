using Sprint1.Domain.Repositories;

namespace Sprint1.UseCase.Usuario;

public class DeleteUsuarioUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;

    public DeleteUsuarioUseCase(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task ExecuteAsync(int id)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(id);
        
        if (usuario == null)
            throw new InvalidOperationException("Usuário não encontrado");

        await _usuarioRepository.DeleteAsync(usuario);
    }
}
