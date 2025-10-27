using Common.Entities.PaginationSortSearch;
using Common.Entities.Requests;
using Common.Entities.Response;
using Common.Errors;
using Common.ResultObject;

namespace Service.RequestsValidation;

/// <summary>
/// Provides validation methods for requests related to accounting bookings.
/// </summary>
public static class ValidateRequests
{
    /// <summary>
    /// Validates the <see cref="GetBankBooksRequest"/> and returns a result indicating success or failure.
    /// </summary>
    /// <param name="request">The request to validate.</param>
    /// <returns>
    /// A <see cref="Result{TValue}"/> containing a paginated response of accounting bookings if validation succeeds, or an error if validation fails.</returns>
    public static Result<PaginatedResponse<GetBankBook>> ValidateGetBankBooksRequest(GetBankBooksRequest request)
    {
        if (request == null)
        {
            return Result.Failure<PaginatedResponse<GetBankBook>>(AccountingError.Validation("AccountingError.NullRequest", "Get bank books request cannot be null"));
        }

        if (request.BookingDate.HasValue)
        {
            DateTime date = request.BookingDate.Value;
            if (date == DateTime.MinValue || date > DateTime.UtcNow)
            {
                return Result.Failure<PaginatedResponse<GetBankBook>>(AccountingError.Validation("AccountingError.InvalidBookingBankDatae", "Invalid accounting booking date provided"));
            }
        }

        if (request.Offset < 0)
        {
            return Result.Failure<PaginatedResponse<GetBankBook>>(AccountingError.Validation("AccountingError.InvalidPageNumber", "Offset can't be less than zero"));
        }   

        if (request.Limit < 1)
        {
            return Result.Failure<PaginatedResponse<GetBankBook>>(AccountingError.Validation("AccountingError.InvalidPageSize", "Limit must be greater than 0."));
        }

       var paginatedResponse = new PaginatedResponse<GetBankBook>
        {
            Items = [],
            Pagination = new Pagination { }
        };

        return Result.Success(paginatedResponse);
    }

    /// <summary>
    /// Validates the parameters and returns a result indicating success or failure.
    /// </summary>
    /// <param name="bankBookId">The ID of the bank book to validate.</param>
    /// <param name="pagedSortedRequest">The paginated and sorted request to validate.</param>
    /// <returns><see cref="Result{TValue}"/> containing a paginated response of accounting book positions if validation succeeds, or an error if validation fails.</returns>
    public static Result<PaginatedResponse<GetBankBookPosition>> ValidateGetBankBookPositionsRequest(Guid bankBookId, PagedSortedRequest pagedSortedRequest)
    {
        if (bankBookId == Guid.Empty)
        { 
            return Result.Failure<PaginatedResponse<GetBankBookPosition>>(AccountingError.Validation("AccountingError.InvalidBankBookId", "Bank Book Id cannot be empty."));
        }

        if (pagedSortedRequest.Offset < 0)
        {
            return Result.Failure<PaginatedResponse<GetBankBookPosition>>(AccountingError.Validation("AccountingError.InvalidPageNumber", "Offset can't be less than zero"));
        }

        if (pagedSortedRequest.Limit < 1)
        {
            return Result.Failure<PaginatedResponse<GetBankBookPosition>>(AccountingError.Validation("AccountingError.InvalidPageSize", "Limit must be greater than 0."));
        }

        var response = new PaginatedResponse<GetBankBookPosition>
        {
            Items = [],
            Pagination = new Pagination { }
        };

        return Result.Success(response);
    }
}