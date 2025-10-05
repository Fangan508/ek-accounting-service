using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Common.Infrastructure
{
    /// <summary>
    /// A global exception handler that handles and logs unhandled exceptions across the application.
    /// </summary>
    /// <remarks>This class implements the <see cref="IExceptionHandler"/> interface to handle exceptions that
    /// occur during the processing of HTTP requests. It logs the exception details and generates a standardized HTTP
    /// response with a problem details payload, as defined by RFC 7807.</remarks>
    /// <param name="logger">The logger instance used to log exception details.</param>
    public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        /// <summary>
        /// Attempts to handle an unhandled exception by logging it and responding with a standardized problem details response.
        /// </summary>
        /// <param name="httpContext">The current HTTP context.</param>
        /// <param name="exception">The exception to handle.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>
        /// A task that resolves to <c>true</c> if the exception was handled; otherwise, <c>false</c>.
        /// </returns>
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError(exception, "An unhandled exception occurred while processing the request.");

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
                Title = "Server failure"
            };

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
