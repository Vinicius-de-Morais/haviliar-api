namespace Haviliar.Domain.OperationCenters.Exceptions;

public class OperationCenterNameAlreadyExistsException : Exception
{
    public OperationCenterNameAlreadyExistsException() : base("Nome já utilizado por outro centro de operações.")
    {
        
    }
}
