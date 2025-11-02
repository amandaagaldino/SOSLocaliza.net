using System.ComponentModel.DataAnnotations;

namespace Sprint1.Models.ViewModels;

/// <summary>
/// ViewModel para alteração de email
/// </summary>
public class AlterarEmailViewModel
{
    [Required(ErrorMessage = "O novo email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    [StringLength(100, ErrorMessage = "Email deve ter no máximo 100 caracteres")]
    [Display(Name = "Novo E-mail")]
    public string Email { get; set; } = string.Empty;
}

