# Repository conventions and assistant context

## Purpose

This document captures coding and repository conventions for the Pulse project. It is intended for developers, reviewers, and automated assistants that interact with the repository.

## Quick contract

- Indentation: 2 spaces.
- File-level namespaces: each C# file must use the file-scoped namespace declaration.
- Formatting: UTF-8, LF line endings, trim trailing whitespace (except in Markdown).
- Build/test: .NET 9 SDK, xUnit tests, run with `dotnet test`.
- Pre-commit: a lightweight pre-commit hook lives in `tools/tests/hooks` and can be installed with `tools/tests/Install-PreCommitHook.ps1`.

## Naming conventions

- Projects and folders: PascalCase (e.g. `Marap.Pulse.Domain`).
- Types (classes, structs, enums, interfaces): PascalCase (interfaces prefixed with `I`).
- Methods and properties: PascalCase.
- Fields: `_camelCase` for private fields (prefer `readonly` when possible).
- Local variables and parameters: camelCase.
- Test names: Use descriptive names that follow the pattern `MethodName_StateUnderTest_ExpectedBehavior` or natural language with Arrange/Act/Assert sections.

## C# style

- Use two-space indentation.
- Prefer `var` when the type is obvious from the right-hand side; otherwise prefer explicit type.
- Use expression-bodied members for trivial getters and single-statement members where it improves readability.
- Use file-scoped namespaces (C# 10+):

```csharp
namespace Marap.Pulse.Domain;
```

- Keep `using` directives at the top of the file; use global usings for commonly used namespaces in `GlobalUsings.cs`.
- Prefer immutable domain objects and value objects. Use Vogen for strong typed value objects when appropriate.

## Architecture and patterns

- Domain-Driven Design is the core approach:
  - Keep a rich domain model in `src/Marap.Pulse.Domain`.
  - Entities, ValueObjects, Events, Services are separated into folders.

- Clean Architecture boundaries are encouraged:
  - Keep infrastructure code (EF Core, migrations, persistence) in `src/Marap.Pulse.Infrastructure`.
  - The API/Bootstrapping pieces live in `src/Marap.Pulse.Bootstrap`.

- Avoid leaking infrastructure concerns into the domain layer. Domain projects should have no external dependencies except well-scoped packages that are domain-relevant.

## Testing

- xUnit is used for unit tests.
- Keep tests fast and focused. Use test factories under `tests/.../Factories` when appropriate.
- Tests should be deterministic and not depend on local state or time (use fakes or test helpers).
- Coverage: the project contains scripts to compute per-project and aggregated coverage under `tools/tests`. The CI pipeline enforces a threshold; keep per-project thresholds reasonable.

## Pre-commit hooks and developer tooling

- The repository includes a pre-commit hook script in `tools/tests/hooks/pre-commit.ps1` and an installer `tools/tests/Install-PreCommitHook.ps1`.

Developers can install the hook by running the installer from the repo root:

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File .\tools\tests\Install-PreCommitHook.ps1
```

- The hook runs a small, fast set of unit tests and aborts the commit if they fail.

## Commit message and PR guidance

- Keep commits small and focused. Reference issue/ticket IDs in the PR description when applicable.
- Add tests for behavior changes and new domain logic.

## Automation and CI

- The CI workflow runs coverage and report generation; local scripts `tools/tests/Run-Coverage.ps1` and `Run-AggregatedCoverage.ps1` reproduce the pipeline locally.

## Notes for assistants

- Always respect file-level namespaces and two-space indentation when generating C# code.
- When adding files, try to match repository layout (src/, tests/, tools/).
- If you modify hooks or installer scripts, update `tools/tests/README.md` with the usage notes.

## Contact

- If conventions need to change, propose them in a PR and document the rationale in `docs/RepoConventions.md`.
