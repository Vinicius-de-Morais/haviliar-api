using Haviliar.DataTransfer.Auth.Request;
using Haviliar.DataTransfer.Auth.Response;
using LanguageExt.Common;

namespace Haviliar.Application.Auth.Services.Interfaces;

public interface IAuthService
{
    Task<Result<LoginResponse>> Login(LoginRequest request, CancellationToken cancellationToken);
}
