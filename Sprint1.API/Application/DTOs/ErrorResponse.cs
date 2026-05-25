namespace Sprint1.DTOs;

public class ErrorResponse
{
    public string Type { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public string? StackTrace { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? Path { get; set; }

    public ErrorResponse()
    {
    }

    public ErrorResponse(string type, string message, int statusCode, string? path = null)
    {
        Type = type;
        Message = message;
        StatusCode = statusCode;
        Path = path;
    }
}

