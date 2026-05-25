namespace Sprint1.Domain.Exceptions;

public class EmailDuplicadoException : Exception
{
    public string Email { get; }

    public EmailDuplicadoException(string email) 
        : base($"O email '{email}' já está em uso por outro usuário")
    {
        Email = email;
    }
}

