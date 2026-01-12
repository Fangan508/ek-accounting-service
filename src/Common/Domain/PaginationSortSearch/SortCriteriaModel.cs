using Common.Enums;

namespace Common.Domain.PaginationSortSearch;

/// <summary>
/// Represents the criteria for sorting data, including the field to sort by and the order of sorting.
/// </summary>
public class SortCriteriaModel
{
    /// <summary>
    /// Gets or sets the field to sort by.
    /// </summary>
    public SortField Field { get; set; }

    /// <summary>
    /// Gets or sets the order of sorting, either ascending or descending.
    /// </summary>
    public SortOrder Order { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SortCriteriaModel"/> class with the specified field and sort order.
    /// </summary>
    /// <param name="field">The field to sort by.</param>
    /// <param name="order">The order in which to sort the field, either ascending or descending.</param>
    public SortCriteriaModel(SortField field, SortOrder order)
    {
        Field = field;
        Order = order;
    }
}