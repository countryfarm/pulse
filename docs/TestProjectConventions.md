## Test project conventions

- Copy `tests/Marap.Pulse.Domain.Tests/Factories/EntityTestFactory.cs` into any new test project and add per-entity thin factories that call it.
- Keep test artifacts under each test project's `TestResults` and `coveragereport` folders (scripts already do this).
- Prefer unit tests that exercise domain invariants via factories rather than constructing entities with default or magic values.

This file is a short reference; see `docs/DomainUnitTesting.md` for full guidance.
