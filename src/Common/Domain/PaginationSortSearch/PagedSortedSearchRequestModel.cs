namespace Common.Domain.PaginationSortSearch;

/// <summary>
/// Represents a request for paged, sorted, and searchable data.
/// </summary>
public class PagedSortedSearchRequestModel : PagedSortedRequestModel
{
    /// <summary>
    /// Gets or sets the search text to filter results.
    /// </summary>
    public string? SearchText { get; set; }
}