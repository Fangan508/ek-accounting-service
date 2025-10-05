using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Entities;

/// <summary>
/// Represents an accounting booking entity with details such as ID and associated metadata.
/// </summary>
[Table("AccountingBooking")]
public class AccountingBooking
{
    /// <summary>
    /// Gets or sets the unique identifier for the accounting booking.
    /// </summary>
    [Key]
    public Guid Id { get; set; }
}