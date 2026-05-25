using Sprint1.Domain.Entities;

namespace Sprint1.Application.Services;

public interface ITokenService
{
    string GenerateToken(Usuario usuario);
}

// Made with Bob
