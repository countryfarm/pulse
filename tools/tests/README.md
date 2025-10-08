# Testing & Coverage helper scripts

This folder contains helper scripts to run tests and coverage locally in a way that mirrors CI.

## Files

- `Run-UnitTests.ps1` — Run all test projects under `tests/` (no coverage).
- `Run-Coverage.ps1` — Run tests with XPlat coverage per test project, produce `TestResults` and `coveragereport` next to each test project and enforce a threshold (default 70%).
- `Run-AggregatedCoverage.ps1` — Run aggregated coverage for the entire solution (mirrors `.github/workflows/dotnet.yml`) and generates a combined summary under `tests/aggregated/coveragereport`.
- `Install-PreCommitHook.ps1` — Installs a Git pre-commit hook to run a fast test/coverage check before commit (optional).
- `hooks/pre-commit.ps1` — Sample pre-commit hook script used by `Install-PreCommitHook.ps1`.

## Usage

Run tests only:

```powershell
.\tools\tests\Run-UnitTests.ps1
```

Run per-project coverage & enforce threshold:

```powershell
.\tools\tests\Run-Coverage.ps1
```

Run aggregated coverage (CI-like):

```powershell
.\tools\tests\Run-AggregatedCoverage.ps1
```

Install pre-commit hook (optional):

```powershell
.\tools\tests\Install-PreCommitHook.ps1
```

## Expected output

- Per-project: `tests/<project>/coveragereport/Summary.txt` and `tests/<project>/TestResults/.../coverage.cobertura.xml`.
- Aggregated: `tests/aggregated/coveragereport/Summary.txt` with a combined TextSummary.

## Notes

- Pre-commit hook is optional and may slow commits if configured to run full coverage; the default hook runs a fast test-only check. Adjust as needed.

## Troubleshooting: Install-PreCommitHook.ps1

- If you see ".git/hooks directory not found", run the installer from anywhere inside the repository — the installer now searches upward from the current working directory to locate the repo root automatically.
- Ensure you have a `.git` directory (i.e., this is a git clone). If the repo is a shallow checkout or inside a container without `.git`, the installer will not find it.
- To install manually, copy `tools/tests/hooks/pre-commit.ps1` to `.git/hooks/pre-commit` and make it executable for your environment.
