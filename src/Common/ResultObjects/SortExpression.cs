using Common.Interfaces;
using System.Linq.Expressions;

namespace Common.ResultObject;

/// <summary>
/// Represents a sorting expression for a specific type and key.
/// </summary>
/// <typeparam name="T">The type of the elements in the query.</typeparam>
/// <typeparam name="TKey">The type of the key used for sorting.</typeparam>
public class SortExpression<T, TKey> : ISortExpression<T>
{
    private readonly Expression<Func<T, TKey>> _expression;

    /// <summary>
    /// Initializes a new instance of the <see cref="SortExpression{T, TKey}"/> class.
    /// </summary>
    /// <param name="expression">The sorting expression.</param>
    public SortExpression(Expression<Func<T, TKey>> expression)
    {
        _expression = expression;
    }

    /// <summary>
    /// Applies an OrderBy or OrderByDescending operation to the query based on the sorting expression.
    /// </summary>
    /// <param name="query">The query to apply the sorting to.</param>
    /// <param name="ascending">Indicates whether to sort in ascending order.</param>
    /// <returns>An ordered queryable with the applied sorting.</returns>
    public IOrderedQueryable<T> ApplyOrderBy(IQueryable<T> query, bool ascending)
    {
        return ascending ? query.OrderBy(_expression) : query.OrderByDescending(_expression);
    }

    /// <summary>
    /// Applies a ThenBy or ThenByDescending operation to the query based on the sorting expression.
    /// </summary>
    /// <param name="query">The ordered query to apply the additional sorting to.</param>
    /// <param name="ascending">Indicates whether to sort in ascending order.</param>
    /// <returns>An ordered queryable with the applied additional sorting.</returns>
    public IOrderedQueryable<T> ApplyThenBy(IOrderedQueryable<T> query, bool ascending)
    {
        return ascending ? query.ThenBy(_expression) : query.ThenByDescending(_expression);
    }
}
