namespace Sprint1.Models.ViewModels;

/// <summary>
/// ViewModel para teste de conexão com banco de dados
/// </summary>
public class TestConnectionViewModel
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Database { get; set; }
    public int TotalUsuarios { get; set; }
    public string? Servidor { get; set; }
    public DateTime Timestamp { get; set; }
    public string? Error { get; set; }
    public string? InnerError { get; set; }
    public string? StackTrace { get; set; }
    public string? ConnectionState { get; set; }
}

