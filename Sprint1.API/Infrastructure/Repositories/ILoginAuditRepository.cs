using Sprint1.Domain.Entities;

namespace Sprint1.Infrastructure.Repositories;

/// <summary>
/// Interface para repositório de auditoria de login no MongoDB
/// </summary>
public interface ILoginAuditRepository
{
    /// <summary>
    /// Registra uma tentativa de login (sucesso ou falha)
    /// </summary>
    Task LogLoginAttemptAsync(LoginAudit audit);

    /// <summary>
    /// Busca tentativas de login por email
    /// </summary>
    Task<List<LoginAudit>> GetLoginAttemptsByEmailAsync(string email, int limit = 10);

    /// <summary>
    /// Busca tentativas de login recentes
    /// </summary>
    Task<List<LoginAudit>> GetRecentLoginAttemptsAsync(int limit = 50);

    /// <summary>
    /// Busca tentativas de login falhadas
    /// </summary>
    Task<List<LoginAudit>> GetFailedLoginAttemptsAsync(int limit = 50);
}

