namespace Haviliar.Domain.Users.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException() : base("O usuário não foi encontrado.")
    {

    }
}
