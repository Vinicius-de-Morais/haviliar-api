using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;
using Haviliar.Application.Auth.Services.Interfaces;
using Haviliar.DataTransfer.Auth.Response;
using Haviliar.Domain.Auth.Exceptions;
using LoginRequest = Haviliar.DataTransfer.Auth.Request.LoginRequest;

namespace haviliar_api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    /// <summary>
    /// Realiza a autenticação do usuário no sistema.
    /// </summary>
    /// <remarks>
    /// Este endpoint valida as credenciais enviadas no corpo da requisição.
    /// Caso o login seja bem-sucedido, retorna as informações do usuário autenticado
    /// juntamente com um token JWT.
    ///
    /// Possíveis cenários:
    /// - <b>200 OK</b>: credenciais válidas, usuário autenticado.
    /// - <b>401 Unauthorized</b>: credenciais inválidas.
    /// - <b>500 Internal Server Error</b>: erro inesperado.
    ///
    /// Exemplo de requisição:
    /// <code>
    /// POST /api/auth/login
    /// {
    ///   "email": "usuario@dominio.com",
    ///   "senha": "123456"
    /// }
    /// </code>
    ///
    /// Exemplo de resposta (200):
    /// <code>
    /// {
    ///   "token": "jwt.token.aqui",
    /// }
    /// </code>
    /// </remarks>
    /// <param name="request">Dados de login contendo email e senha.</param>
    /// <param name="cancellationToken">Token de cancelamento da operação assíncrona.</param>
    /// <returns>Retorna o status da autenticação e os dados do usuário, se válido.</returns>
    /// <response code="200">Retorna o token de autenticação e os dados do usuário.</response>
    /// <response code="401">Credenciais inválidas.</response>
    /// <response code="500">Erro interno inesperado ao tentar autenticar.</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        Result<LoginResponse> result = await _authService.Login(request, cancellationToken);

        return result.Match<IActionResult>(Ok, ex =>
        {
            int statusCode = ex switch
            {
                CredenciaisInvalidasException => StatusCodes.Status401Unauthorized,
                _ => StatusCodes.Status500InternalServerError
            };

            return StatusCode(statusCode, ex.Message);

        });
    }
}
