namespace Haviliar.DataTransfer.Networks.Requests;

/// <summary>
/// Dados para o registro de uma rede.
/// </summary>
public record NetworkRegisterRequest
{
    /// <summary>
    /// Nome da rede.
    /// </summary>
    public required string NetworkName { get; init; }

    /// <summary>
    /// ID do centro de operações ao qual a rede pertence.
    /// </summary>
    public required int OperationCenterId { get; init; }
}
