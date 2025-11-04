using Haviliar.Domain.Networks.Entities;
using Haviliar.Domain.Networks.Repositories;
using Haviliar.Infra.Context;

namespace Haviliar.Infra.Networks.Repositories;

public class NetworkRepository : RepositoryBase<Network>, INetworkRepository
{
    public NetworkRepository(AppDbContext context) : base(context)
    {
    }
}
