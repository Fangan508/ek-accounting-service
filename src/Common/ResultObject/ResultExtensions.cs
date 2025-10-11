using Common.Errors;
using Microsoft.AspNetCore.Http;

namespace Common.ResultObject;

/// <summary>
/// Provides extension methods for handling <see cref="Result"/> objects.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Converts a <see cref="Result"/> object to an ASP.NET Core <see cref="IResult"/> containing problem details.
    /// </summary>
    /// <param name="result">The <see cref="Result"/> object to be converted.</param>
    /// <returns>An <see cref="IResult"/> representing the problem details of the failed operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the <paramref name="result"/> indicates a successful operation, as no problem details can be generated for a success.</exception>
    public static IResult ToProblemDetails(this Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException();
        }

        return Results.Problem(detail: result.Error.Code + ": " + result.Error.Name, statusCode: GetHttpStatusCode(result.Error.Type));
    }

    private static int GetHttpStatusCode(ErrorType errorType) => 
        errorType switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Problem => StatusCodes.Status412PreconditionFailed,
            _ => StatusCodes.Status500InternalServerError
        };
}