namespace Common.Domain.BankBook.ResponseModels;

/// <summary>
/// Represents a response containing paginated bank book positions.
/// </summary>
public class BankBookPositionModel
{
    /// <summary>
    /// Gets or sets the booking date of the bank book position.
    /// </summary>
    public DateTime BookingDate { get; set; }

    /// <summary>
    /// Gets or sets the name of the seller or vendor from whom the item was purchased.
    /// </summary>
    public required string SellerName { get; set; }

    /// <summary>
    /// Gets or sets the monetary amount associated with this position.
    /// </summary>
    public decimal Amount { get; set; }
}