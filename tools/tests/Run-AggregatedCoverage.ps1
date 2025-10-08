<#
Run an aggregated coverage run for the entire solution that mirrors the CI job.
This creates a combined coverage report (TextSummary) and enforces a threshold.

Outputs are placed under `tests/aggregated/` to avoid cluttering the solution root.

Usage (from repo root):
  pwsh -NoProfile -ExecutionPolicy Bypass -File .\tools\tests\Run-AggregatedCoverage.ps1
#>

param(
  [string]$Solution = "Pulse.sln",
  [string]$ResultsDir = "tests/aggregated/TestResults",
  [string]$ReportDir = "tests/aggregated/coveragereport",
  [string]$Configuration = "Release",
  [double]$Threshold = 70.0
)

Write-Host "Running aggregated coverage for solution: $Solution"

dotnet restore $Solution
dotnet build $Solution --no-restore --configuration $Configuration

if (-Not (Test-Path $ResultsDir)) { New-Item -ItemType Directory -Path $ResultsDir | Out-Null }
if (-Not (Test-Path $ReportDir)) { New-Item -ItemType Directory -Path $ReportDir | Out-Null }

dotnet test $Solution --no-build --configuration $Configuration --collect:"XPlat Code Coverage" --results-directory $ResultsDir
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

# Ensure reportgenerator is available
$dotnetTools = Join-Path $env:USERPROFILE '.dotnet\tools'
if (-not ($env:PATH -like "*${dotnetTools}*")) {
  $env:PATH = $env:PATH + ";" + $dotnetTools
}
if (-not (Get-Command reportgenerator -ErrorAction SilentlyContinue)) {
  Write-Host "Installing reportgenerator tool..."
  dotnet tool install --global dotnet-reportgenerator-globaltool
}

reportgenerator -reports:"$ResultsDir\**\coverage.cobertura.xml" -targetdir:"$ReportDir" -reporttypes:TextSummary | Out-Null

$summaryFile = Join-Path $ReportDir 'Summary.txt'
if (-not (Test-Path $summaryFile)) { Write-Error "Coverage summary not generated"; exit 3 }

$summary = Get-Content $summaryFile -Raw
Write-Host "--- Coverage summary ---`n$summary`n--- End summary ---"

if ($summary -match 'Line coverage:\s*([0-9]+(?:\.[0-9]+)?)') {
  $coverage = [double]$matches[1]
  Write-Host "Detected line coverage: $coverage%"
  if ($coverage -lt $Threshold) {
    Write-Error "Coverage $coverage is below threshold $Threshold"
    exit 1
  }
} else {
  Write-Error "Could not parse coverage summary"
  exit 4
}

Write-Host "Aggregated coverage OK (>= $Threshold%). Reports in: $ReportDir"
