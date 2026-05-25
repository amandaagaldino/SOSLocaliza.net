namespace Sprint1.DTOs.Auth;

public class TokenResponseDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
    public string TokenType { get; set; } = "Bearer";
    public int UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}

// Made with Bob
