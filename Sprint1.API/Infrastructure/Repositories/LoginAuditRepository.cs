using MongoDB.Driver;
using Sprint1.Domain.Entities;

namespace Sprint1.Infrastructure.Repositories;

/// <summary>
/// Repositório para auditoria de login usando MongoDB
/// </summary>
public class LoginAuditRepository : ILoginAuditRepository
{
    private readonly IMongoCollection<LoginAudit> _loginAudits;

    public LoginAuditRepository(IMongoDatabase database)
    {
        _loginAudits = database.GetCollection<LoginAudit>("LoginAudits");
        
        // Criar índices para melhor performance
        CreateIndexes();
    }

    private void CreateIndexes()
    {
        var emailIndexModel = new CreateIndexModel<LoginAudit>(
            Builders<LoginAudit>.IndexKeys.Ascending(x => x.Email));
        
        var timestampIndexModel = new CreateIndexModel<LoginAudit>(
            Builders<LoginAudit>.IndexKeys.Descending(x => x.Timestamp));
        
        var successIndexModel = new CreateIndexModel<LoginAudit>(
            Builders<LoginAudit>.IndexKeys.Ascending(x => x.Success));

        _loginAudits.Indexes.CreateMany(new[] 
        { 
            emailIndexModel, 
            timestampIndexModel, 
            successIndexModel 
        });
    }

    public async Task LogLoginAttemptAsync(LoginAudit audit)
    {
        audit.Timestamp = DateTime.UtcNow;
        await _loginAudits.InsertOneAsync(audit);
    }

    public async Task<List<LoginAudit>> GetLoginAttemptsByEmailAsync(string email, int limit = 10)
    {
        return await _loginAudits
            .Find(x => x.Email == email)
            .SortByDescending(x => x.Timestamp)
            .Limit(limit)
            .ToListAsync();
    }

    public async Task<List<LoginAudit>> GetRecentLoginAttemptsAsync(int limit = 50)
    {
        return await _loginAudits
            .Find(_ => true)
            .SortByDescending(x => x.Timestamp)
            .Limit(limit)
            .ToListAsync();
    }

    public async Task<List<LoginAudit>> GetFailedLoginAttemptsAsync(int limit = 50)
    {
        return await _loginAudits
            .Find(x => x.Success == false)
            .SortByDescending(x => x.Timestamp)
            .Limit(limit)
            .ToListAsync();
    }
}

