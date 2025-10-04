using Common.Entities;
using Common.Entities.Requests;
using Common.Entities.Response;

namespace Common.Interfaces;

/// <summary>
/// Represents a repository for managing <see cref="AccountingBooking"/> entities.
/// </summary>
public interface IAccountingBookingRepository : IBaseRepository<AccountingBooking>
{
    /// <summary>
    /// Retrieves a paginated list of bank books based on the specified request parameters.
    /// </summary>
    /// <param name="getBankBooksRequest">The request containing filtering and pagination options.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a paginated of bank books.</returns>
    Task<PaginatedResponse<GetBankBook>> GetBankBooks(GetBankBooksRequest getBankBooksRequest);
}
