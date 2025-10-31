using Haviliar.DataTransfer.Pagination.Requests;
using Haviliar.Domain.Pagination.Enums;

namespace Haviliar.DataTransfer.OperationCenters.Requests;

public record PaginationOperationCenterRequest : PaginationFilterRequest
{
    public PaginationOperationCenterRequest() : base(["Name"], [OrderDirectionEnum.Asc])
    {
        
    }

    /// <summary>
    /// Texto para busca, aplicável a Nome.
    /// </summary>
    public string? Search { get; set; }
}
