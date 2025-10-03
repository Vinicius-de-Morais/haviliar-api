namespace Haviliar.Application.Users.Services.Interfaces;

using Haviliar.DataTransfer.Users.Requests;
using Haviliar.DataTransfer.Users.Responses;
using Haviliar.Domain.Pagination.Entities;
using LanguageExt;
using LanguageExt.Common;

public interface IUserAppService
{
    Task<Result<Unit>> RegisterUserAsync(UserRegisterRequest UserRegister, CancellationToken cancellationToken);
    Task<Result<UserResponse>> GetUserByIdAsync(int UserId, CancellationToken cancellationToken);
    Task<Result<PaginationResult<PaginationUserResponse>>> GetPaginatedAsync(PaginationUserRequest UserRequest, CancellationToken cancellationToken);
    Task<Result<Unit>> UpdateUserAsync(int UserId, UserUpdateRequest UserUpdate, CancellationToken cancellationToken);
    Task<Result<Unit>> DeleteUserAsync(int UserId, CancellationToken cancellationToken);
}
