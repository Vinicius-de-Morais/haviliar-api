namespace Haviliar.Domain.Auth.Interfaces;

public interface ITokenGenerator
{
    string GenerateAuthToken(int userId);
}
