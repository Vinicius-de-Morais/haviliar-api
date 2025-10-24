namespace Haviliar.Domain.OperationCenters.Entities;

public class OperationCenter
{
    public int OperationCenterId { get; set; }
    public required string Name { get; set; }
    public bool IsActive { get; set; }
}
