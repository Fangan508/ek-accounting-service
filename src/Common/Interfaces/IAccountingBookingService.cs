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
    /// A task that represents the asynchronous operation. The task result contains a <see cref="Result{TValue}"/>
    /// object wrapping a <see cref="PaginatedResponse{GetBankBook}"/> which includes the collection of bank books and metadata.
    /// </returns>
    Task<Result<PaginatedResponse<GetBankBook>>> GetBankBooks(GetBankBooksRequest getGetBankBooksRequest);

    /// <summary>
    /// Asynchronously retrieves a paginated list of bank book positions.
    /// </summary>
    /// <param name="bankBookId">The unique identifier of the bank book.</param>
    /// <param name="pagedSortedRequest">The request containing pagination and sorting details.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a <see cref="Result{TValue}"/>
    /// object wrapping a <see cref="PaginatedResponse{GetBankBookPosition}"/> which includes the collection of bank books and metadata.
    /// </returns>
    Task<Result<PaginatedResponse<GetBankBookPosition>>> GetBankBookPositions(Guid bankBookId, PagedSortedRequest pagedSortedRequest);
}