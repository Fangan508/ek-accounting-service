using Common.Errors;
using System.Diagnostics.CodeAnalysis;

namespace Common.ResultObject;

/// <summary>
/// Provides an object containing details of errors.
/// </summary>
public class Result
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Result"/> class.
    /// </summary>
    /// <param name="isSuccess">Indicates whether the operation was successful.</param>
    /// <param name="error">The error associated with the resul, if any.</param>
    /// <exception cref="ArgumentException"></exception>
    protected internal Result(bool isSuccess, AccountingError error)
    {
        if ((isSuccess && error != AccountingError.None) || 
            (!isSuccess && error == AccountingError.None))
        {
            throw new ArgumentException("Invalid AccountingError", nameof(error));
        }


        IsSuccess = isSuccess;
        Error = error;
    }

    /// <summary>
    /// Gets a value indicating whether the request was successful.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Gets a value indicating whether the request failed.
    /// </summary>
    /// <value>
    /// <c>true</c> if the request failed (i.e., <see cref="IsSuccess"/>; otherwise, <c>false</c> if the request was successful.
    /// </value>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Gets specific Accounting Error.
    /// </summary>
    public AccountingError Error { get; }

    /// <summary>
    /// Creates a successful result, indicating no errors.
    /// </summary>
    /// <returns>
    /// A <see cref="Result"/> representing a successful outcome with no errors.
    /// </returns>
    public static Result Success() => new(true, AccountingError.None);

    /// <summary>
    /// Creats a successful result with a given value, indicating no errors.
    /// </summary>
    /// <typeparam name="TValue">The type of the value being returned in the result.</typeparam>
    /// <param name="value">The value to be included in the result.</param>
    /// <returns>
    /// A <see cref="Result{TValue}"/> representing a successful outcome with the provided value and no errors.
    /// </returns>
    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, AccountingError.None);

    /// <summary>
    /// Creates a result indicating failure, with a specific <see cref="AccountingError"/>.
    /// </summary>
    /// <param name="error">The error that caused the failure</param>
    /// <returns>
    /// A <see cref="Result"/> representing a failure outcome with the provided error.
    /// </returns>
    public static Result Failure(AccountingError error) => new(false, error);

    /// <summary>
    /// Creates a result indicating failure, with the specified error and a default value for the result.
    /// </summary>
    /// <typeparam name="TValue">The type of the value that would have been returned in a successful result.</typeparam>
    /// <param name="error">The error that caused the failure.</param>
    /// <returns>
    /// A <see cref="Result{TValue}"/> representing a failure outcome with the default value and the provided error.
    /// </returns>
    public static Result<TValue> Failure<TValue>(AccountingError error) => new(default, false, error);

    /// <summary>
    /// Creates a result based on the provided. Returns a success result if the value is not null,
    /// or a failure result with a <see cref="AccountingError.NullValue"/> if the value is null.
    /// </summary>
    /// <typeparam name="TValue">The type of the value being passed to create the result.</typeparam>
    /// <param name="value">The value to be included in the result. If null, a failure result is returned.</param>
    /// <returns>
    /// A <see cref="Result{TValue}"/> representing either a success with the given value or a failure with a <see cref="AccountingError.NullValue"/>.
    /// </returns>
    public static Result<TValue> Create<TValue>(TValue? value) => 
        value is not null ? Success(value) : Failure<TValue>(AccountingError.NullValue);

    /// <summary>
    /// Converts a value of type <typeparamref name="TValue"/> to a <see cref="Result{TValue}"/>.
    /// This is the named alternative to the implicit operator.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">The value to convert.</param>
    /// <returns>
    /// A <see cref="Result{TValue}"/> representing the given value.
    /// </returns>
    public static Result<TValue> ToResult<TValue>(TValue? value) => Create(value);

    /// <summary>
    /// Converts an <see cref="AccountingError"/> to a <see cref="Result{TValue}"/>.
    /// This is the named alternative for the implicit operator.
    /// </summary>
    /// <typeparam name="TValue">The type of the value</typeparam>
    /// <param name="error">The error to convert.</param>
    /// <returns>
    /// A <see cref="Result{TValue}"/> representing a failure with the given error.
    /// </returns>
    public static Result<TValue> ToResult<TValue>(AccountingError error) => Failure<TValue>(error);
}

/// <summary>
/// Represents the result of an operation that can either successful or failed, with an associated value of type <typeparamref name="TValue"/>.
/// </summary>
/// <typeparam name="TValue"> The type of the value that the result holds when the operation is successful.</typeparam>
public class Result<TValue> : Result
{
    private readonly TValue? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{TValue}"/> class.
    /// </summary>
    /// <param name="value">The value associated with the result, if the operation was successful.</param>
    /// <param name="isSuccess">Indicates whether the operation was successful.</param>
    /// <param name="error">The error associated with the result, if any.</param>
    protected internal Result(TValue? value, bool isSuccess, AccountingError error) : base(isSuccess, error)
    {
        _value = value;
    }

    /// <summary>
    /// Gets the value of the result if the operation was successful.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown if accessed when the result represents a failure (i.e., <see cref="IsSuccess"/> is false).
    /// </exception>
    /// <value>
    /// The value of type <typeparamref name="TValue"/> associated with the result.
    /// </value>
    [NotNull]
    public TValue? Value => IsSuccess ? _value! : throw new InvalidOperationException("The value of a failure result can not be accessed");

    /// <summary>
    /// Implicitly converts a value of type <typeparamref name="TValue"/> (or <typeparamref name="TValue"/>?) to a <see cref="Result{TValue}"/>.
    /// </summary>
    /// <param name="value">The value of type <typeparamref name="TValue"/> to be converted into a <see cref="Result{TValue}"/>.</param>
    /// <returns>
    /// A <see cref="Result{TValue}"/> representing the given value, which will either be a success of depending on the value.
    /// </returns>
    public static implicit operator Result<TValue>(TValue? value) => ToResult(value);

    /// <summary>
    /// Implicit converts a <see cref="AccountingError"/> to a <see cref="Result{TValue}"/>.
    /// </summary>
    /// <param name="error">The <see cref="AccountingError"/> to be converted into a <see cref="Result{TValue}"/>.</param>
    /// <returns>
    /// A <see cref="Result{TValue}"/> representing a failure outcome with the provided error, where the value is the default for the type <typeparamref name="TValue"/>.
    /// </returns>
    public static implicit operator Result<TValue>(AccountingError error) => ToResult<TValue>(error);
}