using Haviliar.Domain.Pagination.Entities;

namespace Haviliar.Domain.OperationCenters.Repositories.Filters;

public class OperationCenterFilter : PaginationFilter
{
    public string? Search { get; set; }
}
