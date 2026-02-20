namespace AccountingService.Presentation.DTOs.Response;

/// <summary>
/// Data transfer object representing an exported bank book file.
/// </summary>
public class BankBookExportDto
{
    /// <summary>
    /// Unique identifier for this export record.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Identifier of the associated bank book that this export record corresponds to.
    /// </summary>
    public Guid BankBookId { get; set; }

    /// <summary>
    /// Name of the exported file.
    /// </summary>
    public required string FileName { get; set; }

    /// <summary>
    /// MIME type of the exported file, indicating the format of the content (e.g., "application/pdf", "text/csv").
    /// </summary>
    public required string ContentType { get; set; }

    /// <summary>
    /// Size of the file in bytes.
    /// </summary>
    public long FileSize { get; set; }

    /// <summary>
    /// Date and time when the export record was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}