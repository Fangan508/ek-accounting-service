namespace AccountingService.Presentation.DTOs.Requests;

/// <summary>
/// Represents a position in a bank book.
/// </summary>
public class BankBookPositionCreateDto
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