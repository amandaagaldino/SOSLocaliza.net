using Microsoft.EntityFrameworkCore;
using Sprint1.Domain.Entities;
using Sprint1.Infrastructure.Data;

namespace Sprint1.Domain.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly ApplicationDbContext _context;

    public UsuarioRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Usuario> AddAsync(Usuario usuario)
    {
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }

    public async Task<List<Usuario>> GetAllAsync()
    {
        return await _context.Usuarios
            .Where(u => u.Ativo == true)
            .OrderBy(u => u.NomeCompleto)
            .ToListAsync();
    }

    public async Task<Usuario?> GetByIdAsync(int id)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Id == id && u.Ativo == true);
    }

    public async Task<Usuario?> GetByEmailAsync(string email)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == email && u.Ativo == true);
    }

    public async Task<Usuario?> GetByCpfAsync(string cpf)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Cpf == cpf && u.Ativo == true);
    }

    public async Task<Usuario> UpdateAsync(Usuario usuario)
    {
        _context.Usuarios.Update(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }

    public async Task DeleteAsync(Usuario usuario)
    {
        usuario.Desativar();
        _context.Usuarios.Update(usuario);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Usuarios
            .AnyAsync(u => u.Id == id && u.Ativo == true);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        var count = await _context.Usuarios.CountAsync(u => u.Email == email && u.Ativo == true);
        return count > 0;
    }

    public async Task<bool> CpfExistsAsync(string cpf)
    {
        var count = await _context.Usuarios.CountAsync(u => u.Cpf == cpf && u.Ativo == true);
        return count > 0;
    }
}

