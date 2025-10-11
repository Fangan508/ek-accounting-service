using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Common.Infrastructure
{
    /// <summary>
    /// Provides application-level diagnostics and metrics for monitoring the system.
    /// </summary>
    public static class ApplicationDiagnostics
    {
        /// <summary>
        /// The name of the service for which metrics are being recorded.
        /// </summary>
        private const string ServiceName = "de-ek-accountingservice";

        /// <summary>
        /// A <see cref="Meter"/> instance used to create and track metrics for the application.
        /// </summary>
        public static readonly Meter Meter = new Meter(ServiceName);

        /// <summary>
        /// ActivitySource for distributed tracing.
        /// </summary>
        public static readonly ActivitySource ActivitySource = new ActivitySource(ServiceName);

        public static readonly Histogram<double> HttpRequestDuration =
            Meter.CreateHistogram<double>(
                name: "http_request_duration_seconds",
                description: "Duration of HTTP requests in seconds");

        /// <summary>
        /// Tracks business operations (e.g., bank books create, updated).
        /// </summary>
        public static readonly Counter<long> BusinessOpertaions =
            Meter.CreateCounter<long>(
                name: "business_operations_total",
                description: "Total count of business operations");

        /// <summary>
        /// Tracks application errors by type and operation.
        /// </summary>
        public static readonly Counter<long> ApplicationErrors = 
            Meter.CreateCounter<long>(
                name: "application_errors_total", 
                description: "Total count of application errors");

        /// <summary>
        /// Tracks the number of items returned by query operations (useful for GetBankBooks).
        /// </summary>
        public static readonly Histogram<int> QueryResultCount = 
            Meter.CreateHistogram<int>(
                name: "query_result_count",
                description: "Number of items returned by query operations");

        /// <summary>
        /// Helper method to record HTTP request metrics.
        /// </summary>
        /// <param name="method">The HTTP method used for the request (e.g., GET, POST,).</param>
        /// <param name="endpoint">The endpoint or route targeded by the HTTP request.</param>
        /// <param name="statusCode">The HTTP status code returned by the request.</param>
        /// <param name="durationSeconds">The duration of the HTTP request in seconds.</param>
        public static void RecordHttpRequest(string method, string endpoint, int statusCode, double durationSeconds)
        {
            HttpRequestDuration.Record(durationSeconds,
                new("method", method),
                new("endpoint", endpoint),
                new("status_code", statusCode));
        }

        /// <summary>
        /// Helper method to record business operations.
        /// </summary>
        /// <param name="result">The result of the operation (e.g., "Success", "Failure").</param>
        /// <param name="itemCount">The number of items affected or returned by the operation, if applicable.</param>
        /// <param name="methodName">The name of the method where the operation is being recorded. This is automatically populated if not provided.</param>
        public static void RecordBusinessOperation(string result, int? itemCount = null, [CallerMemberName] string methodName = "")
        {
            BusinessOpertaions.Add(1,
                new("operation", methodName),
                new("result", result));

            if (itemCount.HasValue && methodName.Contains("Get"))
            {
                QueryResultCount.Record(itemCount.Value, new KeyValuePair<string, object?>("operation", methodName));
            }
        }

        /// <summary>
        /// Helper method to record application errors.
        /// </summary>
        /// <param name="errorType">The type or category of the error (e.g., "DatabaseError", "ValidationError").</param>
        /// <param name="errorCode">An optional error code providing more specific information about the error.</param>
        /// <param name="methodName">The name of the method where the error occurred. This is automatically populated if not provided.</param>
        public static void RecordError(string errorType, string? errorCode = null, [CallerMemberName] string methodName = "")
        {
            var tags = new List<KeyValuePair<string, object?>>
            {
                new("operation", methodName),
                new("error_Type", errorType)
            };

            if (!string.IsNullOrWhiteSpace(errorCode))
            {
                tags.Add(new("error_Code", errorCode));
            }

            ApplicationErrors.Add(1, [.. tags]);
        }
    }
}
