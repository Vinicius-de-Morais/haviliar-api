using Haviliar.Domain.Pagination.Entities;

namespace Haviliar.Domain.Networks.Repositories.Filters;

public class NetworkFilter : PaginationFilter
{
    public string? Search { get; set; }
}
