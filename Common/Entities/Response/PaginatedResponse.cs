using Common.Entities.PaginationSortSearch;

namespace Common.Entities.Response;

/// <summary>
/// Represents a paginated response containing a collection of items and pagination details.
/// </summary>
/// <typeparam name="T">The type of the items in the response.</typeparam>
public class PaginatedResponse<T>
{
    /// <summary>
    /// Gets or sets the collection of items in the response.
    /// </summary>
    public required IEnumerable<T> Items { get; set; }

    /// <summary>
    /// Gets or sets the pagination details for the response.
    /// </summary>
    public required Pagination Pagination { get; set; }
}