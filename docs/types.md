---
title: Types
layout: default
nav_order: 5
---

# Types

---

## Result\<T\>

A `readonly struct` for railway-oriented programming. An operation either succeeds with a value
or fails with an error message — no exceptions needed for expected failures.

### Creating Results

```csharp
// Explicit factory methods
Result<int> ok  = Result<int>.Success(42);
Result<int> err = Result<int>.Failure("Something went wrong");

// Implicit conversion from value
Result<int> implicit = 42;

// Wrap any value
Result<string> wrapped = "hello".ToResult();

// Safe execution (catches exceptions)
Result<int> safe = ResultEx.TryExecute(() => int.Parse("abc"));
// Failure("Input string was not in a correct format.")

// Async safe execution
Result<string> data = await ResultEx.TryExecuteAsync(
    () => httpClient.GetStringAsync("/api/data"));
```

### Inspecting Results

```csharp
Result<int> result = GetResult();

if (result.IsSuccess)
    Console.WriteLine(result.Value);   // access the value

if (result.IsFailure)
    Console.WriteLine(result.Error);   // access the error message

int fallback = result.GetValueOrDefault(-1); // value or fallback

string display = result.ToString(); // "Success(42)" or "Failure(error)"
```

### Railway-Oriented Pipeline (Synchronous)

```csharp
Result<Order> result = GetUserId()              // Result<int>
    .Ensure(id => id > 0, "Invalid user ID")    // validate
    .Map(id => new OrderRequest(id))             // transform value
    .Bind(req => PlaceOrder(req))                // chain to another Result
    .Tap(order => logger.Log($"Order {order.Id} placed"))  // side-effect on success
    .TapError(err => logger.LogWarning(err))     // side-effect on failure
    .MapError(err => $"Order failed: {err}");    // transform error message

// Pattern match to extract the final value
string message = result.Match(
    onSuccess: order => $"Order #{order.Id} confirmed",
    onFailure: error => $"Error: {error}");
```

### Railway-Oriented Pipeline (Async)

Instance async methods (when you already have a `Result<T>`):

```csharp
Result<int> userId = GetUserId();

Result<Order> order = await userId
    .MapAsync(id => FetchUserAsync(id))          // async transform
    .BindAsync(user => CreateOrderAsync(user))   // async chain
    .TapAsync(o => SendConfirmationAsync(o))     // async side-effect
    .TapErrorAsync(e => LogErrorAsync(e));        // async error side-effect
```

Fluent extensions on `Task<Result<T>>` (full async pipeline):

```csharp
Result<OrderConfirmation> confirmation = await GetUserIdAsync()    // Task<Result<int>>
    .EnsureAsync(id => id > 0, "Invalid ID")
    .MapAsync(id => LookupUserAsync(id))
    .BindAsync(user => PlaceOrderAsync(user))
    .TapAsync(order => NotifyAsync(order))
    .TapErrorAsync(err => AlertOpsAsync(err))
    .MapErrorAsync(err => $"Pipeline failed: {err}");

// Async pattern match
string msg = await GetUserIdAsync()
    .MatchAsync(
        onSuccess: id => $"Found user {id}",
        onFailure: err => $"Error: {err}");
```

### Combining Results

```csharp
Result<string> name  = Result<string>.Success("Alice");
Result<int>    age   = Result<int>.Success(30);

// Combine two results into a tuple
Result<(string, int)> combined = ResultEx.Combine(name, age);
// Success(("Alice", 30))

// Combine a collection
var results = new[] { Result<int>.Success(1), Result<int>.Success(2) };
Result<IReadOnlyList<int>> all = ResultEx.Combine(results);
// Success([1, 2])

// If any fails, the first error propagates
var mixed = new[] { Result<int>.Success(1), Result<int>.Failure("bad") };
Result<IReadOnlyList<int>> fail = ResultEx.Combine(mixed);
// Failure("bad")
```

