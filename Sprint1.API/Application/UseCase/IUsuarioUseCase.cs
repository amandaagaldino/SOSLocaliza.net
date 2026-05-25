using Sprint1.DTOs;
using Sprint1.DTOs.Usuario;

namespace Sprint1.Infrastructure.Data.UseCase;

public interface IUsuarioUseCase
{
    Task<UsuarioResponseDto> CreateUsuarioAsync(CreateUsuarioDto dto);
    Task<UsuarioResponseDto?> GetUsuarioByIdAsync(int id);
    Task<List<UsuarioResponseDto>> GetAllUsuariosAsync();
    Task<PagedResult<UsuarioResponseDto>> GetUsuariosPagedAsync(UsuarioQueryParameters parameters);
    Task<UsuarioResponseDto> AlterarEmailUsuarioAsync(int id, AlterarEmailDto dto);
    Task<UsuarioResponseDto> AlterarSenhaUsuarioAsync(int id, AlterarSenhaDto dto);
    Task DeleteUsuarioAsync(int id);
}