using Haviliar.DataTransfer.Pagination.Requests;
using Haviliar.Domain.Pagination.Enums;

namespace Haviliar.DataTransfer.Users.Requests;

public record class PaginationUserRequest : PaginationFilterRequest
{
    public PaginationUserRequest() : base(["Name"], [OrderDirectionEnum.Asc])
    {

    }

    /// <summary>
    /// Texto para busca, aplicável a Nome ou CPF do cliente
    /// </summary>
    public string? Search { get; set; }
}