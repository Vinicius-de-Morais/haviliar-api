namespace Haviliar.DataTransfer.Networks.Requests;

/// <summary>
/// Dados para a atualização de uma rede.
/// </summary>
public record NetworkUpdateRequest
{
    /// <summary>
    /// Nome da rede.
    /// </summary>
    public required string NetworkName { get; init; }
}
