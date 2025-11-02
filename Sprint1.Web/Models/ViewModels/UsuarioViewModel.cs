using System.ComponentModel.DataAnnotations;

namespace Sprint1.Models.ViewModels;

/// <summary>
/// ViewModel para criação de usuário na interface web
/// </summary>
public class UsuarioViewModel
{
    [Required(ErrorMessage = "Nome completo é obrigatório")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Nome completo deve ter entre 2 e 100 caracteres")]
    [Display(Name = "Nome Completo")]
    public string NomeCompleto { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    [StringLength(100, ErrorMessage = "Email deve ter no máximo 100 caracteres")]
    [Display(Name = "E-mail")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Senha é obrigatória")]
    [StringLength(50, MinimumLength = 6, ErrorMessage = "Senha deve ter entre 6 e 50 caracteres")]
    [DataType(DataType.Password)]
    [Display(Name = "Senha")]
    public string Senha { get; set; } = string.Empty;

    [Required(ErrorMessage = "Data de nascimento é obrigatória")]
    [DataType(DataType.Date)]
    [Display(Name = "Data de Nascimento")]
    public DateTime DataNascimento { get; set; } = DateTime.Now.AddYears(-18);

    [Required(ErrorMessage = "CPF é obrigatório")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "CPF deve ter 11 dígitos")]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "CPF deve conter apenas números")]
    [Display(Name = "CPF")]
    public string Cpf { get; set; } = string.Empty;
}

