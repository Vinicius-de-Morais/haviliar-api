namespace Haviliar.Domain.Networks.Exceptions;

public class NetworkNotFoundException : Exception
{
    public NetworkNotFoundException() : base($"Rede não encontrada")
    {
    }
}
