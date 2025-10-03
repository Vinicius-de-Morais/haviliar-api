using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haviliar.Domain.Pagination.Entities;

public class PaginationResultParams(long count, int page, int perPage)
{
    public long Count { get; set; } = count;
    public int Page { get; set; } = page;
    public int PerPage { get; set; } = perPage;
}
