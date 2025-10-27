namespace Common.Interfaces;

/// <summary>
/// Represents a sorting expression that can be applied to queryable collection of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of the elements in the queryable collection.</typeparam>
public interface ISortExpression<T>
{
    /// <summary>
    /// Applies an inital ordering to the queryable collection.
    /// </summary>
    /// <param name="query">The queryable collection to apply the ordering to.</param>
    /// <param name="ascending">A value indicating whether the ordering should be ascending.</param>
    /// <returns>A <see cref="IOrderedQueryable{T}"/> with the applied ordering.</returns>
    IOrderedQueryable<T> ApplyOrderBy(IQueryable<T> query, bool ascending);

    /// <summary>
    /// Applies an additional ordering to an already ordered queryable collection.
    /// </summary>
    /// <param name="query">The ordered queryable collection to apply the additional ordering to.</param>
    /// <param name="ascending">A value indicating whether the ordering should be ascending.</param>
    /// <returns>A <see cref="IOrderedQueryable{T}"/> with the applied additional ordering.</returns>
    IOrderedQueryable<T> ApplyThenBy(IOrderedQueryable<T> query, bool ascending);
}