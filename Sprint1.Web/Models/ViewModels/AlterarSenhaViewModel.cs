using System.ComponentModel.DataAnnotations;

namespace Sprint1.Models.ViewModels;

/// <summary>
/// ViewModel para alteração de senha
/// </summary>
public class AlterarSenhaViewModel
{
    [Required(ErrorMessage = "A senha atual é obrigatória")]
    [DataType(DataType.Password)]
    [Display(Name = "Senha Atual")]
    public string SenhaAtual { get; set; } = string.Empty;

    [Required(ErrorMessage = "A nova senha é obrigatória")]
    [StringLength(50, MinimumLength = 6, ErrorMessage = "A nova senha deve ter entre 6 e 50 caracteres")]
    [DataType(DataType.Password)]
    [Display(Name = "Nova Senha")]
    public string NovaSenha { get; set; } = string.Empty;

    [Required(ErrorMessage = "A confirmação da senha é obrigatória")]
    [DataType(DataType.Password)]
    [Compare("NovaSenha", ErrorMessage = "A confirmação da senha não confere com a nova senha")]
    [Display(Name = "Confirmar Nova Senha")]
    public string ConfirmarNovaSenha { get; set; } = string.Empty;
}

