using Common.Domain.BankBook.RequestModels;
using Common.Domain.BankBook.ResponseModels;
using Common.Domain.PaginationSortSearch;
using Common.DomainHelpers;
using Common.Errors;
using Common.Interfaces.Repositories;
using Common.Interfaces.Services;
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
    /// where T is a <see cref="PaginatedResponseModel{GetBankBook}"/> which includes the paginated list of bank books.
    /// </returns>
    public async Task<Result<PaginatedResponseModel<BankBookModel>>> GetBankBooks(BankBookQueryModel getBankBooksRequest)
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

            return Result.Failure<PaginatedResponseModel<BankBookModel>>(AccountingError.Failure("GetBankBooks.Failed", ex.Message));
        }
    }

    /// <summary>
    /// Retrieves a paginated list of bank book positions based on the specified bank book ID.
    /// </summary>
    /// <param name="bankBookId">The unique identifier of the bank book.</param>
    /// <param name="pagedSortedRequest">The request containing pagination and sorting details.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a <see cref="Result{T}"/>
    /// where T is a <see cref="PaginatedResponseModel{GetBankBookPositions}"/> which includes the paginated list of bank book positions.
    /// </returns>
    public async Task<Result<PaginatedResponseModel<BankBookPositionModel>>> GetBankBookPositions(Guid bankBookId, PagedSortedRequestModel pagedSortedRequest)
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
                return Result.Failure<PaginatedResponseModel<BankBookPositionModel>>(AccountingError.NotFound("GetBankBookPositions.Failed", $"The bank book with id {bankBookId}, was not found."));
            }

            var response = await _accountingBookingRepository.GetBankBookPositions(bankBookId, pagedSortedRequest);
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to get bank book positions for bank book id: {@id}", bankBookId);
            return Result.Failure<PaginatedResponseModel<BankBookPositionModel>>(AccountingError.Failure("GetBankBookPositions.Failed", ex.Message));
        }
    }

    /// <summary>
    /// Creates a new bank book with the specified details.
    /// </summary>
    /// <param name="request">The details of the bank book to be created.</param>
    /// <returns>A result indicating success with the created bank book ID, or failure with error details.</returns>
    public async Task<Result<Guid>> CreateBankBook(BankBookCreateModel request)
    {
        try
        {
            var validationResult = ValidateRequests.ValidateBankBookCreateRequest(request);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }   

            var bankBook = await BuildBankBookFromRequest(request);
            await _accountingBookingRepository.CreateBankBook(bankBook);

            return Result.Success(bankBook.Id);
        }
        catch (Exception ex)
        {

            _logger?.LogError(ex, "Failed to create bank book with request: {@Request}", request);
            return Result.Failure<Guid>(AccountingError.Failure("CreateBankBook.Failed", ex.Message));
        }
    }

    private async Task<BankBookCreated> BuildBankBookFromRequest(BankBookCreateModel request)
    {
        var bankBook = new BankBookCreated
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            BookingDate = request.BookingDate ?? DateTime.UtcNow
        };

        if (request.Positions.Any())
        {
            var bankBookPositions = await CreateBankBookPositions(request.Positions);

            foreach (var position in bankBookPositions)
            {
                bankBook.Positions.Add(position);
            }
        }

        return bankBook;
    }


    private async Task<IEnumerable<BankBookPositionCreated>> CreateBankBookPositions(IEnumerable<BankBookPositionCreateModel> bankBookPositions)
    {
        return bankBookPositions.Select(position => new BankBookPositionCreated
        {
            Id = Guid.NewGuid(),
            SellerName = position.SellerName,
            BookingDate = position.BookingDate,
            Amount = position.Amount
        });
    }
}