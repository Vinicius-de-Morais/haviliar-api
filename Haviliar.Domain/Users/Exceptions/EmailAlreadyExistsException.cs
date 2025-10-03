namespace Haviliar.Domain.Users.Exceptions;

public class EmailAlreadyExistsException : Exception
{
    public EmailAlreadyExistsException() : base("E-mail já cadastrado em outro cliente")
    {

    }
}
