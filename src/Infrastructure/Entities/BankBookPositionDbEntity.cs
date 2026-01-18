using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

/// <summary>
/// Represents a bank book position entity in the database.
/// </summary>
[Table("BankBookPosition")]
public class BankBookPositionDbEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the bank book position.
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the associated bank book.
    /// </summary>
    public Guid BankBookId { get; set; }

    /// <summary>
    /// Gets or sets the bank book associated with this position.
    /// </summary>
    [ForeignKey("BankBookId")]
    public virtual BankBookDbEntity BankBook { get; set; } = null!;

    /// <summary>
    /// Gets or sets the booking date of the bank book position.
    /// </summary>
    public DateTime BookingDate { get; set; }

    /// <summary>
    /// Gets or sets the name of the seller or vendor from whom the item was purchased.
    /// </summary>
    public string SellerName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the monetary amount associated with this position.
    /// </summary>
    public decimal Amount { get; set; }
}