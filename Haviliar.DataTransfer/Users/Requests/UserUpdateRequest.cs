using Haviliar.Domain.Users.Enums;

namespace Haviliar.DataTransfer.Users.Requests;

/// <summary>
/// Dados necessários para atualizar um usuário.
/// </summary>
public class UserUpdateRequest
{
    /// <summary>
    /// Email do usuário.
    /// </summary>
    public required string Email { get; init; }

    /// <summary>
    /// Documento do usuário (CPF, RG, etc.).
    /// </summary>
    public required string Document { get; init; }

    /// <summary>
    /// Endereço do usuário.
    /// </summary>
    public required UserAddressRequest UserAddressRequest { get; init; }

    /// <summary>
    /// Telefone do usuário.
    /// </summary>
    public required string Phone { get; init; }

    /// <summary>
    /// Nome de usuário.
    /// </summary>
    public required string UserName { get; init; }

    /// <summary>
    /// Data de nascimento do usuário.
    /// </summary>
    public required DateOnly DateOfBirth { get; init; }

    /// <summary>
    /// Tipo do usuário (Admin, Regular, etc.).
    /// </summary>
    public required UserTypeEnum UserType { get; init; }
}
