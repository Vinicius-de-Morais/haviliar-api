using Haviliar.Application.Users.Services.Interfaces;
using Haviliar.DataTransfer.Users.Requests;
using Haviliar.DataTransfer.Users.Responses;
using Haviliar.Domain.Pagination.Entities;
using Haviliar.Domain.Users.Exceptions;
using Haviliar.Infra.Security;
using Haviliar.Infra.Utils;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace haviliar_api.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserAppService _userAppService;

        public UserController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        /// <summary>
        /// Registrar um usuário.
        /// </summary>
        /// <param name="request">Dados do usuário a ser registrado</param>
        /// <param name="cancellationToken"></param>
        /// <response code="201">Usuário criado com sucesso</response>
        /// <response code="400">A requisição contém campos inválidos ou em formato incorreto</response>
        /// <response code="409">Documento ou e-mail já utilizado por outro Usuário.</response>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Created), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> RegisterUserAsync([FromBody] UserRegisterRequest request, CancellationToken cancellationToken)
        {
            Result<Unit> result = await _userAppService.RegisterUserAsync(request, cancellationToken);

            return result.Match<IActionResult>(_ => Created(), ex =>
            {
                int statusCode = ex switch
                {
                    DocumentAlreadyExistsException or EmailAlreadyExistsException => StatusCodes.Status409Conflict,
                    _ => StatusCodes.Status400BadRequest,
                };
                return Problem(ex.Message,
                    statusCode: statusCode);
            });
        }

        /// <summary>
        ///  Listar os Usuários
        /// </summary>
        /// <param name="request">Parâmetros para filtrar os Usuários</param>
        /// <param name="cancellationToken"></param>
        /// <response code="200">Lista dos Usuários encontrados</response>
        /// <response code="400">A requisição contém campos inválidos ou em formato incorreto</response>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PaginationResult<PaginationUserResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetUsersPaginatedAsync([FromQuery] PaginationUserRequest request, CancellationToken cancellationToken)
        {
            Result<PaginationResult<PaginationUserResponse>> result = await _userAppService.GetPaginatedAsync(request, cancellationToken);

            return result.Match<IActionResult>(Ok, ex =>
            {
                int statusCode = ex switch
                {
                    UserNotFoundException => StatusCodes.Status404NotFound,
                    _ => StatusCodes.Status400BadRequest,
                };
                return Problem(ex.Message, statusCode: statusCode);
            });

        }

        /// <summary>
        /// Retornar um Usuário específico.
        /// </summary>
        /// <param name="UserId">Id do Usuário criptografado</param>
        /// <param name="cancellationToken"></param>
        /// <response code="200">Usuário retornado com sucesso</response>
        /// <response code="404">Usuário não encontrado</response>
        /// <returns></returns>
        [HttpGet("{UserId}")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetUserByIdAsync([FromRoute][ModelBinder(BinderType = typeof(EncryptedIdModelBinder))] EncryptedInt UserId, CancellationToken cancellationToken)
        {
            Result<UserResponse> result = await _userAppService.GetUserByIdAsync(UserId, cancellationToken);

            return result.Match<IActionResult>(Ok, ex =>
            {
                int statusCode = ex switch
                {
                    UserNotFoundException => StatusCodes.Status404NotFound,
                    _ => StatusCodes.Status400BadRequest,
                };
                return Problem(ex.Message, statusCode: statusCode);
            });
        }

        /// <summary>
        /// Editar um Usuário.
        /// </summary>
        /// <param name="UserId">Id do Usuário criptografado</param>
        /// <param name="request">Dados do Usuário a ser editado.</param>
        /// <param name="cancellationToken"></param>
        /// <response code="204">Usuário editado com sucesso</response>
        /// <response code="400">A requisição contém campos inválidos ou em formato incorreto</response>
        /// <response code="404">Usuário não encontrado</response>
        /// <response code="409">Documento ou e-mail já utilizado por outro Usuário.</response>
        /// <returns></returns>
        [HttpPatch("{UserId}")]
        [ProducesResponseType(typeof(NoContent), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> UpdateUserAsync([FromRoute][ModelBinder(BinderType = typeof(EncryptedIdModelBinder))] EncryptedInt UserId, [FromBody] UserRegisterRequest request, CancellationToken cancellationToken)
        {
            Result<Unit> result = await _userAppService.UpdateUserAsync(UserId, request, cancellationToken);

            return result.Match<IActionResult>(_ => NoContent(), ex =>
            {
                int statusCode = ex switch
                {
                    UserNotFoundException => StatusCodes.Status404NotFound,
                    DocumentAlreadyExistsException or EmailAlreadyExistsException => StatusCodes.Status409Conflict,
                    _ => StatusCodes.Status400BadRequest,
                };
                return Problem(ex.Message, statusCode: statusCode);
            });
        }

        /// <summary>
        /// Excluir um Usuário.
        /// </summary>
        /// <param name="UserId">Id do Usuário criptografado</param>
        /// <param name="cancellationToken"></param>
        /// <response code="204">Usuário excluido com sucesso</response>
        /// <response code="404">Usuário não encontrado</response>
        /// <returns></returns>
        [HttpDelete("{UserId}")]
        [ProducesResponseType(typeof(NoContent), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> DeleteUserAsync([FromRoute][ModelBinder(BinderType = typeof(EncryptedIdModelBinder))] EncryptedInt UserId, CancellationToken cancellationToken)
        {
            Result<Unit> result = await _userAppService.DeleteUserAsync(UserId, cancellationToken);

            return result.Match<IActionResult>(_ => NoContent(), ex =>
            {
                int statusCode = ex switch
                {
                    UserNotFoundException => StatusCodes.Status404NotFound,
                    _ => StatusCodes.Status400BadRequest,
                };
                return Problem(ex.Message, statusCode: statusCode);
            });
        }
    }
}
