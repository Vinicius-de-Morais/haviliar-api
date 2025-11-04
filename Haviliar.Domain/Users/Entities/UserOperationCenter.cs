using Haviliar.Domain.OperationCenters.Entities;

namespace Haviliar.Domain.Users.Entities;

public class UserOperationCenter
{
    public int UserOperationCenterId { get; set; }
    public int UserId { get; set; }
    public int OperationCenterId { get; set; }

    public virtual User User { get; set; } = null!;
    public virtual OperationCenter OperationCenter { get; set; } = null!;
}
