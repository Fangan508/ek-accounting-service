using Common.Entities;
using Common.Entities.PaginationSortSearch;
using Common.Entities.Requests;
using Common.Entities.Response;
using Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

/// <summary>
/// Provides a concrete implementation of the <see cref="IAccountingBookingRepository"/> interface for managing <see cref="AccountingBooking"/> entities.
/// Inherits basic CRUD operations from <see cref="BaseRepository{AccountingBooking}"/>.
/// </summary>
public class AccountingBookingRepository : BaseRepository<AccountingBooking>, IAccountingBookingRepository
{
    private readonly ILogger<AccountingBookingRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountingBookingRepository"/> class with the specified <see cref="AccountingDbContext"/>.
    /// </summary>
    /// <param name="context">The database context used for data access.</param>
    /// <param name="logger">The logger instance used for logging repository operations.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public AccountingBookingRepository(AccountingDbContext context, ILogger<AccountingBookingRepository> logger) : base(context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Retrieves a paginated list of <see cref="BankBook"/> based on the specified <see cref="GetBankBooksRequest"/>.
    /// </summary>
    /// <param name="getBankBooksRequest">The request containing filtering, sorting and pagination options.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="PaginatedResponse{BankBook}"/>.</returns>
    public async Task<PaginatedResponse<GetBankBook>> GetBankBooks(GetBankBooksRequest getBankBooksRequest)
    {
        var query = _context.BankBooks.AsQueryable();

        var totalCount = await query.CountAsync();

        var items = await query
            .Select(bankbook => new GetBankBook
            {
                Id = bankbook.Id,
                Name = bankbook.Name,
                BookingDate = bankbook.BookingDate
            })
            .ToListAsync();

        return new PaginatedResponse<GetBankBook>
        {
            Items = items,
            
            Pagination = new Pagination
            {
                Page = (int)Math.Floor((double)getBankBooksRequest.Offset / getBankBooksRequest.Limit),
                PageSize = getBankBooksRequest.Limit,
                Total = totalCount
            }
        };
    }
}