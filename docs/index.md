---
title: Home
layout: default
nav_order: 1
---

# Grondo — Comprehensive Usage Guide

A complete reference for every public API in the Grondo library with practical code examples.
{: .fs-6 .fw-300 }

> **Requirements:** .NET 10 · C# 14

[Get started](#getting-started){: .btn .btn-primary .fs-5 .mb-4 .mb-md-0 .mr-2 }
[View on GitHub](https://github.com/gt-downunder/grondo){: .btn .fs-5 .mb-4 .mb-md-0 }

---

## Getting Started

### Installation

```shell
dotnet add package Grondo
```

### Using Directives

```csharp
using Grondo;                // Result<T>, Result, Maybe<T>
using Grondo.Extensions;     // All extension methods
using Grondo.Exceptions;     // ExceptionBase, ErrorResponse, and all domain exceptions
using Grondo.Utilities;      // Environments, StringFactory
```

---

## What's Inside

### [Extension Methods]({% link extensions.md %})

27 extension method groups covering strings, collections, dates, JSON, HTTP, tasks, guards, and more.

### [Custom Exceptions]({% link exceptions.md %})

HTTP-aware domain exceptions with status codes and message headers, plus an `ErrorResponse` DTO.

### [Utilities]({% link utilities.md %})

Environment name constants and unique string generation.

### [Types]({% link types.md %})

`Result<T>`, `Result`, and `Maybe<T>` — railway-oriented programming and optional value types.

