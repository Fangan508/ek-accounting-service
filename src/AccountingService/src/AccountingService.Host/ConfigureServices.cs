using AccountingService.Host.Extensions;
using AccountingService.Presentation.Mappings;
using Azure.Identity;
using Common.Infrastructure;
using Common.Interfaces;
using Infrastructure.Repositories;
using Microsoft.OpenApi.Models;
using Service;
using System.Reflection;
using System.Text.Json.Serialization;

namespace AccountingService.Host;

/// <summary>
/// Configures presentation layer services, such as controllers, problem details, and exception handling.
/// </summary>
/// <remarks>This class includes methods to configure essential services for the presentation layer, such as
/// controllers,  JSON serialization options, exception handling, API versioning, and Swagger/OpenAPI documentation.  It
/// is designed to be used during application startup to register these services with the dependency injection
/// container.</remarks>
public static class ConfigureServices
{
    /// <summary>
    /// Configures presentation layer services, such as controllers, problem details, and exception handling.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddSwaggerGen(o =>
        {
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            o.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            o.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "AccountingService.Presentation.xml"));
            o.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Accounting API", 
                Description = "The Accounting service",
            });
            o.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearer" }
                    },
                    Array.Empty<string>()
                }
            });
        });
        services.AddApiVersioning(
            options =>
            {
                options.ReportApiVersions = true;
                options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1);
                options.AssumeDefaultVersionWhenUnspecified = true;
            }).AddApiExplorer(
            options =>
            { 
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

        return services;
    }

    /// <summary>
    /// Registers DI services for the application.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
    {
        //Register services and interfaces for DI
        services.AddMemoryCache();

        services.AddSingleton<DefaultAzureCredential>();
        services.AddSingleton<IAzurePostgresConnectionFactory, AzurePostgresConnectionFactory>();
        //services.AddSingleton<PostgresTokenHealthCHeck>();

        services.AddLogging();
        services.AddScoped<IAccountingBookingService, AccountingBookingService>();

        services.AddAutoMapper(typeof(MappingProfile));

        // Register repositories, unit of work, etc.
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IAccountingBookingRepository, AccountingBookingRepository>();

        return services;
    }
}