namespace Sprint1.Models.ViewModels;

/// <summary>
/// ViewModel para listagem de usuários
/// </summary>
public class UsuarioListViewModel
{
    public int Id { get; set; }
    public string NomeCompleto { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCriacao { get; set; }
}

