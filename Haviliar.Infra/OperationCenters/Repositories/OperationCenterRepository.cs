using Haviliar.Domain.OperationCenters.Entities;
using Haviliar.Domain.OperationCenters.Repositories;
using Haviliar.Domain.OperationCenters.Repositories.Filters;
using Haviliar.Domain.OperationCenters.Repositories.Projections;
using Haviliar.Domain.Users.Repositories.Filters;
using Haviliar.Domain.Users.Repositories.Projections;
using Haviliar.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Haviliar.Infra.OperationCenters.Repositories;

public class OperationCenterRepository : RepositoryBase<OperationCenter>, IOperationCenterRepository
{
    public OperationCenterRepository(AppDbContext context) : base(context)
    {
    }
}
