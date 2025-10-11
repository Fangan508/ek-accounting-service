using Azure.Core;
using Azure.Identity;
using Npgsql;

namespace AccountingService.Host.Extensions;

/// <summary>
/// Factory for creating PostgreSQL data sources with Azure access token support.
/// </summary>
public class AzurePostgresConnectionFactory : IAzurePostgresConnectionFactory, IDisposable
{
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _environment;
    private readonly DefaultAzureCredential _credential;
    private readonly ILogger<AzurePostgresConnectionFactory> _logger;

    private NpgsqlDataSource? _dataSource;
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="AzurePostgresConnectionFactory"/> class.
    /// </summary>
    /// <param name="configuration">The application configuration.</param>
    /// <param name="environment">The hosting environment.</param>
    /// <param name="credential">The Azure credential for token acquisition.</param>
    /// <param name="logger">The logger for logging information.</param>
    public AzurePostgresConnectionFactory(
        IConfiguration configuration,
        IHostEnvironment environment,
        DefaultAzureCredential credential,
        ILogger<AzurePostgresConnectionFactory> logger)
    {
        _configuration = configuration;
        _environment = environment;
        _credential = credential;
        _logger = logger;
    }

    /// <summary>
    /// Gets the PostgreSQL data source configured for the application.
    /// </summary>
    /// <returns>An instance of <see cref="NpgsqlDataSource"/> configured for the application.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the required configuration values for the connection string or token scope are missing.
    /// </exception>
    public NpgsqlDataSource GetPostgresDataSource()
    {
        if (_environment.IsEnvironment("Local"))
        { 
            return new NpgsqlDataSourceBuilder(_configuration["ConnectionString:POSTGRES_CONNECTION_STRING"]!).Build();
        }

        if (_dataSource != null)
        {
            return _dataSource;
        }

        var baseConnStr = _configuration["AZURE_POSTGRESQL_CONNECTIONSTRING"];
        if (string.IsNullOrEmpty(baseConnStr))
        {
            _logger.LogError("AZURE_POSTGRESQL_CONNECTIONSTRING is not set in the configuration.");
            throw new InvalidOperationException("AZURE_POSTGRESQL_CONNECTIONSTRING is not set.");
        }

        var scope = _configuration["TokenScopes:AzurePostgreSql"];
        if (string.IsNullOrEmpty(scope))
        {
            _logger.LogError("TokenScopes:AzurePostgreSql is not set in the configuration.");
            throw new InvalidOperationException("TokenScopes:AzurePostgreSql is not set.");
        }

        var builder = new NpgsqlDataSourceBuilder(baseConnStr);

        builder.UsePeriodicPasswordProvider(
            async (builder, ct) =>
            {
                var token = await _credential.GetTokenAsync(
                    new TokenRequestContext([scope]), ct);

                return token.Token;
            }, 
            successRefreshInterval: TimeSpan.FromMinutes(5), // Refresh token every 5 minutes
            failureRefreshInterval: TimeSpan.FromSeconds(30) // Retry every 30 seconds on failure
         );

        _dataSource = builder.Build();
        return _dataSource;
    }

    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="AzurePostgresConnectionFactory"/> and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _dataSource?.Dispose();
            }
            _disposed = true;
        }
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}