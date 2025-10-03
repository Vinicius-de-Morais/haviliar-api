using Haviliar.Domain.Pagination.Enums;

namespace Haviliar.DataTransfer.Pagination.Requests;

/// <summary>
/// Dados para filtragem e paginação de solicitações
/// </summary>
public record PaginationFilterRequest
{
    /// <summary>
    /// Lista de campos para ordenação
    /// </summary>
    public IList<string> Sort { get; set; } = new List<string>();

    /// <summary>
    /// Direção da ordenação para cada campo: Ascendente| Descendete
    /// </summary>
    public IList<OrderDirectionEnum> Order { get; set; } = new List<OrderDirectionEnum>();

    /// <summary>
    /// Número de registros por página
    /// </summary>
    public int PerPage { get; set; }

    /// <summary>
    /// Número da página atual
    /// </summary>
    public int Page { get; set; }

    public PaginationFilterRequest()
    {
        PerPage = 10;
        Page = 0;
    }

    public PaginationFilterRequest(IList<string> sort, IList<OrderDirectionEnum> order)
    {
        Sort = sort;
        Order = order;
        PerPage = 10;
        Page = 0;
    }
}

