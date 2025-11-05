using Haviliar.Application.Networks.Services.Interfaces;
using Haviliar.DataTransfer.Networks.Requests;
using Haviliar.DataTransfer.Networks.Responses;
using Haviliar.Domain.Networks.Entities;
using Haviliar.Domain.Networks.Exceptions;
using Haviliar.Domain.Networks.Repositories;
using Haviliar.Domain.Pagination.Entities;
using Haviliar.Domain.Users.Exceptions;
using Haviliar.Domain.Users.Repositories;
using LanguageExt;
using LanguageExt.Common;

namespace Haviliar.Application.Networks.Services;

public class NetworkAppService : INetworkAppService
{
    private readonly INetworkRepository _networkRepository;
    private readonly IUserRepository _userRepository;

    public NetworkAppService(INetworkRepository networkRepository,
        IUserRepository userRepository)
    {
        _networkRepository = networkRepository;
        _userRepository = userRepository;
    }

    public async Task<Result<Unit>> DeleteNetworkAsync(int networkId, CancellationToken cancellationToken)
    {
        int? userId = _userRepository.GetCurrentUserId();

        if (userId is null)
            return new Result<Unit>(new UserUnauthorizedException());

        Network? network = await _networkRepository.GetByIdAsync(networkId, cancellationToken);

        if (network is null)
            return new Result<Unit>(new NetworkNotFoundException());

        await _networkRepository.Delete(network, cancellationToken);

        return Unit.Default;
    }

    public async Task<Result<NetworkResponse>> GetNetworkByIdAsync(int networkId, CancellationToken cancellationToken)
    {
        int? userId = _userRepository.GetCurrentUserId();

        if (userId is null)
            return new Result<NetworkResponse>(new UserUnauthorizedException());

        Network? network = await _networkRepository.GetByIdAsync(networkId, cancellationToken);

        if (network is null)
            return new Result<NetworkResponse>(new NetworkNotFoundException());

        NetworkResponse response = new() 
        {
            NetworkName = network.NetworkName,
            NetworkId = network.NetworkId,
            OperationCenterId = network.OperationCenterId,
        };

        return new Result<NetworkResponse>(response);
    }

    public Task<Result<PaginationResult<PaginationNetworkResponse>>> GetPaginatedAsync(PaginationNetworkRequest request, int operationCenterId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<Unit>> RegisterNetworkAsync(NetworkRegisterRequest request, CancellationToken cancellationToken)
    {
        int? userId = _userRepository.GetCurrentUserId();

        if (userId is null)
            return new Result<Unit>(new UserUnauthorizedException());

        if (await _networkRepository.AlreadyExistAsync(x => x.NetworkName == request.NetworkName && x.OperationCenterId == request.OperationCenterId, cancellationToken))
            return new Result<Unit>(new NetworkNameAlreadyExistsException());

        Network network = new()
        {
            NetworkName = request.NetworkName,
            OperationCenterId = request.OperationCenterId,

        };

        await _networkRepository.InsertAsync(network, cancellationToken);

        return Unit.Default;
    }

    public async Task<Result<Unit>> UpdateNetworkAsync(int networkId, NetworkUpdateRequest request, CancellationToken cancellationToken)
    {
        int? userId = _userRepository.GetCurrentUserId();

        if (userId is null)
            return new Result<Unit>(new UserUnauthorizedException());

        Network? network = await _networkRepository.GetByIdAsync(networkId, cancellationToken);

        if (network is null)
            return new Result<Unit>(new NetworkNotFoundException());

        if (await _networkRepository.AlreadyExistAsync(x => x.NetworkName == request.NetworkName && x.OperationCenterId == network.OperationCenterId && x.NetworkId != networkId, cancellationToken))
            return new Result<Unit>(new NetworkNameAlreadyExistsException());

        network.NetworkName = request.NetworkName;

        await _networkRepository.UpdateAsync(network, cancellationToken);

        return Unit.Default;
    }
}
