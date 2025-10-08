#!/usr/bin/env pwsh
# Sample pre-commit hook: runs a quick, fast set of unit tests before allowing commit.
# This avoids running full coverage on every commit (too slow), but catches obvious failures.
#!/usr/bin/env pwsh
# Sample pre-commit hook: runs a quick, fast set of unit tests before allowing commit.
# This avoids running full coverage on every commit (too slow), but catches obvious failures.

try {
  # Determine hook directory and repo root (hook is placed in .git/hooks)
  $hookPath = $MyInvocation.MyCommand.Path
  $hookDir = Split-Path -Parent $hookPath
  $repoRoot = Resolve-Path -Path (Join-Path $hookDir '..\..')
} catch {
  Write-Host "Cannot determine repository root; aborting commit." -ForegroundColor Red
  exit 1
}

Write-Host "Running fast unit tests before commit..."

$proj = Join-Path $repoRoot 'tests\Marap.Pulse.Domain.Tests\Marap.Pulse.Domain.Tests.csproj'

if (-not (Test-Path $proj)) {
  Write-Host "Test project not found at: $proj" -ForegroundColor Red
  exit 1
}

# Run dotnet test, capture output to a temp log, and only print the log on failure
$log = Join-Path $env:TEMP "precommit-hook-output.txt"
if (Test-Path $log) { Remove-Item $log -Force }
& dotnet test $proj --no-build --configuration Debug --logger "trx;LogFileName=precommit.trx" -v minimal 2>&1 | Tee-Object -FilePath $log
$rc = $LASTEXITCODE

if ($rc -ne 0) {
  Write-Host "--- hook output (last 200 lines) ---"
  Get-Content $log -Tail 200 | ForEach-Object { Write-Host $_ }
  Write-Host "--- end hook output ---"
  Write-Host "Unit tests failed. Aborting commit." -ForegroundColor Red
  exit $rc
}

# Clean up log on success
if (Test-Path $log) { Remove-Item $log -Force }
Write-Host "Fast unit tests passed. Proceeding with commit."
exit 0
exit 0
