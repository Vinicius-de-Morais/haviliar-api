using Haviliar.Application.Users.Services.Interfaces;
using Haviliar.DataTransfer.Users.Requests;
using Haviliar.DataTransfer.Users.Responses;
using Haviliar.Domain.Pagination.Entities;
using Haviliar.Domain.Users.Entities;
using Haviliar.Domain.Users.Exceptions;
using Haviliar.Domain.Users.Repositories;
using Haviliar.Domain.Users.Repositories.Filters;
using Haviliar.Domain.Users.Repositories.Projections;
using LanguageExt;
using LanguageExt.Common;

namespace Haviliar.Application.Users.Services;

public class UserAppService(IUserRepository userRepository) : IUserAppService
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Result<Unit>> DeleteUserAsync(int UserId, CancellationToken cancellationToken)
    {
        User? user = await _userRepository.GetByIdAsync(UserId, cancellationToken);

        if (user is null)
        {
            return new Result<Unit>(new UserNotFoundException());
        }

        await _userRepository.Delete(user, cancellationToken);

        return Unit.Default;
    }

    public async Task<Result<PaginationResult<PaginationUserResponse>>> GetPaginatedAsync(PaginationUserRequest userRequst, CancellationToken cancellationToken)
    {
        UserFilter filter = new UserFilter
        {
            Search = userRequst.Search,
            PerPage = userRequst.PerPage,
            Page = userRequst.Page,
            Sort = userRequst.Sort,
            Order = userRequst.Order
        };

        IEnumerable<UsersPaginatedProjection> users = await _userRepository.GetUsersPaginatedAsync(filter, cancellationToken);

        List<PaginationUserResponse> userResponses = users.Select(user => new PaginationUserResponse
        {
            UserId = user.UserId,
            UserName = user.UserName,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            UserType = user.UserType
        }).ToList();

        PaginationResult<PaginationUserResponse> usersPaginated = new(
            userResponses, new PaginationResultParams(filter.TotalItems, filter.Page, filter.PerPage));

        return new Result<PaginationResult<PaginationUserResponse>>(usersPaginated);    
    }


    public async Task<Result<UserResponse>> GetUserByIdAsync(int UserId, CancellationToken cancellationToken)
    {
        User? user = await _userRepository.GetByIdAsync(UserId, cancellationToken);

        if (user is null)
        {
            return new Result<UserResponse>(new UserNotFoundException());
        }

        UserResponse userResponse = new UserResponse
        {
            UserId = user.UserId,
            UserName = user.UserName,
            Email = user.Email,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            Document = user.Document,
            Address = user.Address,
            Phone = user.Phone,
            DateOfBirth = user.DateOfBirth,
            UserType = user.UserType
        };

        return userResponse;
    }

    public async Task<Result<Unit>> RegisterUserAsync(UserRegisterRequest userRegister, CancellationToken cancellationToken)
    {
        if (await _userRepository.AlreadyExistAsync(u => u.Email == userRegister.Email, cancellationToken))
        {
            return new Result<Unit>(new EmailAlreadyExistsException());
        }

        if(await _userRepository.AlreadyExistAsync(u => u.Document == userRegister.Document, cancellationToken))
        {
            return new Result<Unit>(new DocumentAlreadyExistsException());
        }

        User newUser = new User
        {
            UserName = userRegister.UserName,
            Password = userRegister.Password,
            Email = userRegister.Email,
            Document = userRegister.Document,
            Address = $"{userRegister.UserAddressRequest.Street}, {userRegister.UserAddressRequest.Number}, {userRegister.UserAddressRequest.Neighborhood}, {userRegister.UserAddressRequest.City}, {userRegister.UserAddressRequest.State}, {userRegister.UserAddressRequest.ZipCode}" + (userRegister.UserAddressRequest.Complement != null ? $", {userRegister.UserAddressRequest.Complement}" : string.Empty),
            Phone = userRegister.Phone,
            DateOfBirth = userRegister.DateOfBirth,
            UserType = userRegister.UserType,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _userRepository.InsertAsync(newUser, cancellationToken);

        return Unit.Default;
    }

    public async Task<Result<Unit>> UpdateUserAsync(int UserId, UserUpdateRequest userUpdate, CancellationToken cancellationToken)
    {
        User? user = await _userRepository.GetByIdAsync(UserId, cancellationToken);

        if (user is null)
        {
            return new Result<Unit>(new UserNotFoundException());
        }

        if(user.Email != userUpdate.Email &&
            await _userRepository.AlreadyExistAsync(u => u.Email == userUpdate.Email, cancellationToken))
        {
            return new Result<Unit>(new EmailAlreadyExistsException());
        }

        if(user.Document != userUpdate.Document &&
            await _userRepository.AlreadyExistAsync(u => u.Document == userUpdate.Document, cancellationToken))
        {
            return new Result<Unit>(new DocumentAlreadyExistsException());
        }

        user.UserName = userUpdate.UserName;
        user.Email = userUpdate.Email;
        user.Document = userUpdate.Document;
        user.Address = $"{userUpdate.UserAddressRequest.Street}, {userUpdate.UserAddressRequest.Number}, {userUpdate.UserAddressRequest.Neighborhood}, {userUpdate.UserAddressRequest.City}, {userUpdate.UserAddressRequest.State}, {userUpdate.UserAddressRequest.ZipCode}" + (userUpdate.UserAddressRequest.Complement != null ? $", {userUpdate.UserAddressRequest.Complement}" : string.Empty);
        user.Phone = userUpdate.Phone;
        user.DateOfBirth = userUpdate.DateOfBirth;
        user.UserType = userUpdate.UserType;

        return Unit.Default;
    }
}
