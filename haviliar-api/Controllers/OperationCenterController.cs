using Haviliar.DataTransfer.OperationCenters.Requests;
using Haviliar.DataTransfer.OperationCenters.Responses;
using Haviliar.Domain.Pagination.Entities;
using Haviliar.Infra.Security;
using Haviliar.Infra.Utils;
using LanguageExt.Common;
using LanguageExt;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Haviliar.Application.OperationCenters.Services.Interfaces;
using Haviliar.Domain.OperationCenters.Exceptions;

namespace haviliar_api.Controllers;

[ApiController]
[Route("api/operation-center")]
public class OperationCenterController : ControllerBase
{
    private readonly IOperationCenterService _operationCenterService;
    public OperationCenterController(IOperationCenterService operationCenterService)
    {
        _operationCenterService = operationCenterService;
    }

    /// <summary>
    /// Registrar um centro de operações.
    /// </summary>
    /// <param name="request">Dados do centro de operações a ser registrado</param>
    /// <param name="cancellationToken"></param>
    /// <response code="204">Centro de operações criado com sucesso</response>
    /// <response code="400">A requisição contém campos inválidos ou em formato incorreto</response>
    /// <response code="409">Nome já utilizado por outro centro de operações.</response>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(Created), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> RegisterOperationCenterAsync([FromBody] OperationCenterUpsertRequest request, CancellationToken cancellationToken)
    {
        Result<Unit> result = await _operationCenterService.RegisterOperationCenterAsync(request, cancellationToken);

        return result.Match<IActionResult>(_ => NoContent(), ex =>
        {
            int statusCode = ex switch
            {
                OperationCenterNameAlreadyExistsException => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status400BadRequest,
            };
            return Problem(ex.Message,
                statusCode: statusCode);
        });
    }

    /// <summary>
    ///  Listar os centro de operações
    /// </summary>
    /// <param name="request">Parâmetros para filtrar os centro de operações</param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">Lista dos centro de operações encontrados</response>
    /// <response code="400">A requisição contém campos inválidos ou em formato incorreto</response>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginationResult<PaginationOperationCenterResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetOperationCentersPaginatedAsync([FromQuery] PaginationOperationCenterRequest request, CancellationToken cancellationToken)
    {
        Result<PaginationResult<PaginationOperationCenterResponse>> result = await _operationCenterService.GetPaginatedAsync(request, cancellationToken);

        return result.Match<IActionResult>(Ok, ex =>
        {
            int statusCode = ex switch
            {
                OperationCenterNotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status400BadRequest,
            };
            return Problem(ex.Message, statusCode: statusCode);
        });

    }

    /// <summary>
    /// Retornar um centro de operações específico.
    /// </summary>
    /// <param name="operationCenterId">Id do centro de operações criptografado</param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">Centro de operações retornado com sucesso</response>
    /// <response code="404">Centro de operações não encontrado</response>
    /// <returns></returns>
    [HttpGet("{operationCenterId}")]
    [ProducesResponseType(typeof(OperationCenterResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetOperationCenterByIdAsync([FromRoute][ModelBinder(BinderType = typeof(EncryptedIdModelBinder))] EncryptedInt operationCenterId, CancellationToken cancellationToken)
    {
        Result<OperationCenterResponse> result = await _operationCenterService.GetOperationCenterByIdAsync(operationCenterId, cancellationToken);

        return result.Match<IActionResult>(Ok, ex =>
        {
            int statusCode = ex switch
            {
                _ => StatusCodes.Status404NotFound,
            };
            return Problem(ex.Message, statusCode: statusCode);
        });
    }

    /// <summary>
    /// Editar um centro de operações.
    /// </summary>
    /// <param name="operationCenterId">Id do centro de operações criptografado</param>
    /// <param name="request">Dados do centro de operações a ser editado.</param>
    /// <param name="cancellationToken"></param>
    /// <response code="204">Centro de operações editado com sucesso</response>
    /// <response code="400">A requisição contém campos inválidos ou em formato incorreto</response>
    /// <response code="404">Centro de operações não encontrado</response>
    /// <response code="409">Nome já utilizado por outro centro de operações.</response>
    /// <returns></returns>
    [HttpPatch("{operationCenterId}")]
    [ProducesResponseType(typeof(NoContent), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> UpdateOperationCenterAsync([FromRoute][ModelBinder(BinderType = typeof(EncryptedIdModelBinder))] EncryptedInt operationCenterId, [FromBody] OperationCenterUpsertRequest request, CancellationToken cancellationToken)
    {
        Result<Unit> result = await _operationCenterService.UpdateOperationCenterAsync(operationCenterId, request, cancellationToken);

        return result.Match<IActionResult>(_ => NoContent(), ex =>
        {
            int statusCode = ex switch
            {
                OperationCenterNotFoundException => StatusCodes.Status404NotFound,
                OperationCenterNameAlreadyExistsException => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status400BadRequest,
            };
            return Problem(ex.Message, statusCode: statusCode);
        });
    }

    /// <summary>
    /// Excluir um centro de operações.
    /// </summary>
    /// <param name="operationCenterId">Id do centro de operações criptografado</param>
    /// <param name="cancellationToken"></param>
    /// <response code="204">Centro de operações excluido com sucesso</response>
    /// <response code="404">Centro de operações não encontrado</response>
    /// <returns></returns>
    [HttpDelete("{operationCenterId}")]
    [ProducesResponseType(typeof(NoContent), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> DeleteOperationCenterAsync([FromRoute][ModelBinder(BinderType = typeof(EncryptedIdModelBinder))] EncryptedInt operationCenterId, CancellationToken cancellationToken)
    {
        Result<Unit> result = await _operationCenterService.DeleteOperationCenterAsync(operationCenterId, cancellationToken);

        return result.Match<IActionResult>(_ => NoContent(), ex =>
        {
            int statusCode = ex switch
            {
                OperationCenterNotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status400BadRequest,
            };
            return Problem(ex.Message, statusCode: statusCode);
        });
    }

    /// <summary>
    /// Vincula uma lista de usuários a um centro de operações.
    /// </summary>
    /// <param name="operationCenterId">Id do centro de operações criptografado.</param>
    /// <param name="usersIds">Lista de Ids criptografados dos usuários a serem vinculados.</param>
    /// <param name="cancellationToken"></param>
    /// <response code="204">Associação realizada com sucesso.</response>
    /// <response code="404">Centro de operações não encontrado.</response>
    /// <response code="400">Erro de validação ou IDs inválidos.</response>
    /// <returns></returns>
    [HttpPut("{operationCenterId}/users")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> LinkUsersToOperationCenterAsync(
     [FromRoute][ModelBinder(BinderType = typeof(EncryptedIdModelBinder))] EncryptedInt operationCenterId,
     [FromBody][ModelBinder(BinderType = typeof(EncryptedIdModelBinder))] List<EncryptedInt> usersIds,
     CancellationToken cancellationToken)
    {
        Result<Unit> result = await _operationCenterService.LinkUsersAsync(operationCenterId, usersIds, cancellationToken);

        return result.Match<IActionResult>(_ => NoContent(), ex =>
        {
            int statusCode = ex switch
            {
                OperationCenterNotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status400BadRequest,
            };
            return Problem(ex.Message, statusCode: statusCode);
        });
    }
}
