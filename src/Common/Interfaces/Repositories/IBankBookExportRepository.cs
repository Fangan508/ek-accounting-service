using Common.Domain.BankBook.ResponseModels;

namespace Common.Interfaces.Repositories;

/// <summary>
/// Repository interface for managing bank book exports.
/// </summary>
public interface IBankBookExportRepository
{
    /// <summary>
    /// Creates a new bank book export record.
    /// </summary>
    /// <param name="bankBookExportModel">The export model to create.</param>
    /// <param name="fileContent">The file contents to be exported.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The created <see cref="BankBookExportModel"/>.</returns>
    Task<BankBookExportModel> CreateAsync(BankBookExportModel bankBookExportModel, byte[] fileContent, CancellationToken cancellationToken = default);
}
