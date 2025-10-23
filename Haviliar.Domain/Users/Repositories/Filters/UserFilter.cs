using Haviliar.Domain.Pagination.Entities;

namespace Haviliar.Domain.Users.Repositories.Filters;

public class UserFilter : PaginationFilter
{
    public string? Search { get; set; }
}
