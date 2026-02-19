namespace Grondo
{
    /// <summary>
    /// Represents an optional value that may or may not exist.
    /// Use <see cref="Maybe{T}.Some(T)"/> to wrap a value or <see cref="Maybe{T}.None"/> for absence.
    /// </summary>
    /// <typeparam name="T">The type of the contained value.</typeparam>
    public readonly struct Maybe<T>
    {
        private readonly T? _value;

        private Maybe(T value)
        {
            _value = value;
            HasValue = true;
        }

        /// <summary>Gets a value indicating whether this instance contains a value.</summary>
        public bool HasValue { get; }

        /// <summary>Gets a value indicating whether this instance has no value.</summary>
        public bool HasNoValue => !HasValue;

        /// <summary>Gets the contained value.</summary>
        /// <exception cref="InvalidOperationException">Thrown if the instance has no value.</exception>
        public T Value => HasValue
            ? _value!
            : throw new InvalidOperationException("Maybe has no value.");

        /// <summary>Creates a <see cref="Maybe{T}"/> with a value.</summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>A Maybe containing the value.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is null.</exception>
        public static Maybe<T> Some(T value)
        {
            ArgumentNullException.ThrowIfNull(value);
            return new Maybe<T>(value);
        }

        /// <summary>Gets a <see cref="Maybe{T}"/> with no value.</summary>
        public static Maybe<T> None => default;

        /// <summary>
        /// Transforms the contained value using the specified mapping function.
        /// Returns <see cref="None"/> if this instance has no value.
        /// </summary>
        /// <typeparam name="TResult">The type of the transformed value.</typeparam>
        /// <param name="mapper">The function to transform the contained value.</param>
        /// <returns>A <see cref="Maybe{TResult}"/> with the transformed value, or <see cref="Maybe{TResult}.None"/>.</returns>
        public Maybe<TResult> Map<TResult>(Func<T, TResult> mapper)
        {
            ArgumentNullException.ThrowIfNull(mapper);
            return HasValue ? Maybe<TResult>.Some(mapper(_value!)) : Maybe<TResult>.None;
        }

        /// <summary>
        /// Projects the contained value into a new <see cref="Maybe{TResult}"/> using the specified function.
        /// Returns <see cref="None"/> if this instance has no value.
        /// </summary>
        /// <typeparam name="TResult">The type of the new Maybe's value.</typeparam>
        /// <param name="binder">The function that returns a new Maybe.</param>
        /// <returns>The result of the binder, or <see cref="Maybe{TResult}.None"/>.</returns>
        public Maybe<TResult> Bind<TResult>(Func<T, Maybe<TResult>> binder)
        {
            ArgumentNullException.ThrowIfNull(binder);
            return HasValue ? binder(_value!) : Maybe<TResult>.None;
        }

        /// <summary>
        /// Applies the appropriate function depending on whether a value is present.
        /// </summary>
        /// <typeparam name="TResult">The return type of the match.</typeparam>
        /// <param name="some">The function to apply if a value is present.</param>
        /// <param name="none">The function to apply if no value is present.</param>
        /// <returns>The result of applying the appropriate function.</returns>
        public TResult Match<TResult>(Func<T, TResult> some, Func<TResult> none)
        {
            ArgumentNullException.ThrowIfNull(some);
            ArgumentNullException.ThrowIfNull(none);
            return HasValue ? some(_value!) : none();
        }

        /// <summary>
        /// Returns the contained value or the specified default if no value is present.
        /// </summary>
        /// <param name="defaultValue">The fallback value to return if no value is present.</param>
        /// <returns>The contained value, or <paramref name="defaultValue"/> if no value is present.</returns>
        public T GetValueOrDefault(T defaultValue) =>
            HasValue ? _value! : defaultValue;

        /// <summary>
        /// Returns the contained value if the predicate is satisfied; otherwise returns <see cref="None"/>.
        /// </summary>
        /// <param name="predicate">The predicate to evaluate against the contained value.</param>
        /// <returns>This instance if the predicate passes, or <see cref="None"/>.</returns>
        public Maybe<T> Where(Func<T, bool> predicate)
        {
            ArgumentNullException.ThrowIfNull(predicate);
            return HasValue && predicate(_value!) ? this : None;
        }

        /// <summary>
        /// Executes an action on the contained value if present. Returns this instance unchanged.
        /// </summary>
        /// <param name="action">The action to execute on the contained value.</param>
        /// <returns>This instance, unchanged.</returns>
        public Maybe<T> Execute(Action<T> action)
        {
            ArgumentNullException.ThrowIfNull(action);
            if (HasValue) action(_value!);
            return this;
        }

        /// <summary>
        /// Converts this <see cref="Maybe{T}"/> to a <see cref="Result{T}"/>.
        /// Returns <see cref="Result{T}.Success(T)"/> if a value is present;
        /// otherwise returns <see cref="Result{T}.Failure(string)"/> with the specified error.
        /// </summary>
        /// <param name="errorMessage">The error message to use when no value is present.</param>
        /// <returns>A successful result with the value, or a failure with the error message.</returns>
        public Result<T> ToResult(string errorMessage) =>
            HasValue ? Result<T>.Success(_value!) : Result<T>.Failure(errorMessage);

        /// <summary>Returns a string representation of this instance.</summary>
        /// <returns>A string in the form "Some(value)" or "None".</returns>
        public override string ToString() =>
            HasValue ? $"Some({_value})" : "None";

        /// <summary>
        /// Implicitly converts a value to a <see cref="Maybe{T}"/> containing that value.
        /// Returns <see cref="None"/> if the value is null.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        public static implicit operator Maybe<T>(T? value) =>
            value is null ? None : new Maybe<T>(value);
    }
}

