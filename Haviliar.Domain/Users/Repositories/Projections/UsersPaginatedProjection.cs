using Haviliar.Domain.Users.Enums;

namespace Haviliar.Domain.Users.Repositories.Projections;

/// <summary>
/// Dados de projeção para paginação de usuários.
/// </summary>
public record UsersPaginatedProjection
{
    /// <summary>
    /// Identificador único do usuário.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Nome de usuário.
    /// </summary>
    public required string UserName { get; set; }

    /// <summary>
    /// Data de criação do usuário.
    /// </summary>
    public required DateTime CreatedAt { get; set; }

    /// <summary>
    /// Data da última atualização do usuário.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Tipo do usuário (Admin, Regular, etc.).
    /// </summary>
    public required UserTypeEnum UserType { get; set; }
}
