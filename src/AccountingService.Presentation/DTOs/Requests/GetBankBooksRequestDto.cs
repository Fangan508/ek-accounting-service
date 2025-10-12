namespace AccountingService.Presentation.DTOs.Requests;

/// <summary>
/// Represents a request to get bank books with pagination, sorting, and searching capabilities.
/// </summary>
public class GetBankBooksRequestDto : PagedSortedSearchRequestDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the bank book.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Get or set the booking bank date for filtering booking banks.
    /// </summary>
    public DateTime? BookingDate { get; set; }
}