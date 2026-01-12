using Common.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

/// <summary>
/// Provides a generic implementation of the <see cref="IBaseRepository{T}"/> interface using Entity Framework Core.
/// Enacapsulates basic data access operations for any entity type.
/// </summary>
/// <typeparam name="T">The type of the entity. Must be a reference type.</typeparam>
public class BaseRepository<T> : IBaseRepository<T>
    where T : class
{
    /// <summary>
    /// The database context used for data operations.
    /// </summary>
    internal readonly AccountingDbContext _context;

    /// <summary>
    /// The <see cref="DbSet{TEntity}"/> representing the entity collection in the context.
    /// </summary>
    private readonly DbSet<T> _dbSet;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseRepository{T}"/> class with the specified database context.
    /// </summary>
    /// <param name="context"></param>
    public BaseRepository(AccountingDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    /// <summary>
    /// Asynchronously adds a new entity to the data store.
    /// </summary>
    /// <param name="entity">The entity instance to add.</param>
    /// <returns>A task that represents the asynchronous add operation.</returns>
    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    /// <summary>
    /// Asynchronously saves all changes made in the context to the underlying data store.
    /// </summary>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Asynchronously checks whether an entity with the specified unique identifier exists in the data store.
    /// </summary>
    /// <param name="entityId">The unique identifier of the entity.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains <c>true</c> if the entity exists; otherwise, <c>false</c>.</returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<bool> Exists(Guid entityId)
    {
        return await _dbSet.FindAsync(entityId) != null;
    }
}