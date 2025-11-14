using System.Security.Cryptography;
using System.Text;
using Haviliar.Application.Auth.Services.Interfaces;
using Haviliar.DataTransfer.Auth.Request;
using Haviliar.DataTransfer.Auth.Response;
using Haviliar.Domain.Auth.Exceptions;
using Haviliar.Domain.Auth.Interfaces;
using Haviliar.Domain.Users.Entities;
using Haviliar.Domain.Users.Repositories;
using LanguageExt.Common;

namespace Haviliar.Application.Auth.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenGenerator _tokenGenerator;

    public AuthService(IUserRepository userRepository, ITokenGenerator tokenGenerator)
    {
        _userRepository = userRepository;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<Result<LoginResponse>> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        User? user = await _userRepository.GetByUserNameAsync(request.Username, cancellationToken);

        if (user == null)
        {
            return new Result<LoginResponse>(new CredenciaisInvalidasException());
        }

        bool senhaValida = VerifyPassword(request.Password, user.Password);

        if (senhaValida is false)
        {
            return new Result<LoginResponse>(new CredenciaisInvalidasException());
        }

        string token = _tokenGenerator.GenerateAuthToken(user.UserId);

        return new LoginResponse(token);

    }

    private static bool VerifyPassword(string password, string hashedPassword)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] sha256Hash = sha256.ComputeHash(passwordBytes);
            string preHashed = Convert.ToBase64String(sha256Hash);

            return BCrypt.Net.BCrypt.Verify(preHashed, hashedPassword);
        }
    }
}
