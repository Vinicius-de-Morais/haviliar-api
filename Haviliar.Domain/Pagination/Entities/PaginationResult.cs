namespace Haviliar.Domain.Pagination.Entities;

public class PaginationResult<T>
{
    public PaginationResult(IEnumerable<T> items, PaginationResultParams resultParams)
    {
        PageIndex = resultParams.Page + 1;
        TotalPages = (int)Math.Ceiling(resultParams.Count / (double)resultParams.PerPage);
        TotalItems = resultParams.Count;
        PageItemsStartsAt = TotalItems > 0 ? (PageIndex - 1) * resultParams.PerPage + 1 : 0;
        PageItemsEndsAt = 0;

        if (TotalItems > 0)
        {
            if (PageIndex * resultParams.PerPage > TotalItems)
            {
                PageItemsEndsAt = TotalItems;
            }
            else
            {
                PageItemsEndsAt = PageIndex * resultParams.PerPage;
            }
        }

        Items = items;
    }

    /// <summary>
    /// Página atual
    /// </summary>
    public int PageIndex { get; set; }

    /// <summary>
    /// Número indicando a ultima página
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Número total de itens com base na consulta enviada
    /// </summary>
    public long TotalItems { get; set; }

    /// <summary>
    /// Número indicando o primeiro item da página
    /// </summary>
    public long PageItemsStartsAt { get; set; }

    /// <summary>
    /// Número indicando o último item da página
    /// </summary>
    public long PageItemsEndsAt { get; set; }

    /// <summary>
    /// Indicador de existência de página anterior
    /// </summary>
    public bool HasPreviousPage => PageIndex > 1;

    /// <summary>
    /// Indicador de existência de próxima página
    /// </summary>
    public bool HasNextPage => PageIndex < TotalPages;

    /// <summary>
    /// Lista de itens
    /// </summary>
    public IEnumerable<T> Items { get; set; }
}
