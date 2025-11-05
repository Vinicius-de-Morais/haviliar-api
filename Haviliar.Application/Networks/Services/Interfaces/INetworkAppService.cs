using Haviliar.DataTransfer.Networks.Requests;
using Haviliar.DataTransfer.Networks.Responses;
using Haviliar.Domain.Pagination.Entities;
using LanguageExt;
using LanguageExt.Common;

namespace Haviliar.Application.Networks.Services.Interfaces;

public interface INetworkAppService
{
    Task<Result<Unit>> RegisterNetworkAsync(NetworkRegisterRequest request, CancellationToken cancellationToken);
    Task<Result<Unit>> UpdateNetworkAsync(int networkId, NetworkUpdateRequest request, CancellationToken cancellationToken);
    Task<Result<Unit>> DeleteNetworkAsync(int networkId, CancellationToken cancellationToken);
    Task<Result<NetworkResponse>> GetNetworkByIdAsync(int networkId, CancellationToken cancellationToken);
    Task<Result<PaginationResult<PaginationNetworkResponse>>> GetPaginatedAsync(PaginationNetworkRequest request, int operationCenterId, CancellationToken cancellationToken);
}
