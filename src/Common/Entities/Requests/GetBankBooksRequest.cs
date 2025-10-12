namespace Common.Entities.Requests;

/// <summary>
/// Represents a request to retrieve accounting booking bank books with optional filtering by booking bank date.
/// </summary>
public class GetBankBooksRequest : PagedSortedSearchRequest
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