namespace Sprint1.Domain.Exceptions;

public class CpfDuplicadoException : Exception
{
    public string Cpf { get; }

    public CpfDuplicadoException(string cpf) 
        : base($"O CPF '{cpf}' já está em uso por outro usuário")
    {
        Cpf = cpf;
    }
}

