namespace Haviliar.DataTransfer.OperationCenters.Responses;

/// <summary>
/// Dados de resposta para um centro de operações.
/// </summary>
public record OperationCenterResponse
{
    /// <summary>
    /// Identificador único do centro de operações.
    /// </summary>
    public int OperationCenterId { get; init; }
    /// <summary>
    /// Nome do centro de operações.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Indica se o centro de operações está ativo.
    /// </summary>
    public bool IsActive { get; init; }
}
