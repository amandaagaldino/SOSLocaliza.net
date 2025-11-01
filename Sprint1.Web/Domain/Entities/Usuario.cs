using System.ComponentModel.DataAnnotations;

namespace Sprint1.Domain.Entities;

public class Usuario
{
    public int Id { get; private set; }
    public string NomeCompleto { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Senha { get; private set; } = string.Empty;
    public DateTime DataNascimento { get; private set; }
    public string Cpf { get; private set; } = string.Empty;
    public DateTime DataCriacao { get; private set; }
    public DateTime? DataAtualizacao { get; private set; }
    public bool Ativo { get; private set; }

   
    private Usuario() { }

    
    public Usuario(string nomeCompleto, string email, string senha, DateTime dataNascimento, string cpf)
    {
        // Id será gerado automaticamente pelo banco de dados
        NomeCompleto = nomeCompleto;
        Email = email;
        Senha = senha;
        DataNascimento = dataNascimento;
        Cpf = cpf;
        DataCriacao = DateTime.UtcNow;
        Ativo = true;
    }

    // Métodos para alterar 
    public void AlterarNome(string nomeCompleto)
    {
        if (string.IsNullOrWhiteSpace(nomeCompleto))
            throw new ArgumentException("Nome completo não pode ser vazio", nameof(nomeCompleto));
        
        NomeCompleto = nomeCompleto;
        DataAtualizacao = DateTime.UtcNow;
    }

    public void AlterarEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email não pode ser vazio", nameof(email));
        
        if (!IsValidEmail(email))
            throw new ArgumentException("Email inválido", nameof(email));
        
        Email = email;
        DataAtualizacao = DateTime.UtcNow;
    }

    public void AlterarSenha(string senha)
    {
        if (string.IsNullOrWhiteSpace(senha))
            throw new ArgumentException("Senha não pode ser vazia", nameof(senha));
        
        if (senha.Length < 6)
            throw new ArgumentException("Senha deve ter pelo menos 6 caracteres", nameof(senha));
        
        Senha = senha;
        DataAtualizacao = DateTime.UtcNow;
    }

    public void Desativar()
    {
        Ativo = false;
        DataAtualizacao = DateTime.UtcNow;
    }

    public void Ativar()
    {
        Ativo = true;
        DataAtualizacao = DateTime.UtcNow;
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}