---

## Result (non-generic)

For void operations that succeed or fail but don't return a value.

```csharp
// Create
Result ok  = Result.Success();
Result err = Result.Failure("Operation failed");

// Safe execution
Result safe = ResultEx.TryExecute(() => File.Delete("temp.txt"));
Result asyncSafe = await ResultEx.TryExecuteAsync(() => SendEmailAsync());

// Inspect
if (ok.IsSuccess) Console.WriteLine("Done");
if (err.IsFailure) Console.WriteLine(err.Error);
Console.WriteLine(ok.ToString());  // "Success"
Console.WriteLine(err.ToString()); // "Failure(Operation failed)"

// ROP pipeline
Result result = Result.Success()
    .Tap(() => logger.Log("Starting"))
    .Ensure(() => CanProceed(), "Cannot proceed")
    .TapError(err => logger.LogWarning(err))
    .MapError(err => $"Wrapped: {err}");

// Bind to produce a Result<T>
Result<Order> order = Result.Success()
    .Bind(() => CreateOrder());

// Pattern match
string msg = result.Match(
    onSuccess: () => "All good",
    onFailure: error => $"Failed: {error}");

// Async pipeline on Task<Result>
Result final = await DoWorkAsync()       // Task<Result>
    .TapAsync(() => NotifyAsync())
    .TapErrorAsync(err => LogAsync(err))
    .EnsureAsync(() => IsValid(), "Invalid state")
    .MapErrorAsync(err => $"Pipeline: {err}");

string asyncMsg = await DoWorkAsync()
    .MatchAsync(
        onSuccess: () => "OK",
        onFailure: err => $"Error: {err}");

// Async bind to Result<T>
Result<int> value = await DoWorkAsync()
    .BindAsync(() => ComputeAsync());
```

---

## Maybe\<T\>

A `readonly struct` representing an optional value — similar to `Option` in functional languages.
Use `Maybe<T>` when a value might legitimately be absent (instead of returning `null`).

### Creating

```csharp
Maybe<string> some = Maybe<string>.Some("hello");
Maybe<string> none = Maybe<string>.None;

// Implicit conversion (null → None, non-null → Some)
Maybe<string> fromValue = "hello";        // Some("hello")
Maybe<string> fromNull  = (string?)null;  // None
```

### Inspecting

```csharp
Maybe<int> maybe = Maybe<int>.Some(42);

if (maybe.HasValue)
    Console.WriteLine(maybe.Value);  // 42

if (maybe.HasNoValue)
    Console.WriteLine("No value");

int safe = maybe.GetValueOrDefault(-1); // 42 (or -1 if None)
Console.WriteLine(maybe.ToString());    // "Some(42)" or "None"
```

### Transformations

```csharp
Maybe<string> name = Maybe<string>.Some("  Alice  ");

// Map — transform the inner value
Maybe<string> trimmed = name.Map(n => n.Trim()); // Some("Alice")

// Bind — chain to another Maybe
Maybe<User> user = name.Bind(n => FindUserByName(n)); // Some(User) or None

// Where — filter
Maybe<string> long = name.Where(n => n.Length > 10); // None (too short)

// Execute — side-effect (does nothing if None)
name.Execute(n => Console.WriteLine($"Hello, {n}!"));
```

### Pattern Matching

```csharp
Maybe<int> age = Maybe<int>.Some(25);

string message = age.Match(
    some: a => $"Age is {a}",
    none: () => "Age unknown");
// "Age is 25"
```

### Bridge to Result\<T\>

```csharp
Maybe<User> maybeUser = FindUser(42);

// Convert Maybe to Result — None becomes a Failure
Result<User> result = maybeUser.ToResult("User not found");

// Then continue with the full Result pipeline
string output = result
    .Map(u => u.Name)
    .Match(
        onSuccess: name => $"Found: {name}",
        onFailure: err => err);
```

