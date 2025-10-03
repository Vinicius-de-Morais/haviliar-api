namespace Haviliar.Domain.Users.Exceptions;

public class DocumentAlreadyExistsException : Exception
{
    public DocumentAlreadyExistsException() : base("CPF já cadastrado em outro cliente")
    {

    }
}
