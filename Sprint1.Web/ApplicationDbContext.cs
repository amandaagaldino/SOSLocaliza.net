using Microsoft.EntityFrameworkCore;
using Sprint1.Domain.Entities;
using Sprint1.Mappings;

namespace Sprint1.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.ApplyConfiguration(new UsuarioMapping());
        
        base.OnModelCreating(modelBuilder);
    }
}

