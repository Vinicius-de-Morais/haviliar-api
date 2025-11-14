namespace Haviliar.Domain.Auth.Interfaces;

public interface ITokenService
{
    Task<T?> ExtractData<T>(string token);
}
