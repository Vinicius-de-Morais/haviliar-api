using Haviliar.Domain.Users.Enums;

namespace Haviliar.DataTransfer.Users.Requests;

/// <summary>
/// Dados necessários para registrar um novo usuário.
/// </summary>
public record UserRegisterRequest
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
    /// Senha do usuário.
    /// </summary>
    public required string Password { get; init; }

    /// <summary>
    /// Data de nascimento do usuário.
    /// </summary>
    public required DateOnly DateOfBirth { get; init; }

    /// <summary>
    /// Tipo do usuário (Admin, Regular, etc.).
    /// </summary>
    public required UserTypeEnum UserType { get; init; }
}

/// <summary>
/// Dados do endereço do usuário.
/// </summary>
public class UserAddressRequest
{
    /// <summary>
    /// Cidade do endereço.
    /// </summary>
    public required string City { get; init; }

    /// <summary>
    /// Estado ou região do endereço.
    /// </summary>
    public required string State { get; init; }

    /// <summary>
    /// Bairro do endereço.
    /// </summary>
    public required string Neighborhood { get; init; }

    /// <summary>
    /// Código postal do endereço.
    /// </summary>
    public required string ZipCode { get; init; }

    /// <summary>
    /// Rua do endereço.
    /// </summary>
    public required string Street { get; init; }

    /// <summary>
    /// Número do endereço.
    /// </summary>
    public required string Number { get; init; }

    /// <summary>
    /// Complemento do endereço.
    /// </summary>
    public string? Complement { get; init; }
}
