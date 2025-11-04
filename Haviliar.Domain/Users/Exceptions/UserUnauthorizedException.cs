namespace Haviliar.Domain.Users.Exceptions;

public class UserUnauthorizedException : Exception
{
    public UserUnauthorizedException() : base("Usuário não autorizado.")
    {
        
    }
}
