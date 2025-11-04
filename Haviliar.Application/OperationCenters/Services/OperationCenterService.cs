using Haviliar.Application.OperationCenters.Services.Interfaces;
using Haviliar.DataTransfer.OperationCenters.Requests;
using Haviliar.DataTransfer.OperationCenters.Responses;
using Haviliar.Domain.OperationCenters.Entities;
using Haviliar.Domain.OperationCenters.Exceptions;
using Haviliar.Domain.OperationCenters.Repositories;
using Haviliar.Domain.OperationCenters.Repositories.Filters;
using Haviliar.Domain.OperationCenters.Repositories.Projections;
using Haviliar.Domain.Pagination.Entities;
using Haviliar.Domain.Users.Entities;
using Haviliar.Domain.Users.Exceptions;
using Haviliar.Domain.Users.Repositories;
using Haviliar.Infra.Utils;
using LanguageExt;
using LanguageExt.Common;

namespace Haviliar.Application.OperationCenters.Services;

public class OperationCenterService : IOperationCenterService
{
    private readonly IOperationCenterRepository _operationCenterRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserOperationCenterRepository _userOperationCenterRepository;

    public OperationCenterService(IOperationCenterRepository operationCenterRepository,
        IUserRepository userRepository,
        IUserOperationCenterRepository userOperationCenterRepository)
    {
        _operationCenterRepository = operationCenterRepository;
        _userRepository = userRepository;
        _userOperationCenterRepository = userOperationCenterRepository;
    }


    public async Task<Result<Unit>> DeleteOperationCenterAsync(int operationCenterId, CancellationToken cancellationToken)
    {
        OperationCenter? operationCenter = await _operationCenterRepository.GetByIdAsync(operationCenterId);

        if (operationCenter is null)
            return new Result<Unit>(new OperationCenterNotFoundException());

        await _operationCenterRepository.Delete(operationCenter, cancellationToken);

        return Unit.Default;
    }

    public async Task<Result<OperationCenterResponse>> GetOperationCenterByIdAsync(int operationCenterId, CancellationToken cancellationToken)
    {
        int? userId = _userRepository.GetCurrentUserId();

        if (userId is null)
            return new Result<OperationCenterResponse>(new UserUnauthorizedException());

        OperationCenter? operationCenter = await _userOperationCenterRepository.GetByAuthUser(operationCenterId, userId!.Value, cancellationToken);

        if (operationCenter is null)
            return new Result<OperationCenterResponse>(new OperationCenterNotFoundException());

        OperationCenterResponse response = new OperationCenterResponse
        {
            Name = operationCenter.Name,
            OperationCenterId = operationCenter.OperationCenterId,
            IsActive = operationCenter.IsActive
        };

        return response;
    }

    public async Task<Result<PaginationResult<PaginationOperationCenterResponse>>> GetPaginatedAsync(PaginationOperationCenterRequest request, CancellationToken cancellationToken)
    {
        int? userId = _userRepository.GetCurrentUserId();

        if (userId is null)
            return new Result<PaginationResult<PaginationOperationCenterResponse>>(new UserUnauthorizedException());

        OperationCenterFilter filter = new OperationCenterFilter
        {
            Search = request.Search,
            PerPage = request.PerPage,
            Page = request.Page,
            Sort = request.Sort,
            Order = request.Order
        };

        IEnumerable<OperationCenterPaginatedProjection> operationCenters = await _userOperationCenterRepository.GetOperationCentersPaginatedAsync(filter, userId.Value, cancellationToken);

        List<PaginationOperationCenterResponse> operationCenterResponses = operationCenters.Select(op => new PaginationOperationCenterResponse
        {
            OperationCenterId = op.OperationCenterId,
            Name = op.Name,
            IsActive = op.IsActive
        }).ToList();

        PaginationResult<PaginationOperationCenterResponse> operationCentersPaginated = new(
            operationCenterResponses, new PaginationResultParams(filter.TotalItems, filter.Page, filter.PerPage));

        return new Result<PaginationResult<PaginationOperationCenterResponse>>(operationCentersPaginated);
    }

    public async Task<Result<Unit>> LinkUsersAsync(int operationCenterId, List<EncryptedInt> usersIds, CancellationToken cancellationToken)
    {
        int? userId = _userRepository.GetCurrentUserId();

        if (userId is null)
            return new Result<Unit>(new UserUnauthorizedException());

        OperationCenter? operationCenter = await _userOperationCenterRepository.GetByAuthUser(operationCenterId, userId.Value, cancellationToken);

        if (operationCenter is null)
            return new Result<Unit>(new OperationCenterNotFoundException());

        var existingLinks = await _userOperationCenterRepository.GetAllAsync(uoc => uoc.OperationCenterId == operationCenterId &&
                        usersIds.Contains(uoc.UserId), cancellationToken);

        var existingUserIds = existingLinks.Select(x => x.UserId).ToHashSet();

        var newUserIds = usersIds.Where(id => !existingUserIds.Contains(id)).ToList();

        if (newUserIds.Count == 0)
            return Unit.Default;

        var newLinks = newUserIds.Select(uid => new UserOperationCenter
        {
            UserId = uid,
            OperationCenterId = operationCenterId
        }).ToList();

        await _userOperationCenterRepository.InsertManyAsync(newLinks, cancellationToken);

        return Unit.Default;


    }

    public async Task<Result<Unit>> RegisterOperationCenterAsync(OperationCenterUpsertRequest request, CancellationToken cancellationToken)
    {
        if (await _operationCenterRepository.AlreadyExistAsync(oc => oc.Name.ToLower() == request.Name.ToLower(), cancellationToken))
        {
            return new Result<Unit>(new OperationCenterNameAlreadyExistsException());
        }

        OperationCenter operationCenter = new OperationCenter
        {
            Name = request.Name,
            IsActive = true
        };

        await _operationCenterRepository.InsertAsync(operationCenter, cancellationToken);

        return Unit.Default;
    }

    public async Task<Result<Unit>> UpdateOperationCenterAsync(int operationCenterId, OperationCenterUpsertRequest request, CancellationToken cancellationToken)
    {
        OperationCenter? operationCenter = await _operationCenterRepository.GetByIdAsync(operationCenterId);

        if (operationCenter is null)
        {
            return new Result<Unit>(new OperationCenterNotFoundException());
        }

        if (await _operationCenterRepository.AlreadyExistAsync(oc => oc.Name.ToLower() == request.Name.ToLower() && oc.OperationCenterId != operationCenterId, cancellationToken))
        {
            return new Result<Unit>(new OperationCenterNameAlreadyExistsException());
        }

        operationCenter.Name = request.Name;
        operationCenter.IsActive = request.IsActive ?? false;

        await _operationCenterRepository.UpdateAsync(operationCenter, cancellationToken);

        return Unit.Default;
    }
}
