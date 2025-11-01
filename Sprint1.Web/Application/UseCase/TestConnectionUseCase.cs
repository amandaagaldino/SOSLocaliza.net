using Microsoft.EntityFrameworkCore;
using Sprint1.DTOs;
using Sprint1.Infrastructure.Data;

namespace Sprint1.Infrastructure.Data.UseCase;

public class TestConnectionUseCase
{
    private readonly ApplicationDbContext _context;

    public TestConnectionUseCase(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TestConnectionDto> ExecuteAsync()
    {
        try
        {
            var connection = _context.Database.GetDbConnection();
            string? databaseName = null;
            int count = 0;

            try
            {
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                databaseName = connection.Database;
                
                count = await _context.Usuarios.CountAsync();
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    await connection.CloseAsync();
                }
            }
            
            return new TestConnectionDto
            {
                Success = true,
                Message = "Conexão com Oracle Database estabelecida com sucesso!",
                Database = databaseName ?? "N/A",
                TotalUsuarios = count,
                Servidor = "Oracle Database - São Paulo",
                Timestamp = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            var connection = _context.Database.GetDbConnection();
            if (connection.State == System.Data.ConnectionState.Open)
            {
                await connection.CloseAsync();
            }

            return new TestConnectionDto
            {
                Success = false,
                Message = "Erro ao conectar com o banco de dados",
                Error = ex.Message,
                InnerError = ex.InnerException?.Message,
                StackTrace = ex.StackTrace,
                ConnectionState = connection.State.ToString(),
                Timestamp = DateTime.UtcNow
            };
        }
    }
}

