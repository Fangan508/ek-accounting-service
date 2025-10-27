using AccountingService.Presentation.DTOs;
using AccountingService.Presentation.DTOs.Requests;
using AccountingService.Presentation.DTOs.Response;
using AutoMapper;
using Common.Entities.Requests;
using Common.Infrastructure;
using Common.Interfaces;
using Common.ResultObject;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AccountingService.Presentation.Controllers
{
    /// <summary>
    /// Controller for managing accounting bookings and related operations.
    /// </summary>
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AccountingBookingController : ControllerBase
    {
        private readonly IAccountingBookingService _accountingBookingService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountingBookingController"/> class.
        /// </summary>
        /// <param name="accountingBookingService">The service used for handling accounting booking operations.</param>
        /// <param name="mapper">The mapper for transforming between domain models and DTOs.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public AccountingBookingController(IAccountingBookingService accountingBookingService, IMapper mapper)
        {
            _accountingBookingService = accountingBookingService;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Retrieves a list of bank books based on the provided search criteria.
        /// </summary>
        /// <param name="getBankBooksRequestDto">The search criteria for retrieving bank books, including offset, limit, sort options, and filters.</param>
        /// <returns>
        /// An <see cref="IResult"/> containing the results of the bank book search operation.
        /// If successful, returns a 200 OK response with a paginated list of bank books.
        /// </returns>
        /// <response code="200">Successfully retrieved the list of bank books.</response>
        /// <response code="401">Unauthorized - User not authenticated.</response>
        /// <response code="404">No bank books found for the current user.</response>
        /// <response code="500">Internal server error occurred.</response>
        [ProducesResponseType(typeof(PaginatedResponseDto<GetBankBookDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<IResult> GetBankBooks([FromQuery] GetBankBooksRequestDto getBankBooksRequestDto)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                var request = _mapper.Map<GetBankBooksRequest>(getBankBooksRequestDto);
                var result = await _accountingBookingService.GetBankBooks(request);

                if (result.IsFailure)
                {
                    ApplicationDiagnostics.RecordError(result.Error.Name, result.Error.Code);
                    return result.ToProblemDetails();
                }

                var response = _mapper.Map<PaginatedResponseDto<GetBankBookDto>>(result.Value);

                ApplicationDiagnostics.RecordBusinessOperation("Success", response.Pagination.Total);

                return Results.Ok(response);
            }
            catch (AutoMapperMappingException ex)
            {
                ApplicationDiagnostics.RecordError("MappingError", ex.Message);
                return Results.Problem("Mapping failure.", statusCode: StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                ApplicationDiagnostics.RecordError("UnhandledException", ex.Message);
                return Results.Problem("An unexpected error occurred.", statusCode: StatusCodes.Status500InternalServerError);
            }
            finally
            {
                stopwatch.Stop();
                
                ApplicationDiagnostics.RecordHttpRequest(
                    HttpContext.Request.Method,
                    HttpContext.Request.Path,
                    HttpContext.Response.StatusCode,
                    stopwatch.Elapsed.TotalSeconds);
            }
        }

        /// <summary>
        /// Retrieves the positions of a specific bank book.
        /// </summary>
        /// <param name="bankBookId">The unique identifier of the bank book.</param>
        /// <param name="pagedSortedRequestDto">The pagination, sorting, and search criteria for retrieving bank book positions.</param>
        /// <returns>
        /// An <see cref="IResult"/> containing the result of the operation.
        /// If successful, returns a 200 status code with a paginated list of bank book positions.
        /// </returns>
        /// <response code="200">Successfully retrieved the positions the bank book.</response>
        /// <response code="401">Unauthorized - User not authenticated.</response>
        /// <response code="404">No positions found for the specified bank book.</response>
        /// <response code="500">Internal server error occurred.</response>
        [ProducesResponseType(typeof(PaginatedResponseDto<GetBankBookPositionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{bankBookId}")]
        public async Task<IResult> GetBankBookPositions([FromRoute] Guid bankBookId, [FromQuery] PagedSortedRequestDto pagedSortedRequestDto)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                var pagedSortedRequest = _mapper.Map<PagedSortedRequest>(pagedSortedRequestDto);
                var result = await _accountingBookingService.GetBankBookPositions(bankBookId, pagedSortedRequest);

                if (result.IsFailure)
                {
                    ApplicationDiagnostics.RecordError(result.Error.Name, result.Error.Code);
                    return result.ToProblemDetails();
                }

                var response = _mapper.Map<PaginatedResponseDto<GetBankBookPositionDto>>(result.Value);

                ApplicationDiagnostics.RecordBusinessOperation("Success", response.Pagination.Total);

                return Results.Ok(response);
            }
            catch (AutoMapperMappingException ex)
            {
                ApplicationDiagnostics.RecordError("MappingError", ex.Message);
                return Results.Problem("Mapping failure.", statusCode: StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                ApplicationDiagnostics.RecordError("UnhandledException", ex.Message);
                return Results.Problem("An unexpected error occurred.", statusCode: StatusCodes.Status500InternalServerError);
            }
            finally
            {
                stopwatch.Stop();
                ApplicationDiagnostics.RecordHttpRequest(
                    HttpContext.Request.Method,
                    HttpContext.Request.Path,
                    HttpContext.Response.StatusCode,
                    stopwatch.Elapsed.TotalSeconds);
            }
        }
    }
}