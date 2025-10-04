namespace Common.Entities.PaginationSortSearch;

/// <summary>
/// Specifies the fields that can be used for sorting across different entity types.
/// </summary>
public enum SortField
{
    /// <summary>
    /// Sort by the name of the entity.
    /// </summary>
    Name,

    /// <summary>
    /// Sort by the booking date date of the entity. 
    /// </summary>
    BookingDate
}