using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

/// <summary>
/// Represents a bank book export entity with file details and associated bank book information.
/// </summary>
[Table("BankBookExport")]
public class BankBookExportDBEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the bank book export.
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the associated bank book.
    /// </summary>
    [Required]
    public Guid BankBookId { get; set; }

    /// <summary>
    /// Gets or sets the file name of the bank book export.
    /// </summary>
    [Required]
    [MaxLength(255)]
    public required string FileName { get; set; }

    /// <summary>
    /// Gets or sets the content of the exported bank book file as a byte array.
    /// </summary>
    [Column(TypeName = "bytea")]
    public byte[] FileContent { get; set; } = [];

    /// <summary>
    /// Gets or sets the content type (MIME type) of the exported bank book file.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public required string ContentType { get; set; }

    /// <summary>
    /// Gets or sets the file-size of the bank book export.
    /// </summary>
    public long FileSize { get; set; }

    /// <summary>
    /// Gets or sets the creation date of the bank book export.
    /// </summary>
    public DateTime CreateAt { get; set; }

    /// <summary>
    /// Gets or sets the bank book associated with this export.
    /// </summary>
    [ForeignKey(nameof(BankBookId))]
    public virtual BankBookDbEntity? BankBook { get; set; }
}
