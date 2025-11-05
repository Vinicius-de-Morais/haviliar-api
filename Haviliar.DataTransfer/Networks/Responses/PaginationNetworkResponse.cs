namespace Haviliar.DataTransfer.Networks.Responses;

/// <summary>
/// Dados de resposta paginada de redes.
/// </summary>
public record PaginationNetworkResponse
{
    /// <summary>
    /// ID da rede.
    /// </summary>
    public int NetworkId { get; init; }

    /// <summary>
    /// Nome da rede.
    /// </summary>
    public required string NetworkName { get; init; }
}
