using Common.Domain.BankBook.RequestModels;
using Common.Domain.BankBook.ResponseModels;
using Common.Domain.PaginationSortSearch;
using Common.DomainHelpers;

namespace Common.Interfaces.Repositories;

/// <summary>
/// Represents a repository for managing BankBook entities.
/// </summary>
public interface IAccountingBookingRepository
{
    /// <summary>
    /// Retrieves a paginated list of bank books based on the specified request parameters.
    /// </summary>
    /// <param name="getBankBooksRequest">The request containing filtering and pagination options.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a paginated of bank books.</returns>
    Task<PaginatedResponseModel<BankBookModel>> GetBankBooks(BankBookQueryModel getBankBooksRequest);

    /// <summary>
    /// Retrieves a paginated list of bank book positions for a specific bank book.
    /// </summary>
    /// <param name="bankBookId">The unique identifier of the bank book.</param>
    /// <param name="pagedSortedRequest">The request containing pagination and sorting options.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a response with paginated bank book positions.</returns>
    Task<PaginatedResponseModel<BankBookPositionModel>> GetBankBookPositions(Guid bankBookId, PagedSortedRequestModel pagedSortedRequest);

    /// <summary>
    /// Checks if a bank book exists in the repository.
    /// </summary>
    /// <param name="bankBookId">The unique identifier of the bank book.</param>
    /// <returns>A task that represents the asynchronous operation. The task result indicates whether the bank book exists.</returns>
    Task<bool> BankBookExists(Guid bankBookId);

    /// <summary>
    /// Creates a new bank book in the database.
    /// </summary>
    /// <param name="bankBookModel">The bank book entity to be created.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    Task CreateBankBook(BankBookCreated bankBookModel);
}