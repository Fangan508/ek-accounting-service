using Npgsql;

namespace AccountingService.Host.Extensions;

/// <summary>
/// Represents a factory for creating connections to an Azure Postgres database.
/// </summary>
public interface IAzurePostgresConnectionFactory
{
    /// <summary>
    /// Gets an instance of <see cref="NpgsqlDataSource"/> to interact with the Azure Postgres database.
    /// </summary>
    /// <returns>An <see cref="NpgsqlDataSource"/> instance configured for the database.</returns>
    NpgsqlDataSource GetPostgresDataSource();
}