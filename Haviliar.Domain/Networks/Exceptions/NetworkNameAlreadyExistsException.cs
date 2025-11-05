namespace Haviliar.Domain.Networks.Exceptions;

public class NetworkNameAlreadyExistsException : Exception
{
    public NetworkNameAlreadyExistsException() : base("O nome da rede já está em uso.")
    {
    }
}
