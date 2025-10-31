using Haviliar.Domain.OperationCenters.Entities;
using Haviliar.Domain.OperationCenters.Repositories;
using Haviliar.Infra.Context;

namespace Haviliar.Infra.OperationCenters.Repositories;

public class OperationCenterRepository : RepositoryBase<OperationCenter>, IOperationCenterRepository
{
    public OperationCenterRepository(AppDbContext context) : base(context)
    {
    }
}
