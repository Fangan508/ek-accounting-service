namespace AccountingService.Presentation.DTOs.Response;

/// <summary>
/// Represents a position in a bank book, including details about the transaction, seller and date.
/// </summary>
public class BankBookPositionDto
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