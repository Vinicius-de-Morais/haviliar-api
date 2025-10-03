using Haviliar.Domain.Pagination.Enums;

namespace Haviliar.Domain.Pagination.Entities;

public class PaginationFilter
{
    private int _perPage = 10;
    private int _page = 0;

    public IList<string> Sort { get; set; } = [];
    public IList<OrderDirectionEnum> Order { get; set; } = [];

    public long TotalItems { get; set; }

    public int PerPage
    {
        get
        {
            return _perPage;
        }
        set
        {
            _perPage = value < 100 ? value : 100;
        }
    }

    public int Page
    {
        get
        {
            return _page;
        }
        set
        {
            _page = value < 0 ? 0 : value;
        }
    }
}
