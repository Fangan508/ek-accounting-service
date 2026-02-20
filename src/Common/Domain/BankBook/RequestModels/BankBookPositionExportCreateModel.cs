namespace Common.Domain.BankBook.RequestModels;

/// <summary>
/// Represents a bank book position entry for download, including booking date, description, and amount.
/// </summary>
public class BankBookPositionExportCreateModel
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