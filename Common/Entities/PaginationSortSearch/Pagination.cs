namespace Common.Entities.PaginationSortSearch;

/// <summary>
/// Represents pagination details for a paginated response.
/// </summary>
public class Pagination
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
    /// Gets or sets the total number of items.
    /// </summary>
    public int Total { get; set; }
}