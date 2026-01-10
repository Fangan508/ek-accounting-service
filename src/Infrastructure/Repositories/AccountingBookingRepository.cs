using AutoMapper;
using Common.Domain.BankBook.RequestModels;
using Common.DomainHelpers;
using Common.Entities;
using Common.Entities.PaginationSortSearch;
using Common.Entities.Requests;
using Common.Entities.Response;
using Common.Interfaces;
using Common.ResultObject;
using Common.Utilities;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

/// <summary>
/// Provides a concrete implementation of the <see cref="IAccountingBookingRepository"/> interface for managing <see cref="AccountingBooking"/> entities.
/// Inherits basic CRUD operations from <see cref="BaseRepository{AccountingBooking}"/>.
/// </summary>
public class AccountingBookingRepository : BaseRepository<AccountingBooking>, IAccountingBookingRepository
{
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountingBookingRepository"/> class with the specified <see cref="AccountingDbContext"/>.
    /// </summary>
    /// <param name="context">The database context used for data access.</param>
    /// <param name="mapper">The mapper instance used for object-object mapping.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public AccountingBookingRepository(AccountingDbContext context, IMapper mapper) : base(context)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
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

    /// <summary>
    /// Retrieves a paginated list of bank book positions for a specific bank book.
    /// </summary>
    /// <param name="bankBookId">The unique identifier of the bank book.</param>
    /// <param name="pagedSortedRequest">The request containing pagination and sorting options.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a response with paginated bank book positions.</returns>
    public async Task<PaginatedResponse<GetBankBookPosition>> GetBankBookPositions(Guid bankBookId, PagedSortedRequest pagedSortedRequest)
    {
        if (bankBookId == Guid.Empty)
        {
            return new PaginatedResponse<GetBankBookPosition>
            {
                Items = [],
                Pagination = new Pagination
                {
                    Page = 0,
                    PageSize = 0,
                    Total = 0
                }
            };
        }

        var positionQuery = _context.BankBookPositions.AsNoTracking()
            .Where(position => position.BankBookId == bankBookId);

        var groupedQuery = positionQuery
            .GroupBy(position => new
            {
                position.SellerName,
                position.BookingDate,
                position.Amount
            })
            .Select(g => new GetBankBookPosition
            {
                BookingDate = g.Key.BookingDate,
                SellerName = g.Key.SellerName,
                Amount = g.Key.Amount
            });

        var sortCriteria = pagedSortedRequest.SortCriteria;
        if (sortCriteria != null && sortCriteria.Any())
        {
            var sortFieldMappings = new Dictionary<SortField, ISortExpression<GetBankBookPosition>>
            {
                { SortField.BookingDate, new SortExpression<GetBankBookPosition, DateTime>(p => p.BookingDate) }
            };

            groupedQuery = SortPaginationHelper.ApplySorting(groupedQuery, sortCriteria, sortFieldMappings);
        }

        var totalCount = await groupedQuery.CountAsync();

        groupedQuery = SortPaginationHelper.ApplyPagination(groupedQuery, pagedSortedRequest.Offset, pagedSortedRequest.Limit);

        var items = await groupedQuery.ToListAsync();

        return new PaginatedResponse<GetBankBookPosition>
        {
            Items = items,
            Pagination = new Pagination
            {
                Page = (int)Math.Floor((double)pagedSortedRequest.Offset / pagedSortedRequest.Limit),
                PageSize = pagedSortedRequest.Limit,
                Total = totalCount
            }
        };
    }

    /// <summary>
    /// Checks if a bank book with the specified ID exists in the database.
    /// </summary>
    /// <param name="bankBookId">The unique identifier of the bank book.</param>
    /// <returns>A task that representing the asynchronous operation. The task result contains a boolean value indicating whether the bank book exists.</returns>
    public async Task<bool> BankBookExists(Guid bankBookId)
    {
        return await _context.BankBooks.AnyAsync(bankBook =>  bankBook.Id == bankBookId);
    }

    /// <summary>
    /// Creates a new bank book in the database.
    /// </summary>
    /// <param name="bankBookModel">The bank book entity to be created.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    public async Task CreateBankBook(BankBookCreated bankBookModel)
    {   
        var bankBook = _mapper.Map<BankBookDbEntity>(bankBookModel);
        await AddAsync(bankBook);
        await SaveAsync();
    }
}