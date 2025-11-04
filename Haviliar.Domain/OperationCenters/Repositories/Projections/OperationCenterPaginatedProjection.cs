namespace Haviliar.Domain.OperationCenters.Repositories.Projections;

public class OperationCenterPaginatedProjection
{
    public int OperationCenterId { get; init; }
    public required string Name { get; init; }
    public bool IsActive { get; init; }
}
