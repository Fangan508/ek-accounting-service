using Common.Entities.PaginationSortSearch;

namespace Common.Entities.Requests;

/// <summary>
/// Represents a request with pagination and sorting capabilities.
/// </summary>
public class PagedSortedRequest
{
    /// <summary>
    /// Gets or sets the offset for pagination.
    /// </summary>
    public int Offset { get; set; }

    /// <summary>
    /// Gets or sets the limit for pagination. Default is 50.
    /// </summary>
    public int Limit { get; set; } = 50;

    /// <summary>
    /// Gets or sets the sorting criteria for the request.
    /// </summary>
    public IEnumerable<SortCriteria>? SortCriteria { get; set; }
}