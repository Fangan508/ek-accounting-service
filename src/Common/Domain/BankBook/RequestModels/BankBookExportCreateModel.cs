namespace Common.Domain.BankBook.RequestModels;

/// <summary>
/// Represents a request to create a bank book export.
/// </summary>
public class BankBookExportCreateModel
{
    /// <summary>
    /// Gets or sets the unique identifier for the bank book.
    /// </summary>
    public Guid BankBookId { get; set; }

    /// <summary>
    /// Gets or sets the month for which the bank book export is being created.
    /// </summary>
    public DateTime Month { get; set; }

    /// <summary>
    /// Gets or sets the text representation of the month and year for which the bank book export is being created.
    /// </summary>
    public string MonthYearText { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the collection of bank book position export models.
    /// </summary>
    public IEnumerable<BankBookPositionExportCreateModel> Positions { get; set; } = [];
}
