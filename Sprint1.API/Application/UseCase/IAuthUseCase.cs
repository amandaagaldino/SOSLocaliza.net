using Sprint1.DTOs.Auth;

namespace Sprint1.Application.UseCase;

public interface IAuthUseCase
{
    Task<TokenResponseDto> LoginAsync(LoginDto loginDto);
}

