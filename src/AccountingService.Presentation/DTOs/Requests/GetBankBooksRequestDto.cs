﻿namespace AccountingService.Presentation.DTOs.Requests;

/// <summary>
/// Represents a request to get bank books with pagination, sorting, and searching capabilities.
/// </summary>
public class GetBankBooksRequestDto : PagedSortedSearchRequestDto
{
    /// <summary>
    /// Get or set the booking bank date for filtering booking banks.
    /// </summary>
    public DateTime? BookingBankDate { get; set; }
}