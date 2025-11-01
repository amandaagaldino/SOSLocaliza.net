using System.ComponentModel.DataAnnotations;

namespace Sprint1.DTOs.Usuario;

public class AlterarSenhaDto
{
    [Required(ErrorMessage = "Senha atual é obrigatoria")]
    public string SenhaAtual { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nova senha é obrigatoria")]
    [StringLength(30, MinimumLength = 6, ErrorMessage = "Nova senha deve ter entre 6 e 30 caracteres")]
    public string NovaSenha { get; set; } = string.Empty;
}

