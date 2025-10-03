namespace Haviliar.DataTransfer.Users.Requests;

/// <summary>
/// Dados do endereço do usuário.
/// </summary>
public record UserAddressRequest
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
