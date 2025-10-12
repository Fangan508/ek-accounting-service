using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Entities;

/// <summary>
/// Represents a account book with details such as name, and associated metadata.
/// </summary>
[Table("BankBook")]
public class BankBook
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
    /// Gets or sets the booking date.
    /// </summary>
    [Required]
    public DateTime BookingDate { get; set; }
}