namespace Sprint1.Domain.Exceptions;

public class UsuarioNotFoundException : Exception
{
    public UsuarioNotFoundException(int id) 
        : base($"Usuário com ID {id} não foi encontrado")
    {
    }

    public UsuarioNotFoundException(string message) 
        : base(message)
    {
    }
}

