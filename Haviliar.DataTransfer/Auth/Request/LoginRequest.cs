namespace Haviliar.DataTransfer.Auth.Request;

/// <summary>
/// Representa uma solicitação de login contendo as credenciais do usuário.
/// </summary>
public record LoginRequest
{
    /// <summary>
    /// Nome de usuário do usuário que está tentando fazer login.
    /// </summary>
    public required string Username { get; init; }

    /// <summary>
    /// Senha do usuário que está tentando fazer login.
    /// </summary>
    public required string Password { get; init; }
}
