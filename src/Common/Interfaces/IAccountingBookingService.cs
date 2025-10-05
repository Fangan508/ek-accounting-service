using Common.Entities.Requests;
using Common.Entities.Response;
using Common.ResultObject;

namespace Common.Interfaces;

/// <summary>
/// Defines the contract for getting accounting booking.
/// </summary>
public interface IAccountingBookingService
{
    /// <summary>
    /// Asynchronously retrieves a paginated list of bank books.
    /// </summary>
    /// <param name="getGetBankBooksRequest">The request containing pagination and filtering details.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a <see cref="Result{T}"/>
    /// object wrapping a <see cref="PaginatedResponse{GetBankBook}"/> which includes the collection of bank books and metadata.
    /// </returns>
    Task<Result<PaginatedResponse<GetBankBook>>> GetBankBooks(GetBankBooksRequest getGetBankBooksRequest);
}