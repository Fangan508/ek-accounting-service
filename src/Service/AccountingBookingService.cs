using ClosedXML.Excel;
using Common.Domain.BankBook.RequestModels;
using Common.Domain.BankBook.ResponseModels;
using Common.Domain.PaginationSortSearch;
using Common.DomainHelpers;
using Common.Errors;
using Common.Interfaces.Repositories;
using Common.Interfaces.Services;
using Common.ResultObjects;
using Microsoft.Extensions.Logging;
using Service.RequestsValidation;

namespace Service;

/// <summary>
/// Service responsible for handling accounting booking requests.
/// </summary>
public class AccountingBookingService : IAccountingBookingService
{
    private readonly IAccountingBookingRepository _accountingBookingRepository;
    private readonly IBankBookExportRepository _bankBookExportRepository;
    private readonly ILogger<AccountingBookingService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountingBookingService"/> class.
    /// Constructor for the AccountingBookingService.
    /// </summary>
    /// <param name="accountingBookingRepository">The repository used to manage accountings.</param>
    /// <param name="bankBookExportRepository">The repository used to manage bank book exports.</param>
    /// <param name="logger">The logger used for logging information and errors.</param>
    /// <exception cref="ArgumentNullException">Thrown when any of the parameters are null.</exception>"
    public AccountingBookingService(
        IAccountingBookingRepository accountingBookingRepository,
        IBankBookExportRepository bankBookExportRepository,
        ILogger<AccountingBookingService> logger)
    {
        _accountingBookingRepository = accountingBookingRepository;
        _bankBookExportRepository = bankBookExportRepository;
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

    private Task<IEnumerable<BankBookPositionCreated>> CreateBankBookPositions(IEnumerable<BankBookPositionCreateModel> bankBookPositions)
    {
        var result = bankBookPositions.Select(position => new BankBookPositionCreated 
        { 
            Id = Guid.NewGuid(), 
            SellerName = position.SellerName, 
            BookingDate = position.BookingDate, 
            Amount = position.Amount 
        }); 
        
        return Task.FromResult(result);
    }

    /// <summary>
    /// Asynchronously exports a file related to a bank book.
    /// </summary>
    /// <param name="request">The request containing the necessary information to generate the bank book export.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a <see cref="Result{T}"/>
    /// where T is a <see cref="BankBookExportModel"/> which includes details about the exported bank book file
    /// </returns>
    public async Task<Result<BankBookExportModel>> ExportBankBookAsync(BankBookExportCreateModel request)
    {
        try
        {
            var bankBookExists = await _accountingBookingRepository.BankBookExists(request.BankBookId);
            if (!bankBookExists)
            {
                return Result.Failure<BankBookExportModel>(
                    AccountingError.NotFound(
                        "ExportBankBook.NotFound", 
                        $"The bank book with id {request.BankBookId} not found."));
            }

            var bankBookExport = new BankBookExportModel
            {
                Id = request.BankBookId,
                BankBookId = request.BankBookId,
                FileName = $"Bankbuch_{request.BankBookId}.xlsx",
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                
                CreatedAt = DateTime.UtcNow
            };

            var content = await CreateBankBookAsExcel(request);
            var createdExport = await _bankBookExportRepository.CreateAsync(bankBookExport, content);

            if (_logger.IsEnabled(LogLevel.Warning))
            {
                _logger.LogWarning(
                    "Export file uploaded for bank book {@bankBookId}: {FileName} ({FileSize} bytes)",
                    createdExport.Id,
                    "Test",
                    50);
            }

            return Result.Success(createdExport);
        }
        catch (Exception ex)
        {

            if (_logger.IsEnabled(LogLevel.Error))
            {
                _logger.LogError(ex, "Failed to export bank book as Excel with request: {@Request}", request);
            }

            return Result.Failure<BankBookExportModel>(
                AccountingError.Failure("ExportBankBook.Failed", "An error occured while exporting the bank book."));
        }
    }

    private async Task<byte[]> CreateBankBookAsExcel(BankBookExportCreateModel request)
    {
        return await Task.Run(() =>
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Bank Book");

                worksheet.Cell(1, 1).Value = $"Bankbuch {request.MonthYearText}";

                worksheet.Cell(5, 1).Value = "Beleg";
                worksheet.Cell(5, 2).Value = "Datum";
                worksheet.Cell(5, 3).Value = "Konto";
                worksheet.Cell(5, 4).Value = "Text";
                worksheet.Cell(5, 5).Value = "Haben";
                worksheet.Cell(5, 6).Value = "Soll";
                worksheet.Cell(5, 7).Value = "Saldo";

                worksheet.Column(1).Width = 10.71;
                worksheet.Column(2).Width = 10.71;
                worksheet.Column(3).Width = 10.71;
                worksheet.Column(4).Width = 51;
                worksheet.Column(5).Width = 10.71;
                worksheet.Column(6).Width = 10.71;
                worksheet.Column(7).Width = 10.71;

                worksheet.Style.Font.FontName = "Arial";

                worksheet.Row(1).Style.Font.FontSize = 18;
                worksheet.Row(1).Style.Font.Bold = true;

                worksheet.Row(5).Style.Font.FontSize = 14;

                int index = 6;
                foreach (var position in request.Positions)
                {
                    worksheet.Cell(index, 2).Value = position.BookingDate;
                    worksheet.Cell(index, 2).Style.DateFormat.Format = "dd.MM.yyyy";

                    worksheet.Cell(index, 4).Value = position.Description;

                    worksheet.Cell(index, 6).Value = (decimal)position.Amount;
                    worksheet.Cell(index, 6).Style.NumberFormat.Format = "#,##0.00";

                    worksheet.Row(index).Style.Font.FontSize = 10;

                    index++;
                }

                var allDataRange = worksheet.Range(5, 1, index - 1, 7);
                allDataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                allDataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;

                var headerRange = worksheet.Range(5, 1, 5, 7);
                headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                return stream.ToArray();
            }
        });
    }
}