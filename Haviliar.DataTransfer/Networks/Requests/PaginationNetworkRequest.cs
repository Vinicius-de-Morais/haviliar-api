using Haviliar.DataTransfer.Pagination.Requests;
using Haviliar.Domain.Pagination.Enums;

namespace Haviliar.DataTransfer.Networks.Requests;

public record PaginationNetworkRequest : PaginationFilterRequest
{
    public PaginationNetworkRequest() : base(["Name"], [OrderDirectionEnum.Asc])
    {

    }

    /// <summary>
    /// Texto para busca, aplicável a Nome.
    /// </summary>
    public string? Search { get; set; }
}
