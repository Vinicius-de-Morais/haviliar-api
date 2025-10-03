using Haviliar.Domain.Users.Enums;

namespace Haviliar.DataTransfer.Users.Responses;

/// <summary>
/// Dados de resposta para um usuário.
/// </summary>
public record UserResponse
{
    /// <summary>
    /// Identificador único do usuário.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Email do usuário.
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// Documento do usuário (CPF).
    /// </summary>
    public required string Document { get; set; }

    /// <summary>
    /// Endereço do usuário.
    /// </summary>
    public required string Address { get; set; }

    /// <summary>
    /// Telefone do usuário.
    /// </summary>
    public required string Phone { get; set; }

    /// <summary>
    /// Nome de usuário.
    /// </summary>
    public required string UserName { get; set; }

    /// <summary>
    /// Data de nascimento do usuário.
    /// </summary>
    public required DateOnly DateOfBirth { get; set; }

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
