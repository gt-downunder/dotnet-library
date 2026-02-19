namespace Grondo
{
    /// <summary>
    /// Represents the outcome of an operation that can either succeed with a value or fail with an error message.
    /// </summary>
    /// <typeparam name="T">The type of the success value.</typeparam>
    public readonly struct Result<T>
    {
        private readonly T? _value;
        private readonly string? _error;

        private Result(T value)
        {
            _value = value;
            _error = null;
            IsSuccess = true;
        }

        private Result(string error)
        {
            _value = default;
            _error = error;
            IsSuccess = false;
        }

        /// <summary>Gets a value indicating whether the operation succeeded.</summary>
        public bool IsSuccess { get; }

        /// <summary>Gets a value indicating whether the operation failed.</summary>
        public bool IsFailure => !IsSuccess;

        /// <summary>
        /// Gets the success value. Throws if the result is a failure.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the result is a failure.</exception>
        public T Value => IsSuccess
            ? _value!
            : throw new InvalidOperationException($"Cannot access Value on a failed result. Error: {_error}");

        /// <summary>
        /// Gets the error message. Throws if the result is a success.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the result is a success.</exception>
        public string Error => IsFailure
            ? _error!
            : throw new InvalidOperationException("Cannot access Error on a successful result.");

        /// <summary>Creates a successful result with the specified value.</summary>
        public static Result<T> Success(T value) => new(value);

        /// <summary>Creates a failed result with the specified error message.</summary>
        public static Result<T> Failure(string error) => new(error);

        /// <summary>Implicitly converts a value to a successful result.</summary>
        public static implicit operator Result<T>(T value) => Success(value);

        /// <summary>
        /// Gets the value if successful, or the specified fallback value if failed.
        /// </summary>
        public T GetValueOrDefault(T fallback) => IsSuccess ? _value! : fallback;

        /// <summary>
        /// Transforms the success value using the specified function.
        /// </summary>
        public Result<TNew> Map<TNew>(Func<T, TNew> mapper) =>
            IsSuccess ? Result<TNew>.Success(mapper(_value!)) : Result<TNew>.Failure(_error!);

        /// <summary>
        /// Chains this result with another operation that returns a Result.
        /// </summary>
        public Result<TNew> Bind<TNew>(Func<T, Result<TNew>> binder) =>
            IsSuccess ? binder(_value!) : Result<TNew>.Failure(_error!);
    }

    /// <summary>
    /// Provides extension methods for creating <see cref="Result{T}"/> instances.
    /// </summary>
    public static class ResultEx
    {
        /// <summary>
        /// Executes the function and wraps the result, catching any exceptions as failures.
        /// </summary>
        public static Result<T> TryExecute<T>(Func<T> func)
        {
            try
            {
                return Result<T>.Success(func());
            }
            catch (Exception ex)
            {
                return Result<T>.Failure(ex.Message);
            }
        }

        /// <summary>
        /// Executes the async function and wraps the result, catching any exceptions as failures.
        /// </summary>
        public static async Task<Result<T>> TryExecuteAsync<T>(Func<Task<T>> func)
        {
            try
            {
                return Result<T>.Success(await func().ConfigureAwait(false));
            }
            catch (Exception ex)
            {
                return Result<T>.Failure(ex.Message);
            }
        }

        extension<T>(T value)
        {
            /// <summary>
            /// Wraps the value in a successful <see cref="Result{T}"/>.
            /// </summary>
            public Result<T> ToResult() => Result<T>.Success(value);
        }
    }
}
