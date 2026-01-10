namespace AccountingService.Presentation.DTOs.Response;

/// <summary>
/// Represents the response data for creating a bank book.
/// </summary>
public class BankBookCreatedDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the created bank book.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the message associated with the bank book creation.
    /// </summary>
    public string Message { get; set; } = string.Empty;
}