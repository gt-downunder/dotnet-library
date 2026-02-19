---
title: Utilities
layout: default
nav_order: 4
---

# Utilities

---

## Environments

String constants for common environment names.

```csharp
using Grondo.Utilities;

string env = Environments.Local; // "local"
// Also: Environments.Dev, Environments.Test, Environments.Uat, Environments.Prod
```

Typically used with ASP.NET Core host configuration:

```csharp
if (builder.Environment.EnvironmentName == Environments.Prod)
    builder.Services.AddProductionLogging();
```

---

## StringFactory

Generates unique string identifiers.

```csharp
using Grondo.Utilities;

string id = StringFactory.UniqueString;
// e.g. "a3f5b2c8d1e94f6789abcdef01234567" (32 hex digits, no hyphens)

// Each access generates a new value
string id2 = StringFactory.UniqueString; // different from id
```

