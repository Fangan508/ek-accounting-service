using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Entities;

/// <summary>
/// Represents a bank book position
/// </summary>
[Table("BankBookPosition")]
public class BankBookPosition
{
    /// <summary>
    /// Gets or sets the unique identifier for the bank book position.
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the bank book associated with this position.
    /// </summary>
    [Required]
    public Guid BankBookId { get; set; }

    /// <summary>
    /// Gets or sets the booking date of the bank book position.
    /// </summary>
    [Required]
    public DateTime BookingDate { get; set; }

    /// <summary>
    /// Gets or sets the name of the seller or vendor from whom the item was purchased.
    /// </summary>
    [Required]
    public string SellerName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the monetary amount associated with this position.
    /// </summary>
    [Required]
    public decimal Amount { get; set; }
}