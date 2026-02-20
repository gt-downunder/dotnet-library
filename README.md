# Grondo

[![NuGet](https://img.shields.io/nuget/v/Grondo.svg)](https://www.nuget.org/packages/Grondo)
[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)

A zero-dependency C# 14 / .NET 10 library providing extension methods, utilities, and custom exception types. Designed around `readonly struct` value types, railway-oriented programming, and modern C# 14 extension blocks.

## Installation

```bash
dotnet add package Grondo
```

## Features

### Extension Methods

| Class | Methods | Description |
|---|---|---|
| `BooleanEx` | `ToYesNo`, `ToActiveInactive`, … | Boolean-to-string conversions |
| `ByteArrayEx` | `ToHex`, `ToBase64`, `ToSha256` | Hex/Base64 encoding, SHA-256 hashing for byte arrays |
| `ClaimsPrincipalEx` | `GetClaim`, `GetClaimValue` | Claim extraction helpers for `ClaimsPrincipal` |
| `ConfigurationEx` | `GetValue<T>`, `GetSection<T>` | Typed value retrieval from `IConfiguration` |
| `DateTimeEx` | `StartOfDay`, `EndOfDay`, `StartOfMonth`, `ToRelativeTime`, … | Formatting, parsing, week arithmetic, start/end of day/month, relative time |
| `DateTimeOffsetEx` | *(mirrors `DateTimeEx`)* | Same set of extensions for `DateTimeOffset` |
| `DictionaryEx` | `ToJson`, `Merge`, `GetOrAdd`, `ToQueryString`, `IsDeepEqualTo`, … | JSON serialization, key lookups, deep equality, merge, get-or-add, query strings |
| `EnumEx` | `GetDescription`, `GetDisplayName` | Description attributes and display name helpers |
| `EnumerableEx` | `Batch`, `Partition`, `Interleave`, `Scan`, `Window`, `Shuffle`, `FallbackIfEmpty`, `Pairwise`, `TagFirstLast`, `WhereNotNull`, … | Rich LINQ-style collection operations |
| `EnvironmentEx` | `IsLocal`, `IsTest`, `IsUat` | Hosting environment checks |
| `ExceptionEx` | `Flatten`, `ToDetailString` | Flatten inner exceptions, extract full detail strings |
| `GuardEx` | `ThrowIfNull`, `ThrowIfNullOrWhiteSpace`, `ThrowIfDefault`, `ThrowIfEmpty`, `ThrowIfNegative`, `ThrowIfZero`, `ThrowIfOutOfRange`, `ThrowIf`, `ThrowIfInvalidFormat` | Fluent guard clauses for argument validation |
| `GuidEx` | `IsEmpty`, `ToShortString` | `IsEmpty` check, short-string encoding |
| `HttpEx` | `EnsureObject<T>`, `GetRawBodyAsync` | Deserialization and raw body extraction from `HttpRequest` |
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

### Types

| Type | Description |
|---|---|
| `Result<T>` | Railway-oriented result type (`readonly struct`) — `Map`, `Bind`, `Match`, `Tap`, `TapError`, `MapError`, `Ensure`, async variants (`MapAsync`, `BindAsync`, `TapAsync`, `EnsureAsync`, `MapErrorAsync`, `TapErrorAsync`), `Task<Result<T>>` extensions, `Combine` |
| `Result` | Non-generic result for void operations (`readonly struct`) — `Tap`, `TapError`, `MapError`, `Ensure`, `Match`, `Bind<T>`, async variants, `Task<Result>` extensions |
| `Maybe<T>` | Optional value type (`readonly struct`) — `Some`, `None`, `Map`, `Bind`, `Match`, `Where`, `Execute`, `GetValueOrDefault`, `ToResult`, implicit conversion from `T` |
| `JsonDefaults` | Pre-configured, cached `JsonSerializerOptions` for consistent serialization |
| `StringFactory` | Factory methods for generating string values |
| `Environments` | Standard environment name constants (local, dev, test, uat, prod) |
| `ErrorResponse` | Standardized error response with message and header |

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
| `TechnicalException` | 500 |
| `MethodNotAvailableException` | 501 |

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

## Documentation

> 📖 **[gt-downunder.github.io/Grondo](https://gt-downunder.github.io/Grondo/)** — Full API reference with practical code examples for every public method.

- **[CONTRIBUTING.md](CONTRIBUTING.md)** — Guidelines for contributing to this project

## License

This project is licensed under the [GPL-3.0-or-later](LICENSE.md) license.
