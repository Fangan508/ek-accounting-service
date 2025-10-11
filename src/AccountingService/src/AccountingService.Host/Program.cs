using AccountingService.Host;
using AccountingService.Host.Extensions;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // Loads the main configuration file (required).
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true) // Loads environment-specific configuration file (optional).
    .AddUserSecrets<Program>() // Loads secrets from user secrets store (the secrets.json file linked to the project’s UserSecretsId).
    .AddEnvironmentVariables(); // Loads configuration values from environment variables.


// AllowedCorsOrigins controls which origins are allowed to make cross-origin requests to the API.
var allowedOrigins = builder.Configuration.GetSection("AllowedCorsOrigins:Origins").Get<string[]>();

if (allowedOrigins == null || allowedOrigins.Length == 0)
{
    throw new InvalidOperationException("Missing required configuration: AllowedCorsOrigins:Origins");
}

// Add services to the container.
builder.Services
    .AddPresentation()
    .AddDependencyInjection()
    .AddCors(options =>
    {         
        options.AddPolicy("AllowAngularApp",
            policy => policy
                .WithOrigins(allowedOrigins)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials());
     });

builder.Services.AddDbContext<AccountingDbContext>((sp, options) =>
{
    var factory = sp.GetRequiredService<IAzurePostgresConnectionFactory>();

    try
    {
        var dataSource = factory.GetPostgresDataSource();
        options.UseNpgsql(dataSource);
    }
    catch (Exception ex)
    {
        var logger = sp.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Failed to acquire PostgreSQL data source during DbContext registration.");
        throw;
    }
});

builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapHealthChecks("/api/health");

app.ApplyNecessaryDatabaseMigrations();

//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Local") || app.Environment.EnvironmentName == "Staging")
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngularApp");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();

//var summaries = new[]
//{
//    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//};

//app.MapGet("/weatherforecast", () =>
//{
//    var forecast = Enumerable.Range(1, 5).Select(index =>
//        new WeatherForecast
//        (
//            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//            Random.Shared.Next(-20, 55),
//            summaries[Random.Shared.Next(summaries.Length)]
//        ))
//        .ToArray();
//    return forecast;
//})
//.WithName("GetWeatherForecast")
//.WithOpenApi();

//app.Run();

//internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
//{
//    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
//}
