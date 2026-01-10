namespace Common.Domain.BankBook.RequestModels;

/// <summary>
/// Represents a position in a bank book.
/// </summary>
public class BankBookPositionModel
{
    /// <summary>
    /// Gets or sets the unique identifier for the bank book position.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the booking date of the bank book position.
    /// </summary>
    public DateTime? BookingDate { get; set; }

    /// <summary>
    /// Gets or sets the name of the seller or vendor from whom the item was purchased.
    /// </summary>
    public string SellerName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the monetary amount associated with this position.
    /// </summary>
    public decimal Amount { get; set; }
}
