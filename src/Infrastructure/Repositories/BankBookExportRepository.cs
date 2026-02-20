using AutoMapper;
using Common.Domain.BankBook.ResponseModels;
using Common.Interfaces.Repositories;
using Infrastructure.Entities;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

/// <summary>
/// Repository for managing export operations.
/// Handles persistence of bank book export metadata and file content.
/// </summary>
public class BankBookExportRepository : BaseRepository<BankBookExportDBEntity>, IBankBookExportRepository
{
    private readonly IMapper _mapper;
    private readonly ILogger<BankBookExportRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="BankBookExportRepository"/> class.
    /// </summary>
    /// <param name="context">The database context used for data access.</param>
    /// <param name="mapper">The AutoMapper instance for entity/model mapping.</param>
    /// <param name="logger">The logger instance for logging repository operations.</param>
    public BankBookExportRepository(AccountingDbContext context, IMapper mapper, ILogger<BankBookExportRepository> logger)
        : base(context)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Creates a new bank book export record including binary file content.
    /// </summary>
    /// <param name="bankBookExportModel">The export metadata model to persist.</param>
    /// <param name="fileContent">The binary file content to associate with the export record.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<BankBookExportModel> CreateAsync(BankBookExportModel bankBookExportModel, byte[] fileContent, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(bankBookExportModel);

        if (fileContent == null || fileContent.Length == 0)
        {
            throw new ArgumentException("File content cannot be null or empty.", nameof(fileContent));
        }

        try
        {
            var entity = _mapper.Map<BankBookExportDBEntity>(bankBookExportModel);
            entity.FileContent = fileContent;

            await _context.BankBookExports.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation(
                    "Created export record {ExportId} with file ({FileSize} bytes) for bank book {BankBookId}", 
                    entity.Id,
                    fileContent.Length,
                    entity.BankBookId);
            }

            return _mapper.Map<BankBookExportModel>(entity);
        }
        catch (Exception ex)
        {
            if (_logger.IsEnabled(LogLevel.Error))
            {
                _logger.LogError(
                    ex,
                    "Error creating export record for bank book {BankBookId} with file ({FileSize} bytes)",
                    bankBookExportModel.BankBookId,
                    fileContent.Length);
            }

            throw;
        }
    }
}
