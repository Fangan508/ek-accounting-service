namespace AccountingService.Presentation.DTOs;

/// <summary>
/// Represents pagination details for paginated responses.
/// </summary>
public class PaginationDto
{
    /// <summary>
    /// Gets or sets the current page number.
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Gets or sets the number of items per page.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Gets or sets the total number of items available.
    /// </summary>
    public int Total { get; set; }
}