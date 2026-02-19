---
title: Custom Exceptions
layout: default
nav_order: 3
---

# Custom Exceptions

Grondo provides a hierarchy of HTTP-aware domain exceptions that extend `ExceptionBase`.
Each exception carries an HTTP status code and a short message header.

| Exception | HTTP Status | Default Header |
|---|---|---|
| `BadRequestException` | 400 Bad Request | "Bad Request" |
| `NotAuthorizedException` | 401 Unauthorized | "Not authorized" |
| `ForbiddenException` | 403 Forbidden | "Forbidden" |
| `EntityNotFoundException` | 404 Not Found | "Not found" |
| `ConflictException` | 409 Conflict | "Conflict" |
| `BusinessException` | 409 Conflict | "Business rule violation" |
| `DuplicateFoundException` | 409 Conflict | "Duplicate found" |
| `TechnicalException` | 500 Internal Server Error | "Internal server error" |
| `MethodNotAvailableException` | 501 Not Implemented | "Method not available" |

---

## Throwing and Catching

```csharp
// Throw with a custom message
throw new EntityNotFoundException("User with ID 42 was not found");

// Throw with custom message and header
throw new BadRequestException("Email is invalid", "Validation Error");

// Throw with inner exception
throw new TechnicalException("Database timeout", innerException: ex);

// TechnicalException has a parameterless constructor (uses defaults)
throw new TechnicalException();
// Message: "Please contact the system administrator"
// Header:  "Internal server error"
```

---

## Converting to ErrorResponse

```csharp
try
{
    throw new ForbiddenException("You do not have access to this resource");
}
catch (ExceptionBase ex)
{
    // ExceptionBase properties
    HttpStatusCode code = ex.StatusCode;       // 403
    string header       = ex.MessageHeader;    // "Forbidden"
    string message      = ex.Message;          // "You do not have access..."

    // Convert to a serializable DTO
    ErrorResponse response = ex.ToErrorResponse();
    // response.Message       → "You do not have access to this resource"
    // response.MessageHeader → "Forbidden"

    return Results.Json(response, statusCode: (int)code);
}
```

---

## ErrorResponse Record

```csharp
// ErrorResponse is a simple record with two required properties:
public record ErrorResponse
{
    public required string Message { get; init; }
    public required string MessageHeader { get; init; }
}

// You can also construct it manually:
var error = new ErrorResponse
{
    Message = "Something went wrong",
    MessageHeader = "Error"
};
```

