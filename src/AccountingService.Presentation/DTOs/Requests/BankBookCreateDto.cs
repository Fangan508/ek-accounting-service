using System.ComponentModel.DataAnnotations;

namespace AccountingService.Presentation.DTOs.Requests;

/// <summary>
/// Represents a request to create a bank book.
/// </summary>
public class BankBookCreateDto
{
    /// <summary>
    /// Gets or sets the name associated with the bank book.
    /// </summary>
    [Required(ErrorMessage = "Bank book name is required.")]
    [StringLength(100, ErrorMessage = "Bank book name cannot exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the booking date for the bank book.
    /// </summary>
    public DateTime BookingDate { get; set; }

    /// <summary>
    /// Gets the collection of positions to be included in the bank book.
    /// </summary>
    [Required(ErrorMessage = "Positions must be provided.")]
    [MinLength(1, ErrorMessage = "At least one position must be provided.")]
    public IEnumerable<BankBookPositionCreateDto> Positions { get; set; } = Enumerable.Empty<BankBookPositionCreateDto>();
}