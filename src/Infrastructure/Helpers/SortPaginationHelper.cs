using Common.Domain.PaginationSortSearch;
using Common.Enums;
using Common.Interfaces;

namespace Infrastructure.Helpers;

/// <summary>
/// Provides helper methods for applying sorting and pagination to queries.
/// </summary>
public static class SortPaginationHelper
{
    /// <summary>
    /// Applies sorting to the given query based on the specified sort criteria and field mappings.
    /// </summary>
    /// <typeparam name="T">The type of the entity being queried.</typeparam>
    /// <param name="query">The IQueryable to apply sorting to.</param>
    /// <param name="sortCriteria">A list of sort criteria specifying the fields and sort order.</param>
    /// <param name="fieldMappings">A dictionary mapping sort fields to sort expressions.</param>
    /// <returns>The sorted IQueryable.</returns>
    public static IQueryable<T> ApplySorting<T>(
        IQueryable<T> query,
        IEnumerable<SortCriteriaModel> sortCriteria,
        Dictionary<SortField, ISortExpression<T>> fieldMappings)
    {
        IOrderedQueryable<T>? orderedQuery = null;
        var sortCriteriaList = sortCriteria?.ToList() ?? [];

        for (var i = 0; i < sortCriteriaList.Count; i++)
        {
            var criteria = sortCriteriaList[i];

            if (!fieldMappings.TryGetValue(criteria.Field, out var sortExpression))
            {
                continue;
            }

            if (i == 0)
            {
                orderedQuery = sortExpression.ApplyOrderBy(query, criteria.Order == SortOrder.Ascending);
            }
            else
            {
                if (orderedQuery != null)
                {
                    orderedQuery = sortExpression.ApplyThenBy(orderedQuery, criteria.Order == SortOrder.Ascending);
                }
            }
        }

        return orderedQuery ?? query;
    }

    /// <summary>
    /// Applies pagination to the given query by skipping a specified number of items and taking a specified number of items.
    /// </summary>
    /// <typeparam name="T">The type of the entity being queried.</typeparam>
    /// <param name="query">The IQueryable to apply pagination to.</param>
    /// <param name="offset">The number of items to skip.</param>
    /// <param name="limit">The maximum number of items to take.</param>
    /// <returns>The paginated IQueryable.</returns>
    public static IQueryable<T> ApplyPagination<T>(IQueryable<T> query, int offset, int limit)
    {
        if (offset < 0)
        {
            offset = 0;
        }

        if (limit < 0)
        {
            limit = 0;
        }

        return query.Skip(offset).Take(limit);
    }
}