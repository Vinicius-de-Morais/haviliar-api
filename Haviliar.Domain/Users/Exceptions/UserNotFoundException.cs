namespace Haviliar.Domain.Users.Exceptions;

public class ClientNotFoundException : Exception
{
    public ClientNotFoundException() : base("O usuário não foi encontrado.")
    {

    }
}
