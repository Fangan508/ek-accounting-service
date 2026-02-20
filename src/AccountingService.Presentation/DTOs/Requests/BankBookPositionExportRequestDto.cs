namespace AccountingService.Presentation.DTOs.Requests;

/// <summary>
/// Represents a request to export a bank book position.
/// </summary>
public class BankBookPositionExportRequestDto 
{
    /// <summary>
    /// Gets or sets the date of the booking.
    /// </summary>
    public DateTime BookingDate { get; set; }

    /// <summary>
    /// Gets or sets a textual description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the monetary amount.
    /// </summary>
    public decimal Amount { get; set; }
}
