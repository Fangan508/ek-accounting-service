namespace Common.DomainHelpers;

/// <summary>
/// Represents a bank book entity with details such as name, booking date, and associated positions.
/// </summary>
public class BankBookCreated
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

    /// <summary>
    /// Gets the collection of positions associated with the bank book.
    /// </summary>
    public virtual ICollection<BankBookPositionCreated> Positions { get; } = [];
}