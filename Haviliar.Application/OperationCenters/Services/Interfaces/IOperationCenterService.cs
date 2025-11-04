using Haviliar.DataTransfer.OperationCenters.Requests;
using Haviliar.DataTransfer.OperationCenters.Responses;
using Haviliar.Domain.Pagination.Entities;
using LanguageExt;
using LanguageExt.Common;

namespace Haviliar.Application.OperationCenters.Services.Interfaces;

public interface IOperationCenterService
{
    Task<Result<Unit>> RegisterOperationCenterAsync(OperationCenterUpsertRequest request, CancellationToken cancellationToken);
    Task<Result<PaginationResult<PaginationOperationCenterResponse>>> GetPaginatedAsync(PaginationOperationCenterRequest request, CancellationToken cancellationToken);
    Task<Result<Unit>> UpdateOperationCenterAsync(int operationCenterId, OperationCenterUpsertRequest request, CancellationToken cancellationToken);
    Task<Result<Unit>> DeleteOperationCenterAsync(int operationCenterId, CancellationToken cancellationToken);
    Task<Result<OperationCenterResponse>> GetOperationCenterByIdAsync(int operationCenterId, CancellationToken cancellationToken);
    Task<Result<Unit>> LinkUsersAsync(int operationCenterId, List<int> usersIds, CancellationToken cancellationToken);

}
