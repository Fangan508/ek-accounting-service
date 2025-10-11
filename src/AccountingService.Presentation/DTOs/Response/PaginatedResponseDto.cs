namespace AccountingService.Presentation.DTOs.Response;

/// <summary>
/// Represents a paginated response containing a collection of items and pagination details.
/// </summary>
/// <typeparam name="T"></typeparam>
public class PaginatedResponseDto<T>
{
    /// <summary>
    /// Gets or sets the collection of items in the current page.
    /// </summary>
    public required IEnumerable<T> Items { get; set; }

    /// <summary>
    /// Gets or sets the pagination details for the response.
    /// </summary>
    public required PaginationDto Pagination { get; set; }
}