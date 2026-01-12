namespace Common.Domain.BankBook.ResponseModels;

/// <summary>
/// Represents a response containing details of an bank book.
/// </summary>
public class BankBookModel
{
    /// <summary>
    /// Gets or sets the unique identifier for the bank book.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the bank book.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the booking date of the bank book.
    /// </summary>
    public DateTime BookingDate { get; set; }
}