using Common.Entities.Requests;
using Common.Entities.Response;
using Common.Errors;
using Common.Interfaces;
using Common.ResultObject;
using Microsoft.Extensions.Logging;
using Service.RequestsValidation;

namespace Service;

/// <summary>
/// Service responsible for handling accounting booking requests.
/// </summary>
public class AccountingBookingService : IAccountingBookingService
{
    private readonly IAccountingBookingRepository _accountingBookingRepository;
    private readonly ILogger<AccountingBookingService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountingBookingService"/> class.
    /// Constructor for the AccountingBookingService.
    /// </summary>
    /// <param name="accountingBookingRepository">The repository used to manage accountings.</param>
    /// <param name="logger">The logger used for logging information and errors.</param>
    /// <exception cref="ArgumentNullException">Thrown when any of the parameters are null.</exception>"
    public AccountingBookingService(
        IAccountingBookingRepository accountingBookingRepository,
        ILogger<AccountingBookingService> logger)
    {
        _accountingBookingRepository = accountingBookingRepository;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves a paginated list of bank books based on the specified request criteria.
    /// </summary>
    /// <param name="getBankBooksRequest">The request containing filters, pagination, and sorting operations.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a <see cref="Result{T}"/>
    /// where T is a <see cref="PaginatedResponse{GetBankBook}"/> which includes the paginated list of bank books.
    /// </returns>
    public async Task<Result<PaginatedResponse<GetBankBook>>> GetBankBooks(GetBankBooksRequest getBankBooksRequest)
    {
        try
        {
            var validationResult = ValidateRequests.ValidateGetBankBooksRequest(getBankBooksRequest);

            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            var response = await _accountingBookingRepository.GetBankBooks(getBankBooksRequest);
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to get bank books with request: {@Request}", getBankBooksRequest);

            return Result.Failure<PaginatedResponse<GetBankBook>>(AccountingError.Failure("GetBankBooks.Failed", ex.Message));
        }
    }

    /// <summary>
    /// Retrieves a paginated list of bank book positions based on the specified bank book ID.
    /// </summary>
    /// <param name="bankBookId">The unique identifier of the bank book.</param>
    /// <param name="pagedSortedRequest">The request containing pagination and sorting details.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a <see cref="Result{T}"/>
    /// where T is a <see cref="PaginatedResponse{GetBankBookPositions}"/> which includes the paginated list of bank book positions.
    /// </returns>
    public async Task<Result<PaginatedResponse<GetBankBookPosition>>> GetBankBookPositions(Guid bankBookId, PagedSortedRequest pagedSortedRequest)
    {
        try
        {
            var validationResult = ValidateRequests.ValidateGetBankBookPositionsRequest(bankBookId, pagedSortedRequest);
            if (validationResult.IsFailure)
            { 
                return validationResult; 
            }

            var exists = await _accountingBookingRepository.BankBookExists(bankBookId);
            if (!exists) 
            {
                return Result.Failure<PaginatedResponse<GetBankBookPosition>>(AccountingError.NotFound("GetBankBookPositions.Failed", $"The bank book with id {bankBookId}, was not found."));
            }

            var response = await _accountingBookingRepository.GetBankBookPositions(bankBookId, pagedSortedRequest);
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to get bank book positions for bank book id: {@id}", bankBookId);

            return Result.Failure<PaginatedResponse<GetBankBookPosition>>(AccountingError.Failure("GetBankBookPositions.Failed", ex.Message));
        }
    }
}