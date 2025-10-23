using Haviliar.Domain.Pagination.Enums;
using System.Linq.Expressions;
using System.Reflection;

namespace Haviliar.Domain.Pagination.Entities;

public static class PaginationSort
{
    public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, PaginationFilter filter)
    {
        if (filter.Order.Any() && filter.Sort.Any())
        {
            filter.Sort = filter.Sort.Select(s => s.ToLower()).ToList();
            PropertyInfo[] properties = typeof(T).GetProperties();
            var propertiesToSort = properties.Where(p => filter.Sort!.Contains(p.Name.ToLower())).ToList();
            var orders = filter.Order?.ToList();

            for (int i = 0; i < propertiesToSort.Count; i++)
            {
                var property = propertiesToSort[i];
                var isDescending = orders is not null &&
                    orders[i] == OrderDirectionEnum.Desc;

                query = ApplyOrder(query, property.Name, i == 0, isDescending);
            }
        }

        return query;
    }

    private static IQueryable<T> ApplyOrder<T>(IQueryable<T> query, string propertyName, bool isFirst, bool isDescenting = false)
    {
        var parameter = Expression.Parameter(typeof(T), "p");
        var property = Expression.Property(parameter, propertyName);
        var lambda = Expression.Lambda(property, parameter);

        string methodName = isFirst ?
            isDescenting ? "OrderByDescending" : "OrderBy" :
            isDescenting ? "ThenByDescending" : "ThenBy";

        var result = typeof(Queryable).GetMethods()
            .First(method => method.Name == methodName && method.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(T), property.Type)
            .Invoke(null, [query, lambda]);

        return (IQueryable<T>)result!;
    }
}
