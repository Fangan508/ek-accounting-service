namespace Common.Errors;

/// <summary>
/// Represents an error related to accountings.
/// </summary>
public class AccountingError
{
    /// <summary>
    /// Represents no error.
    /// </summary>
    public static readonly AccountingError None = new(string.Empty, string.Empty, ErrorType.Failure);

    /// <summary>
    /// Represents an error when a null value is provided.
    /// </summary>
    public static readonly AccountingError NullValue = new("AccountingError.NullValue", "Null value was provided.", ErrorType.Failure);

    /// <summary>
    /// Represents an error when an null value is provided.
    /// </summary>
    /// <param name="code"></param>
    /// <param name="name"></param>
    /// <param name="type"></param>
    public AccountingError(string code, string name, ErrorType type)
    {
        Code = code;
        Name = name;
        Type = type;
    }

    /// <summary>
    /// Gets the error code.
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Gets the error name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the type of error.
    /// </summary>
    public ErrorType Type { get; }

    /// <summary>
    /// Creates an error of type Failure.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="name">The error name</param>
    /// <returns>
    /// An <see cref="AccountingError"/> instance representing a failure error.
    /// </returns>
    public static AccountingError Failure(string code , string name) => 
        new(code, name, ErrorType.Failure);

    /// <summary>
    /// Creates an error of type NotFound.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="name">The error name</param>
    /// <returns>
    /// An <see cref="AccountingError"/> instance representing a not found error.
    /// </returns>
    public static AccountingError NotFound(string code, string name) =>
        new(code, name, ErrorType.NotFound);

    /// <summary>
    /// Creates an error of type Problem.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="name">The error name</param>
    /// <returns>
    /// An <see cref="AccountingError"/> instance representing a problem.
    /// </returns>
    public static AccountingError Problem(string code, string name) =>
        new(code, name, ErrorType.Problem);

    /// <summary>
    /// Creates an error of type Conflict.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="name">The error name</param>
    /// <returns>
    /// An <see cref="AccountingError"/> instance representing a conflict.
    /// </returns>
    public static AccountingError Conflict(string code, string name) =>
        new(code, name, ErrorType.Conflict);

    /// <summary>
    /// Creates an error of type Validation.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="name">The error name</param>
    /// <returns>
    /// An <see cref="AccountingError"/> instance representing a validation error.
    /// </returns>
    public static AccountingError Validation(string code, string name) =>
        new(code, name, ErrorType.Validation);
}