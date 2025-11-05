using System.Net.Mime;
using Haviliar.Application.Networks.Services.Interfaces;
using Haviliar.Domain.Pagination.Entities;
using Haviliar.Infra.Security;
using Haviliar.Infra.Utils;
using LanguageExt.Common;
using LanguageExt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Haviliar.DataTransfer.Networks.Requests;
using Haviliar.DataTransfer.Networks.Responses;
using Haviliar.Domain.Networks.Exceptions;

namespace haviliar_api.Controllers;

[ApiController]
[Route("api/network")]
[Authorize]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
public class NetworkController : ControllerBase
{
    private readonly INetworkAppService _networkAppService;

    public NetworkController(INetworkAppService networkAppService)
    {
        _networkAppService = networkAppService;
    }

    /// <summary>
    /// Registrar uma rede.
    /// </summary>
    /// <param name="request">Dados da rede a ser registrada</param>
    /// <param name="cancellationToken"></param>
    /// <response code="204">Rede criada com sucesso</response>
    /// <response code="400">A requisição contém campos inválidos ou em formato incorreto</response>
    /// <response code="409">Nome já utilizado por outra rede.</response>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(Created), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> RegisterNetworkAsync([FromBody] NetworkRegisterRequest request, CancellationToken cancellationToken)
    {
        Result<Unit> result = await _networkAppService.RegisterNetworkAsync(request, cancellationToken);

        return result.Match<IActionResult>(_ => NoContent(), ex =>
        {
            int statusCode = ex switch
            {
                NetworkNameAlreadyExistsException => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status400BadRequest,
            };
            return Problem(ex.Message,
                statusCode: statusCode);
        });
    }

    /// <summary>
    ///  Listar as redes vinculadas ao centro de operações.
    /// </summary>
    /// <param name="request">Parâmetros para filtrar os redes</param>
    /// <param name="operationCenterId">Id do centro de operações criptografado</param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">Lista das redes encontrados</response>
    /// <response code="400">A requisição contém campos inválidos ou em formato incorreto</response>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginationResult<PaginationNetworkResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetNetworksPaginatedAsync([FromRoute][ModelBinder(BinderType = typeof(EncryptedIdModelBinder))] EncryptedInt operationCenterId, [FromQuery] PaginationNetworkRequest request, CancellationToken cancellationToken)
    {
        Result<PaginationResult<PaginationNetworkResponse>> result = await _networkAppService.GetPaginatedAsync(request, operationCenterId,  cancellationToken);

        return result.Match<IActionResult>(Ok, ex =>
        {
            int statusCode = ex switch
            {
                NetworkNotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status400BadRequest,
            };
            return Problem(ex.Message, statusCode: statusCode);
        });

    }

    /// <summary>
    /// Retornar uma rede específica.
    /// </summary>
    /// <param name="networkId">Id da rede criptografado</param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">Rede retornada com sucesso</response>
    /// <response code="404">Rede não encontrada</response>
    /// <returns></returns>
    [HttpGet("{networkId}")]
    [ProducesResponseType(typeof(NetworkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetNetworkByIdAsync([FromRoute][ModelBinder(BinderType = typeof(EncryptedIdModelBinder))] EncryptedInt networkId, CancellationToken cancellationToken)
    {
        Result<NetworkResponse> result = await _networkAppService.GetNetworkByIdAsync(networkId, cancellationToken);

        return result.Match<IActionResult>(Ok, ex =>
        {
            int statusCode = ex switch
            {
                NetworkNotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status400BadRequest,
            };
            return Problem(ex.Message, statusCode: statusCode);
        });
    }

    /// <summary>
    /// Editar uma rede.
    /// </summary>
    /// <param name="networkId">Id da rede criptografado</param>
    /// <param name="request">Dados da rede a ser editada.</param>
    /// <param name="cancellationToken"></param>
    /// <response code="204">Rede editado com sucesso</response>
    /// <response code="400">A requisição contém campos inválidos ou em formato incorreto</response>
    /// <response code="404">Rede não encontrado</response>
    /// <response code="409">Nome já utilizado por outra rede.</response>
    /// <returns></returns>
    [HttpPatch("{networkId}")]
    [ProducesResponseType(typeof(NoContent), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> UpdateNetworkAsync([FromRoute][ModelBinder(BinderType = typeof(EncryptedIdModelBinder))] EncryptedInt networkId, [FromBody] NetworkUpdateRequest request, CancellationToken cancellationToken)
    {
        Result<Unit> result = await _networkAppService.UpdateNetworkAsync(networkId, request, cancellationToken);

        return result.Match<IActionResult>(_ => NoContent(), ex =>
        {
            int statusCode = ex switch
            {
                NetworkNotFoundException => StatusCodes.Status404NotFound,
                NetworkNameAlreadyExistsException => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status400BadRequest,
            };
            return Problem(ex.Message, statusCode: statusCode);
        });
    }

    /// <summary>
    /// Excluir uma rede.
    /// </summary>
    /// <param name="networkId">Id da rede criptografada</param>
    /// <param name="cancellationToken"></param>
    /// <response code="204">Rede excluida com sucesso</response>
    /// <response code="404">Rede não encontrada</response>
    /// <returns></returns>
    [HttpDelete("{networkId}")]
    [ProducesResponseType(typeof(NoContent), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> DeleteNetworkAsync([FromRoute][ModelBinder(BinderType = typeof(EncryptedIdModelBinder))] EncryptedInt networkId, CancellationToken cancellationToken)
    {
        Result<Unit> result = await _networkAppService.DeleteNetworkAsync(networkId, cancellationToken);

        return result.Match<IActionResult>(_ => NoContent(), ex =>
        {
            int statusCode = ex switch
            {
                NetworkNotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status400BadRequest,
            };
            return Problem(ex.Message, statusCode: statusCode);
        });
    }

}
