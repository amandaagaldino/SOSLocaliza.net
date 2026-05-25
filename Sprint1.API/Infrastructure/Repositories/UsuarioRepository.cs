using Microsoft.EntityFrameworkCore;
using Sprint1.Domain.Entities;
using Sprint1.Infrastructure.Data;
using Sprint1.DTOs.Usuario;
using System.Linq.Expressions;

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

    public async Task<(List<Usuario> usuarios, int totalCount)> GetPagedAsync(UsuarioQueryParameters parameters)
    {
        var query = _context.Usuarios.AsQueryable();

        // Aplicar filtros
        if (parameters.Ativo.HasValue)
        {
            query = query.Where(u => u.Ativo == parameters.Ativo.Value);
        }
        else
        {
            query = query.Where(u => u.Ativo == true); // Por padrão, apenas ativos
        }

        if (!string.IsNullOrWhiteSpace(parameters.Nome))
        {
            query = query.Where(u => u.NomeCompleto.Contains(parameters.Nome));
        }

        if (!string.IsNullOrWhiteSpace(parameters.Email))
        {
            query = query.Where(u => u.Email.Contains(parameters.Email));
        }

        if (!string.IsNullOrWhiteSpace(parameters.Cpf))
        {
            query = query.Where(u => u.Cpf.Contains(parameters.Cpf));
        }

        // Contar total antes da paginação
        var totalCount = await query.CountAsync();

        // Aplicar ordenação
        if (!string.IsNullOrWhiteSpace(parameters.SortBy))
        {
            query = ApplyOrdering(query, parameters.SortBy, parameters.SortOrder);
        }
        else
        {
            query = query.OrderBy(u => u.NomeCompleto); // Ordenação padrão
        }

        // Aplicar paginação
        var usuarios = await query
            .Skip((parameters.Page - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync();

        return (usuarios, totalCount);
    }

    private IQueryable<Usuario> ApplyOrdering(IQueryable<Usuario> query, string sortBy, string sortOrder)
    {
        var isDescending = sortOrder.ToLower() == "desc";

        return sortBy.ToLower() switch
        {
            "nome" or "nomecompleto" => isDescending
                ? query.OrderByDescending(u => u.NomeCompleto)
                : query.OrderBy(u => u.NomeCompleto),
            "email" => isDescending
                ? query.OrderByDescending(u => u.Email)
                : query.OrderBy(u => u.Email),
            "cpf" => isDescending
                ? query.OrderByDescending(u => u.Cpf)
                : query.OrderBy(u => u.Cpf),
            "datacriacao" => isDescending
                ? query.OrderByDescending(u => u.DataCriacao)
                : query.OrderBy(u => u.DataCriacao),
            "datanascimento" => isDescending
                ? query.OrderByDescending(u => u.DataNascimento)
                : query.OrderBy(u => u.DataNascimento),
            _ => query.OrderBy(u => u.NomeCompleto) // Padrão
        };
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
