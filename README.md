# Grondo

[![NuGet](https://img.shields.io/nuget/v/Grondo.svg)](https://www.nuget.org/packages/Grondo)
[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)

A C# 14 / .NET 10 library providing extension methods, utilities, and custom exception types.

## Installation

```bash
dotnet add package Grondo
```

## Features

### Extension Methods

| Class | Description |
|---|---|
| `BooleanEx` | Boolean-to-string conversions (Yes/No, Active/Inactive, etc.) |
| `ByteArrayEx` | Hex/Base64 encoding, SHA-256 hashing for byte arrays |
| `ClaimsPrincipalEx` | Claim extraction helpers for `ClaimsPrincipal` |
| `ConfigurationEx` | Typed value retrieval from `IConfiguration` |
| `DateTimeEx` | Formatting, parsing, week arithmetic, start/end of day/month, relative time |
| `DateTimeOffsetEx` | Mirrors `DateTimeEx` for `DateTimeOffset` |
| `DictionaryEx` | JSON serialization, key lookups, deep equality, merge, get-or-add |
| `EnumEx` | Description attributes and display name helpers |
| `EnumerableEx` | `IsEmpty`, `ForEach`, `Batch`, `Partition`, `WhereNotNull` |
| `EnvironmentEx` | Hosting environment checks (IsLocal, IsTest, IsUat) |
| `ExceptionEx` | Flatten inner exceptions, extract full detail strings |
| `GuardEx` | Fluent guard clauses for argument validation |
| `GuidEx` | `IsEmpty` check, short-string encoding |
| `HttpEx` | `EnsureObject<T>` deserialization, raw body extraction from `HttpRequest` |
| `JsonEx` | JSON serialize/deserialize, `TryFromJson`, indented output |
| `ListEx` | Null-safe add, deduplication, case-insensitive contains, array initializers |
| `NumericEx` | `IsZero`, `IsPositive`, `IsNegative` for any `INumber<T>` |
| `ObjectEx` | Null/empty checks, type conversions (int, long, decimal, double, bool, DateTime, Guid) |
| `SemaphoreSlimEx` | `LockAsync` returning `IAsyncDisposable` for scoped locking |
| `ServiceProviderEx` | Scoped service resolution from `IServiceProvider` |
| `SetEx` | Compare sets against serialized JSON |
| `SpanEx` | `ContainsIgnoreCase`, `IsNumeric`, `SafeTrim` for `ReadOnlySpan<char>` |
| `StreamEx` | `ReadAllBytesAsync`, `CopyToStringAsync` for streams |
| `StringEx` | Email validation, case-insensitive ops, truncate, Base64, mask, slug, regex |
| `TaskEx` | `RetryAsync` with exponential backoff, `FireAndForget`, `WhenAll` helpers |
| `TimeSpanEx` | `ToHumanReadable` — friendly duration strings |
| `UriEx` | URI inspection and diagnostics |
| `XmlSerializerEx` | XML serialize/deserialize helpers |

### Utilities

| Class | Description |
|---|---|
| `Result<T>` | Operation outcome type — success with value or failure with error message |
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

## Usage

```csharp
using Grondo.Extensions;
using Grondo.Exceptions;

// String extensions
"user@example.com".IsWellFormedEmailAddress(); // true
"Hello World!".Truncate(8);                    // "Hello..."
"secret data".Mask(4);                         // "*******data"
"My Blog Post!".ToSlug();                      // "my-blog-post"
"Hello".ToBase64();                            // "SGVsbG8="

// DateTime extensions
DateTime.Now.StartOfMonth();
DateTime.Now.IsBetween(start, end);
DateTime.Now.AddDays(-3).ToRelativeTime();     // "3 days ago"

// Collection extensions
new[] { 1, 2, 3, 4, 5 }.Batch(2);             // [[1,2], [3,4], [5]]
items.Partition(x => x.IsActive);              // (Matches, NonMatches)
nullableList.WhereNotNull();

// Dictionary extensions
dict.Merge(otherDict, overwrite: true);
dict.GetOrAdd("key", () => ComputeValue());

// Task extensions — retry with exponential backoff
var result = await TaskEx.RetryAsync(() => httpClient.GetAsync(url), maxRetries: 3);

// Guard clauses
GuardEx.Against.NullOrEmpty(name);
GuardEx.Against.OutOfRange(age, 0, 150);

// Result type
Result<User> result = Result<User>.Success(user);
if (result.IsSuccess) Console.WriteLine(result.Value.Name);

// Custom exceptions with HTTP status codes
throw new EntityNotFoundException("User not found");
// .StatusCode = 404, .MessageHeader = "Not found"
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

- .NET 10.0 or later

## License

This project is licensed under the [GPL-3.0-or-later](LICENSE.md) license.