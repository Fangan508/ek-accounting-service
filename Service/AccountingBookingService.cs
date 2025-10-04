using Common.Entities.Requests;
using Common.Entities.Response;
using Common.Interfaces;
using Common.ResultObject;

namespace Service;

/// <summary>
/// Service responsible for handling accounting booking requests.
/// </summary>
public class AccountingBookingService : IAccountingBookingService
{
    /// <summary>
    /// Retrieves a paginated list of bank books based on the specified request criteria.
    /// </summary>
    /// <param name="getGetBankBooksRequest">The request containing filters, pagination, and sorting operations.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a <see cref="Result{T}"/>
    /// where T is a <see cref="PaginatedResponse{GetBankBook}"/> which includes the paginated list of bank books.
    /// </returns>
    public async Task<Result<PaginatedResponse<GetBankBook>>> GetBankBooks(GetBankBooksRequest getGetBankBooksRequest)
    {
        throw new NotImplementedException();
    }
}