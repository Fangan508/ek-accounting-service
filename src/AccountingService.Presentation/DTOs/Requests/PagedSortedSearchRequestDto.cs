namespace AccountingService.Presentation.DTOs.Requests;

/// <summary>
/// Represents a request for paginated, sorted, and searchable data.
/// </summary>
public class PagedSortedSearchRequestDto : PagedSortedRequestDto
{
    /// <summary>
    /// Gets or sets the search text for filtering results.
    /// </summary>
    public string? SearchText { get; set; }
}