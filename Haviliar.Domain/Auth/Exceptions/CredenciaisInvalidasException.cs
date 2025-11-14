namespace Haviliar.Domain.Auth.Exceptions;

public class CredenciaisInvalidasException : Exception
{
    public CredenciaisInvalidasException() : base("Credenciais inválidas.")
    {
    }
}
