namespace AccountingService.Presentation.DTOs.Requests;

/// <summary>
/// Represents a request for paginated, sorted, and searchable data.
/// </summary>
public class PagedSortedSearchRequestDto
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
    /// Gets or sets the fields to sort the results by.
    /// </summary>
    public IEnumerable<string>? OrderBy { get; set; }
}