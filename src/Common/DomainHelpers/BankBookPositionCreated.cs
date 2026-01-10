namespace Common.DomainHelpers;

/// <summary>
/// Represents the creation of a bank book position.
/// </summary>
public class BankBookPositionCreated
{
    /// <summary>
    /// Gets or sets the unique identifier for the bank book position.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the booking date of the bank book position.
    /// </summary>
    public DateTime BookingDate { get; set; }

    /// <summary>
    /// Gets or sets the name of the seller or vendor from whom the item was purchased.
    /// </summary>
    public string? SellerName { get; set; }

    /// <summary>
    /// Gets or sets the monetary amount associated with this position.
    /// </summary>
    public decimal Amount { get; set; }
}