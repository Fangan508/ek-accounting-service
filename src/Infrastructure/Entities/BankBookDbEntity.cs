using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

/// <summary>
/// Represents a bank book entity in the database.
/// </summary>
[Table("BankBook")]
public class BankBookDbEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the bank book.
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the bank book.
    /// </summary>
    [Required]
    [StringLength(255)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the booking date of the bank book.
    /// </summary>
    public DateTime BookingDate { get; set; }

    /// <summary>
    /// Gets the collection of positions associated with the bank book.
    /// </summary>
    public ICollection<BankBookPositionDbEntity> Positions { get; } = [];
}
