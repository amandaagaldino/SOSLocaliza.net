namespace Sprint1.DTOs;

public class TestConnectionDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Database { get; set; } = string.Empty;
    public int TotalUsuarios { get; set; }
    public string Servidor { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string? Error { get; set; }
    public string? InnerError { get; set; }
    public string? StackTrace { get; set; }
    public string? ConnectionState { get; set; }
}

