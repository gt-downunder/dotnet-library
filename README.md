# Grondo

[![NuGet](https://img.shields.io/nuget/v/Grondo.svg)](https://www.nuget.org/packages/Grondo)
[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)

A C# 14 / .NET 10 library providing functional programming types, extension methods, utilities, and custom exception types. Features `Result<T>`, `Maybe<T>`, `Either<L,R>`, and `Validation<T>` with LINQ query syntax support, plus 29 extension method classes covering strings, collections, async operations, and more.

## Installation

```bash
dotnet add package Grondo
```

> **Note:** Grondo uses the ASP.NET Core shared framework. See [Requirements](#requirements) for details.

## Features

### Extension Methods

| Class | Methods | Description |
|---|---|---|
| `BooleanEx` | `ToYesNo`, `ToActiveInactive`, … | Boolean-to-string conversions |
| `ByteArrayEx` | `ToHex`, `ToBase64`, `ToSha256` | Hex/Base64 encoding, SHA-256 hashing for byte arrays |
| `ClaimsPrincipalEx` | `GetClaim`, `GetClaimValue` | Claim extraction helpers for `ClaimsPrincipal` |
| `ConfigurationEx` | `GetValue<T>`, `GetRequiredValue<T>`, `GetSection<T>`, `GetValueOrDefault`, `HasKey` | Typed value retrieval from `IConfiguration` |
| `DateTimeEx` | `StartOfDay`, `EndOfDay`, `StartOfMonth`, `ToRelativeTime`, … | Formatting, parsing, week arithmetic, start/end of day/month, relative time |
| `DateTimeOffsetEx` | *(mirrors `DateTimeEx`)* | Same set of extensions for `DateTimeOffset` |
| `DictionaryEx` | `ToJson`, `Merge`, `GetOrAdd`, `ToQueryString`, `IsDeepEqualTo`, … | JSON serialization, key lookups, deep equality, merge, get-or-add, query strings |
| `EnumEx` | `GetDescription`, `GetDisplayName` | Description attributes and display name helpers |
| `EnumerableEx` | `Batch`, `Partition`, `Interleave`, `Scan`, `Window`, `Shuffle`, `FallbackIfEmpty`, `Pairwise`, `TagFirstLast`, `WhereNotNull`, … | Rich LINQ-style collection operations (synchronous) |
| `EnumerableExAsync` | `SelectAsync`, `SelectAsyncParallel`, `WhereAsync`, `ForEachAsync`, `AggregateAsync`, `AnyAsync`, `AllAsync` | Async LINQ operations with concurrency control |
| `EnvironmentEx` | `IsLocal`, `IsTest`, `IsUat`, `IsProduction`, `IsDevelopment`, `IsStaging`, `IsEnvironment` | Hosting environment checks |
| `ExceptionEx` | `Flatten`, `ToDetailString` | Flatten inner exceptions, extract full detail strings |
| `GuardEx` | `ThrowIfNull`, `ThrowIfNullOrEmpty`, `ThrowIfNullOrWhiteSpace`, `ThrowIfTooLong`, `ThrowIfTooShort`, `ThrowIfContains`, `ThrowIfDoesNotContain`, `ThrowIfNotDefined`, `ThrowIfInPast`, `ThrowIfInFuture`, `ThrowIfEmpty` (Guid), … | Comprehensive guard clauses for validation |
| `GuidEx` | `IsEmpty`, `ToShortString` | `IsEmpty` check, short-string encoding |
| `HttpEx` | `EnsureObject<T>`, `GetRawBodyAsync`, `GetRawBodyAsBytesAsync`, `GetQueryParams`, `GetFormDataAsync`, `IsAjaxRequest`, `GetClientIpAddress` | HTTP request helpers and body extraction |
| `JsonEx` | `ToJson`, `FromJson<T>`, `TryFromJson<T>` | JSON serialize/deserialize with optional indented output |
| `ListEx` | `AddIfNotNull`, `AddRangeNoDuplicates`, `ContainsIgnoreCase` | Null-safe add, deduplication, case-insensitive contains |
| `NumericEx` | `IsZero`, `IsPositive`, `IsNegative`, `ToHumanByteSize`, `ToOrdinal`, `Days`, `Hours`, `Minutes`, `Seconds`, `Milliseconds` | Numeric checks, byte-size humanization, ordinals, fluent `TimeSpan` construction |
| `ObjectEx` | `IsNumeric`, `ToNullableDouble`, `ToNullableInteger`, `ToStringContent`, `Pipe` | Null/empty checks, type conversions, side-effect tap |
| `SemaphoreSlimEx` | `LockAsync` | Returns `IAsyncDisposable` for scoped locking |
| `ServiceProviderEx` | `GetScopedService<T>` | Scoped service resolution from `IServiceProvider` |
| `SetEx` | `IsDeepEqualTo` | Compare sets against serialized JSON |
| `SpanEx` | `ContainsIgnoreCase`, `IsNumeric`, `SafeTrim` | High-performance `ReadOnlySpan<char>` operations |
| `StreamEx` | `ToByteArrayAsync`, `ToStringAsync`, `ToMemoryStreamAsync` | Async stream conversion helpers |
| `StringEx` | `IsWellFormedEmailAddress`, `Truncate`, `Mask`, `ToSlug`, `ToBase64`, `FromBase64`, `IsNumeric`, `IsGuid`, `ToSnakeCase`, `ToKebabCase`, `ToCamelCase`, `Humanize`, `Reverse`, … | Comprehensive string manipulation and validation |
| `TaskEx` | `RetryAsync`, `FireAndForget`, `WhenAllSequentialAsync`, `WithTimeoutAsync` | Retry with exponential backoff and exception filtering, fire-and-forget, sequential execution |
| `TimeSpanEx` | `ToHumanReadable` | Friendly duration strings with optional part limit |
| `UriEx` | `IsAbsolute`, `GetDomain` | URI inspection and diagnostics |
| `XmlSerializerEx` | `ToXml`, `FromXml<T>` | XML serialize/deserialize helpers |
| `FuncEx` | `Memoize`, `Debounce`, `Throttle`, `ToLazy` | Function memoization, debouncing, throttling, and lazy initialization |

### Types

| Type | Description |
|---|---|
| `Result<T>` | Railway-oriented result type (`readonly struct`) with LINQ query syntax — `Map`, `Bind`, `Match`, `Tap`, `Recover`, `Flatten`, `Zip`, `OrElse`, async variants, `IEquatable<T>`, `[DebuggerDisplay]` |
| `Result` | Non-generic result for void operations (`readonly struct`) — `Tap`, `TapError`, `MapError`, `Ensure`, `Match`, `Bind<T>`, async variants, `IEquatable`, `[DebuggerDisplay]` |
| `Maybe<T>` | Optional value type (`readonly struct`) with LINQ query syntax — `Some`, `None`, `Map`, `Bind`, `Match`, `Where`, `Filter`, `OrElse`, `OrValue`, `Flatten`, `Zip`, `ToResult`, async variants, `IEquatable<T>`, `[DebuggerDisplay]` |
| `Either<L,R>` | Dual-value type (`readonly struct`) — `FromLeft`, `FromRight`, `Match`, `Map`, `Bind`, `MapLeft`, `Tap`, `TapLeft`, async variants, `IEquatable<T>`, `[DebuggerDisplay]` |
| `Validation<T>` | Accumulative validation type (`readonly struct`) — `Valid`, `Invalid`, `Combine`, `Map`, `Bind`, `Match`, `ToResult`, async variants, `IEquatable<T>`, `[DebuggerDisplay]` |
| `JsonDefaults` | Pre-configured, cached `JsonSerializerOptions` for consistent serialization |
| `StringFactory` | Factory methods for generating string values (thread-safe with `Random.Shared`) |
| `Environments` | Standard environment name constants (Local, Test, Uat, Prod) |
| `ErrorResponse` | Standardized error response record with message and header |

### Custom Exceptions

Pre-built exception types mapped to HTTP status codes, each extending `ExceptionBase` with `StatusCode` and `MessageHeader`:

| Exception | HTTP Status |
|---|---|
| `BadRequestException` | 400 |
| `NotAuthorizedException` | 401 |
| `ForbiddenException` | 403 |
| `EntityNotFoundException` | 404 |
| `BusinessException` | 409 |
| `ConflictException` | 409 |
| `DuplicateFoundException` | 409 |
| `UnprocessableEntityException` | 422 |
| `TooManyRequestsException` | 429 (with `RetryAfter`) |
| `TechnicalException` | 500 |
| `MethodNotAvailableException` | 501 |
| `ServiceUnavailableException` | 503 (with `RetryAfter`) |
| `ValidationException` | 400 (with field-level `Errors`) |

> 📖 **[Explore the full documentation with code examples →](https://gt-downunder.github.io/Grondo/)**

## Usage

### String Extensions

```csharp
using Grondo.Extensions;

"user@example.com".IsWellFormedEmailAddress(); // true
"Hello World!".Truncate(8);                    // "Hello..."
"secret data".Mask(4);                         // "*******data"
"My Blog Post!".ToSlug();                      // "my-blog-post"
"Hello".ToBase64();                            // "SGVsbG8="
"12345".IsNumeric();                           // true

// Case conversions
"MyPropertyName".ToSnakeCase();                // "my_property_name"
"MyPropertyName".ToKebabCase();                // "my-property-name"
"my_variable_name".ToCamelCase();              // "myVariableName"
"some_variable".Humanize();                    // "Some variable"
"hello".Reverse();                             // "olleh"
```

### Collection Extensions

```csharp
new[] { 1, 2, 3, 4, 5 }.Batch(2);             // [[1,2], [3,4], [5]]
items.Partition(x => x.IsActive);              // (Matches, NonMatches)
nullableList.WhereNotNull();

// Scan — running accumulator
new[] { 1, 2, 3, 4 }.Scan(0, (acc, x) => acc + x); // [1, 3, 6, 10]

// Sliding window
new[] { 1, 2, 3, 4, 5 }.Window(3);            // [[1,2,3], [2,3,4], [3,4,5]]

// Shuffle, pairwise, tagging
items.Shuffle();
items.Pairwise((a, b) => b - a);              // consecutive differences
items.TagFirstLast((item, isFirst, isLast) => ...);

// Fallback for empty sequences
emptyList.FallbackIfEmpty([defaultItem]);
```

### Numeric Extensions

```csharp
1024L.ToHumanByteSize();                       // "1 KB"
(1536L * 1024).ToHumanByteSize();              // "1.5 MB"
1000L.ToHumanByteSize(useSI: true);            // "1 kB"

1.ToOrdinal();                                 // "1st"
22.ToOrdinal();                                // "22nd"

// Fluent TimeSpan construction
3.Days();                                      // TimeSpan.FromDays(3)
2.5.Hours();                                   // TimeSpan.FromHours(2.5)
500.Milliseconds();                            // TimeSpan.FromMilliseconds(500)
```

### Guard Clauses

```csharp
// Null / empty / default guards
name.ThrowIfNull();
input.ThrowIfNullOrWhiteSpace();
items.ThrowIfEmpty();

// Numeric guards
age.ThrowIfNegative();
count.ThrowIfZero();
index.ThrowIfOutOfRange(0, 100);

// Predicate and format guards
value.ThrowIf(x => x > 1000, "Value too large", nameof(value));
email.ThrowIfInvalidFormat(@"^[\w.-]+@[\w.-]+\.\w+$", nameof(email));
```

### Result&lt;T&gt; — Railway-Oriented Pipelines

```csharp
using Grondo;

// Full async pipeline with chained operations
var result = await GetUserAsync(id)              // Task<Result<User>>
    .EnsureAsync(u => u.IsActive, "User inactive")
    .MapAsync(u => u.Email)
    .BindAsync(email => SendEmailAsync(email))
    .TapAsync(r => LogAsync("Email sent"))
    .MapErrorAsync(e => $"Pipeline failed: {e}");

result.Match(
    onSuccess: val => Console.WriteLine($"OK: {val}"),
    onFailure: err => Console.WriteLine($"Error: {err}"));

// Non-generic Result for void operations
Result outcome = Result.Success()
    .Tap(() => Console.WriteLine("Executed"))
    .Ensure(() => conditionMet, "Condition not met")
    .MapError(e => $"Wrapped: {e}");
```

### Maybe&lt;T&gt; — Optional Values

```csharp
using Grondo;

Maybe<string> name = Maybe<string>.Some("Alice");
Maybe<string> empty = Maybe<string>.None;

// Implicit conversion (null becomes None)
Maybe<string> fromValue = "hello";           // Some("hello")
Maybe<string> fromNull = (string?)null;      // None

// Transform and chain
var upper = name.Map(s => s.ToUpper());      // Some("ALICE")
var greeting = name.Bind(s => Maybe<string>.Some($"Hi, {s}!"));

// Pattern matching
string display = name.Match(
    some: v => $"Name: {v}",
    none: () => "Anonymous");

// Filter and default
var long_name = name.Where(n => n.Length > 10); // None
string fallback = empty.GetValueOrDefault("N/A"); // "N/A"

// Bridge to Result<T>
Result<string> result = empty.ToResult("Name is required");

// LINQ query syntax (NEW!)
var fullName = from first in GetFirstName()
               from last in GetLastName()
               select $"{first} {last}";
```

### LINQ Query Syntax Support (NEW!)

```csharp
using Grondo;

// Result<T> with LINQ
var orderResult =
    from userId in ValidateUserId(request.UserId)
    from user in GetUser(userId)
    from product in GetProduct(request.ProductId)
    from inventory in CheckInventory(product.Id)
    select new Order
    {
        UserId = user.Id,
        ProductId = product.Id,
        Quantity = request.Quantity
    };

// Maybe<T> with LINQ
var displayName =
    from user in GetUser(id)
    where user.IsActive
    from profile in user.GetProfile()
    where !string.IsNullOrEmpty(profile.DisplayName)
    select profile.DisplayName;
```

### Either&lt;L,R&gt; — Typed Errors (NEW!)

```csharp
using Grondo;

public Either<ValidationError, User> CreateUser(CreateUserRequest request)
{
    var validation = ValidateRequest(request);
    if (!validation.IsValid)
        return Either<ValidationError, User>.FromLeft(
            new ValidationError(validation.Errors));

    var user = new User { Name = request.Name, Email = request.Email };
    return Either<ValidationError, User>.FromRight(user);
}

// Pattern matching with typed errors
var result = CreateUser(request);
return result.Match(
    onLeft: error => BadRequest(new { errors = error.Fields }),
    onRight: user => Ok(user));
```

### Validation&lt;T&gt; — Accumulative Validation (NEW!)

```csharp
using Grondo;

public Validation<User> ValidateUser(UserRequest request)
{
    var errors = new List<string>();

    if (string.IsNullOrWhiteSpace(request.Name))
        errors.Add("Name is required");
    if (!IsValidEmail(request.Email))
        errors.Add("Email must be valid");
    if (request.Age < 18)
        errors.Add("Age must be 18 or older");

    // Returns ALL errors at once!
    if (errors.Count > 0)
        return Validation<User>.Invalid(errors);

    return Validation<User>.Valid(new User { /* ... */ });
}

// Usage
var validation = ValidateUser(request);
if (validation.IsInvalid)
{
    // Returns: ["Name is required", "Email must be valid", "Age must be 18+"]
    return BadRequest(new { errors = validation.Errors });
}
```

### Async LINQ Extensions (NEW!)

```csharp
using Grondo.Extensions;

var userIds = new[] { 1, 2, 3, 4, 5 };

// Sequential async projection
var users = await userIds.SelectAsync(id => GetUserAsync(id));

// Parallel async with concurrency limit
var enriched = await users.SelectAsyncParallel(
    user => EnrichAsync(user),
    maxConcurrency: 10);

// Async filtering
var active = await users.WhereAsync(u => IsActiveAsync(u));
```

### Memoization, Debounce, Throttle (NEW!)

```csharp
using Grondo.Extensions;

// Memoization - cache expensive calculations
Func<int, int> fibonacci = null!;
fibonacci = n => n <= 1 ? n : fibonacci(n - 1) + fibonacci(n - 2);
var memoized = fibonacci.Memoize();

var result = memoized(40); // Calculated once, then cached

// Debounce - wait for user to stop typing
Action search = () => PerformSearch();
var debounced = search.Debounce(TimeSpan.FromMilliseconds(300));
textBox.TextChanged += (s, e) => debounced();

// Throttle - limit API call frequency
Action update = () => SendMetrics();
var throttled = update.Throttle(TimeSpan.FromSeconds(5));
```

### Pipe — Side-Effect Tap

```csharp
var result = GetValue()
    .Pipe(v => logger.LogDebug("Value: {V}", v))
    .ToString();
```

### Task Extensions

```csharp
// Retry with exponential backoff and exception filtering
var response = await TaskEx.RetryAsync(
    () => httpClient.GetAsync(url),
    maxRetries: 3,
    exceptionFilter: ex => ex is HttpRequestException);
```

### Custom Exceptions

```csharp
using Grondo.Exceptions;

throw new EntityNotFoundException("User not found");
// .StatusCode = 404, .MessageHeader = "Not found"
```

## Project Structure

```
Grondo.sln
├── src/                        # Main library (Grondo)
│   ├── Extensions/             # 28 extension method classes
│   ├── Exceptions/             # 9 HTTP-mapped exception types
│   ├── Utilities/              # JsonDefaults, StringFactory, Environments
│   ├── Result.cs               # Result<T> and Result types
│   └── Maybe.cs                # Maybe<T> optional type
├── tests/                      # MSTest unit tests (664 tests)
│   ├── Extensions/             # Extension method tests
│   ├── Exceptions/             # Exception tests
│   ├── ResultTests.cs          # Result<T>/Result tests
│   └── MaybeTests.cs           # Maybe<T> tests
└── benchmarks/                 # BenchmarkDotNet performance tests
```

## Build and Test

```bash
dotnet build
dotnet test
```

### Benchmarks

A [BenchmarkDotNet](https://benchmarkdotnet.org/) project is included for measuring performance of hot-path extension methods:

```bash
dotnet run --project benchmarks -c Release -- --filter "*StringEx*"
```

## Requirements

- .NET 10.0 SDK or later
- C# 14 (uses extension blocks, `readonly struct`, `INumber<T>`, `[GeneratedRegex]`)
- **ASP.NET Core shared framework** — This library references `Microsoft.AspNetCore.App`

### ASP.NET Core Shared Framework

Grondo uses a `<FrameworkReference>` to the ASP.NET Core shared framework (`Microsoft.AspNetCore.App`), which is installed with the .NET SDK. This is **not** an external NuGet package dependency.

Grondo includes several extension methods that require ASP.NET Core types:

- **`ServiceProviderEx`** — Extensions for `IServiceProvider` (dependency injection)
- **`EnvironmentEx`** — Extensions for `IHostEnvironment` (environment checks)
- **`ConfigurationEx`** — Extensions for `IConfiguration` (configuration access)
- **`HttpEx`** — Extensions for `HttpRequest` (request body reading)

Grondo is primarily designed for **ASP.NET Core applications** (web APIs, MVC apps, Blazor, etc.), but can be used in any .NET 10 application since the shared framework is part of the .NET runtime installation. Most extension methods (strings, collections, dates, `Result<T>`, `Maybe<T>`, etc.) don't require ASP.NET Core and work in console apps, class libraries, and other project types.

## For Maintainers

### Releasing a New Version

Grondo uses an automated release process with GitHub Actions:

```bash
# Update CHANGELOG.md first, then:
./RELEASE.sh <version>
```

**Examples:**
```bash
./RELEASE.sh 1.1.0  # Minor release (new features)
./RELEASE.sh 1.0.1  # Patch release (bug fixes)
./RELEASE.sh 2.0.0  # Major release (breaking changes)
```

**What happens:**
1. Script validates version format and runs tests
2. Prompts for confirmation before committing
3. Creates Git tag (e.g., `v1.1.0`)
4. Prompts for confirmation before pushing
5. GitHub Actions automatically builds, tests, and publishes to NuGet.org

**See [.github/RELEASE_GUIDE.md](.github/RELEASE_GUIDE.md) for complete release documentation.**

### VSCode Tasks

20 VSCode tasks are available for common development workflows:

```bash
# Build (Cmd+Shift+B or Ctrl+Shift+B)
# Test (Cmd+Shift+T or Ctrl+Shift+T)
# Or: Cmd+Shift+P → "Tasks: Run Task"
```

**See [.vscode/TASKS_GUIDE.md](.vscode/TASKS_GUIDE.md) for complete task documentation.**

## Documentation

> 📖 **[gt-downunder.github.io/Grondo](https://gt-downunder.github.io/Grondo/)** — Full API reference with practical code examples for every public method.

- **[CONTRIBUTING.md](CONTRIBUTING.md)** — Guidelines for contributing to this project

## License

This project is licensed under the [GPL-3.0-or-later](LICENSE.md) license.
