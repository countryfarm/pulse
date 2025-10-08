# How to Unit Test Pulse Domain Entities

This document explains the approach we use for unit testing domain entities in the **Pulse** project. It covers:

- Why we use **entity factories** in tests  
- How the **common factory logic** is centralized in a static helper  
- How to write clean, maintainable unit tests for domain entities  

---

## Why Entity Factories?

Our domain entities are designed with **strongly‑typed IDs** (e.g. `PartId`, `VendorId`, `LocationId`) and EF Core is responsible for assigning those IDs at runtime.  

This means that in unit tests we cannot simply do:

```csharp
var vendor = new Vendor(1, "Acme Supplies"); // ❌ no longer valid
```

Instead, we use **test factories** to create entities with stable IDs for testing. This keeps our tests expressive, avoids reflection hacks in every test, and ensures consistency across the suite.

---

## Common Factory Logic

We created a static helper `EntityTestFactory` that uses reflection to set the `Id` property on any entity. This logic is centralized so we don’t repeat it in every factory.

```csharp
using System.Reflection;
using Marap.Pulse.Domain.Common;

namespace Marap.Pulse.Tests.Factories
{
  public static class EntityTestFactory
  {
    public static TEntity WithId<TEntity, TId>(TEntity entity, TId id)
      where TEntity : Entity<TId>
      where TId : struct
    {
      typeof(TEntity)
        .GetProperty(nameof(Entity<TId>.Id), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!
        .SetValue(entity, id);

      return entity;
    }

    public static TEntity WithNullableId<TEntity, TId>(TEntity entity, TId? id)
      where TEntity : Entity<TId>
      where TId : struct
    {
      typeof(TEntity)
        .GetProperty(nameof(Entity<TId>.Id), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!
        .SetValue(entity, id);

      return entity;
    }
  }
}
```

---

## Example: VendorFactory

Each entity has its own thin factory that delegates to `EntityTestFactory`. For example:

```csharp
using Marap.Pulse.Domain.Entities;
using Marap.Pulse.Domain.Common;

namespace Marap.Pulse.Tests.Factories
{
  public static class VendorFactory
  {
    public static Vendor CreateWithId(VendorId id, string name = "Default Vendor")
    {
      var vendor = new Vendor(name);
      return EntityTestFactory.WithId(vendor, id);
    }
  }
}
```

This keeps test code clean and intention‑revealing.

---

## Example Unit Test

Here’s how we test the `Vendor` entity:

```csharp
using FluentAssertions;
using Marap.Pulse.Domain.Common;
using Marap.Pulse.Tests.Factories;

public class VendorTests
{
  [Fact]
  public void Vendor_ShouldStoreProperties()
  {
    // Arrange
    var vendor = VendorFactory.CreateWithId(VendorId.From(1), "Acme Supplies");

    // Act & Assert
    vendor.Id.Should().Be(VendorId.From(1));
    vendor.Name.Should().Be("Acme Supplies");
  }
}
```

- The **factory** handles ID assignment.  
- The **test** focuses only on verifying domain behavior and invariants.  

---

## Guidelines for Future Tests

1. **Always use factories** to create entities in tests.  
2. **Do not** set IDs directly in tests — let the factory handle it.  
3. **Add new factories** for new entities as they are introduced.  
4. **Keep tests focused** on domain behavior (properties, invariants, methods), not EF Core persistence.  
5. **Use `WithNullableId`** when testing entities with optional foreign keys.  

---

## Benefits

- **Consistency**: All tests follow the same creation pattern.  
- **Maintainability**: If entity constructors change, only factories need updating.  
- **Clarity**: Tests read cleanly and focus on business rules.  
- **Scalability**: Adding new entities and tests is straightforward.  

---

✅ With this approach, our unit tests remain robust, expressive, and aligned with our domain‑driven design.

---

## Test coverage (local workflow)

The CI workflow (`.github/workflows/dotnet.yml`) runs coverage for the solution and enforces a minimum line coverage threshold. To make local developer workflow match CI, we've added small PowerShell helper scripts under `tools/tests` that run tests and coverage from the `tests` folder and place results next to each test project (not in the repository root).

Files added:

- `tools/tests/Run-UnitTests.ps1` — run all test projects under `tests` (no coverage).
- `tools/tests/Run-Coverage.ps1` — run each test project with the XPlat coverage collector, generate a `coveragereport/Summary.txt` next to each test project, and enforce a threshold (default 70%).

How it works

- Each test project's `TestResults` and `coveragereport` are generated in the test project's directory (so build artifacts and coverage files are not written to the solution root).
- The scripts install `dotnet-reportgenerator-globaltool` if not present and use it to create a `TextSummary` report from Cobertura XML produced by the XPlat collector.
- The coverage script parses the generated `Summary.txt` and fails (non-zero exit code) when coverage is below the threshold.

Quick run (PowerShell, from repository root):

```powershell
# Run tests only
.\tools\tests\Run-UnitTests.ps1

# Run coverage and enforce threshold (70% default)
.\tools\tests\Run-Coverage.ps1
```

Notes & customization

- To change the threshold, call `Run-Coverage.ps1 -Threshold 80.0` (for 80%).
- If you prefer to run coverage only for a single test project, you can run `dotnet test <path-to-csproj> --collect:"XPlat Code Coverage" --results-directory <path-to-TestResults>` and then run ReportGenerator manually against the produced `coverage.cobertura.xml`.
- The `tests/.gitignore` already excludes `TestResults/` and common coverage artifacts so they don't get committed.
