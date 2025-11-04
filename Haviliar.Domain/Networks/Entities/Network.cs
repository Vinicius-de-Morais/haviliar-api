using Haviliar.Domain.OperationCenters.Entities;

namespace Haviliar.Domain.Networks.Entities;

public class Network
{
    public int NetworkId { get; set; }
    public required string NetworkName { get; set; }
    public int OperationCenterId { get; set; }
    public virtual OperationCenter OperationCenter { get; set; } = null!;
}
