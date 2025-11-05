namespace Haviliar.DataTransfer.Networks.Responses;

/// <summary>
/// Dados de resposta de uma rede.
/// </summary>
public record NetworkResponse
{
    /// <summary>
    /// ID da rede.
    /// </summary>
    public int NetworkId { get; init; }
    /// <summary>
    /// Nome da rede.
    /// </summary>
    public required string NetworkName { get; init; }
    /// <summary>
    /// ID do centro de operações ao qual a rede pertence.
    /// </summary>
    public int OperationCenterId { get; init; }
}
