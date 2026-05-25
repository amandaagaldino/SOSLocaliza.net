using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Sprint1.Application.Services;
using Sprint1.Domain.Entities;
using Sprint1.Domain.Repositories;
using Sprint1.DTOs.Auth;
using Sprint1.Infrastructure.Repositories;

namespace Sprint1.Application.UseCase;

public class AuthUseCase : IAuthUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ITokenService _tokenService;
    private readonly ILoginAuditRepository _loginAuditRepository;
    private readonly ILogger<AuthUseCase> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthUseCase(
        IUsuarioRepository usuarioRepository,
        ITokenService tokenService,
        ILoginAuditRepository loginAuditRepository,
        ILogger<AuthUseCase> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _usuarioRepository = usuarioRepository;
        _tokenService = tokenService;
        _loginAuditRepository = loginAuditRepository;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TokenResponseDto> LoginAsync(LoginDto loginDto)
    {
        _logger.LogInformation("Login attempt for email: {Email}", loginDto.Email);

        var httpContext = _httpContextAccessor.HttpContext;
        var ipAddress = httpContext?.Connection.RemoteIpAddress?.ToString();
        var userAgent = httpContext?.Request.Headers["User-Agent"].ToString();

        try
        {
            var usuario = await _usuarioRepository.GetByEmailAsync(loginDto.Email);

            if (usuario == null)
            {
                _logger.LogWarning("Login failed. User not found: {Email}", loginDto.Email);
                
                // Registrar tentativa falhada no MongoDB
                await _loginAuditRepository.LogLoginAttemptAsync(new LoginAudit
                {
                    Email = loginDto.Email,
                    IpAddress = ipAddress,
                    Success = false,
                    FailureReason = "User not found",
                    UserAgent = userAgent
                });

                throw new UnauthorizedAccessException("Email ou senha inválidos");
            }

            if (!usuario.Ativo)
            {
                _logger.LogWarning("Login failed. User inactive: {Email}", loginDto.Email);
                
                // Registrar tentativa falhada no MongoDB
                await _loginAuditRepository.LogLoginAttemptAsync(new LoginAudit
                {
                    Email = loginDto.Email,
                    IpAddress = ipAddress,
                    Success = false,
                    FailureReason = "User inactive",
                    UserId = usuario.Id,
                    UserAgent = userAgent
                });

                throw new UnauthorizedAccessException("Usuário inativo");
            }

            // Verificar senha (em produção, usar hash)
            if (usuario.Senha != loginDto.Senha)
            {
                _logger.LogWarning("Login failed. Invalid password for: {Email}", loginDto.Email);
                
                // Registrar tentativa falhada no MongoDB
                await _loginAuditRepository.LogLoginAttemptAsync(new LoginAudit
                {
                    Email = loginDto.Email,
                    IpAddress = ipAddress,
                    Success = false,
                    FailureReason = "Invalid password",
                    UserId = usuario.Id,
                    UserAgent = userAgent
                });

                throw new UnauthorizedAccessException("Email ou senha inválidos");
            }

            var token = _tokenService.GenerateToken(usuario);
            var expirationMinutes = 60; // Pode vir da configuração

            _logger.LogInformation("Login successful for user: {Email}", loginDto.Email);

            // Registrar login bem-sucedido no MongoDB
            await _loginAuditRepository.LogLoginAttemptAsync(new LoginAudit
            {
                Email = loginDto.Email,
                IpAddress = ipAddress,
                Success = true,
                UserId = usuario.Id,
                UserAgent = userAgent
            });

            return new TokenResponseDto
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddMinutes(expirationMinutes),
                TokenType = "Bearer",
                UserId = usuario.Id,
                Email = usuario.Email,
                Role = usuario.Role
            };
        }
        catch (UnauthorizedAccessException)
        {
            // Re-throw para manter o comportamento esperado
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during login for email: {Email}", loginDto.Email);
            
            // Registrar erro no MongoDB
            await _loginAuditRepository.LogLoginAttemptAsync(new LoginAudit
            {
                Email = loginDto.Email,
                IpAddress = ipAddress,
                Success = false,
                FailureReason = $"System error: {ex.Message}",
                UserAgent = userAgent
            });

            throw;
        }
    }
}
