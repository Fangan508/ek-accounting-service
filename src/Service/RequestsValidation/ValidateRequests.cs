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

        if (request.BookingBankDate.HasValue)
        {
            DateTime date = request.BookingBankDate.Value;
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
}