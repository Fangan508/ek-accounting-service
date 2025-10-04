namespace Common.Entities.Requests;

/// <summary>
/// Represents a request for paged, sorted, and searchable data.
/// </summary>
public class PagedSortedSearchRequest : PagedSortedRequest
{
    /// <summary>
    /// Gets or sets the search text to filter results.
    /// </summary>
    public string? SearchText { get; set; }
}