﻿namespace Haviliar.DataTransfer.OperationCenters.Requests;

/// <summary>
/// Dados de requisição para registro ou atualização de um centro de operações.
/// </summary>
public record OperationCenterUpsertRequest
{
    /// <summary>
    /// Nome do centro de operações.
    /// </summary>
    public required string Name { get; init; }
}
