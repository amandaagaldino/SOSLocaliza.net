using Sprint1.Domain.Entities;
using Sprint1.DTOs.Usuario;

namespace Sprint1.Domain.Repositories;

public interface IUsuarioRepository
{
    Task<Usuario> AddAsync(Usuario usuario);
    Task<Usuario?> GetByIdAsync(int id);
    Task<List<Usuario>> GetAllAsync();
    Task<(List<Usuario> usuarios, int totalCount)> GetPagedAsync(UsuarioQueryParameters parameters);
    Task<Usuario?> GetByEmailAsync(string email);
    Task<Usuario?> GetByCpfAsync(string cpf);
    Task<Usuario> UpdateAsync(Usuario usuario);
    Task DeleteAsync(Usuario usuario);
    Task<bool> ExistsAsync(int id);
    Task<bool> EmailExistsAsync(string email);
    Task<bool> CpfExistsAsync(string cpf);
}