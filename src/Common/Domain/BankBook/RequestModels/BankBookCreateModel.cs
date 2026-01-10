namespace Common.Domain.BankBook.RequestModels;

/// <summary>
/// Represents a request to create a bank book.
/// </summary>
public class BankBookCreateModel
{
    /// <summary>
    /// Gets or sets the name of the bank book.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the booking date for the bank book.
    /// </summary>
    public DateTime BookingDate { get; set; }

    /// <summary>
    /// Gets the collection of positions to be included in the bank book.
    /// </summary>
    public IEnumerable<BankBookPositionModel> Positions { get; set; } = Enumerable.Empty<BankBookPositionModel>();
}
