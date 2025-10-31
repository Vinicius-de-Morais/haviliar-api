namespace Haviliar.Domain.OperationCenters.Exceptions;

public class OperationCenterNotFoundException : Exception
{
    public OperationCenterNotFoundException() : base("Nenhum centro de operações encontrado.")
    {
        
    }
}
