namespace Sprint1.DTOs.Usuario;

public class UsuarioQueryParameters : QueryParameters
{
    public string? Nome { get; set; }
    public string? Email { get; set; }
    public string? Cpf { get; set; }
    public bool? Ativo { get; set; }
}

// Made with Bob
