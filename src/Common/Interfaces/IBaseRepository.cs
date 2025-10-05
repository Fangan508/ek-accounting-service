namespace Common.Interfaces;

/// <summary>
/// Defines the basic contract for a generic repository to manage data access operations for a specific entity type.
/// </summary>
/// <typeparam name="T">The type of the entity that the repository will manage. Must be a reference type.</typeparam>
public interface IBaseRepository<T> 
    where T : class
{
    /// <summary>
    /// Asynchronously adds a new entity to the data store.
    /// </summary>
    /// <param name="entity">The entity instance to add.</param>
    /// <returns>A task that represents the asynchronous add operation.</returns>
    Task AddAsync(T entity);

    /// <summary>
    /// Asynchronously checks whether an entity with the specified unique identifier exists in the data store.
    /// </summary>
    /// <param name="entityId">The unique identifier of the entity.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains <c>true</c> if the entity exists; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> Exists(Guid entityId);

    /// <summary>
    /// Asynchronously saves all changes made in the context to the underlying data store.
    /// </summary>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    Task SaveAsync();
}