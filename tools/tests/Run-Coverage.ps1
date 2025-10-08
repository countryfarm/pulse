<#
Run tests with coverage for test projects under the 'tests' folder.
This script runs from the repository root but places TestResults and coveragereport under the tests folder.
Usage: .\tools\tests\Run-Coverage.ps1
#>

param(
  [string]$TestsFolder = "tests",
  [string]$Configuration = "Release",
  [double]$Threshold = 70.0
)

$testProjects = Get-ChildItem -Path $TestsFolder -Recurse -Filter "*.csproj" | Select-Object -ExpandProperty FullName
if (-not $testProjects) {
  Write-Error "No test projects found under '$TestsFolder'"
  exit 2
}

# Ensure tools path is available in this session
$dotnetTools = Join-Path $env:USERPROFILE '.dotnet\tools'
if (-not ($env:PATH -like "*${dotnetTools}*")) {
  $env:PATH = $env:PATH + ";" + $dotnetTools
}

foreach ($proj in $testProjects) {
  $projDir = Split-Path $proj -Parent
  $resultsDir = Join-Path $projDir 'TestResults'
  if (-Not (Test-Path $resultsDir)) { New-Item -ItemType Directory -Path $resultsDir | Out-Null }

  Write-Host "Running tests with coverage for: $proj"
  dotnet test $proj --no-build --configuration $Configuration --collect:"XPlat Code Coverage" --results-directory $resultsDir
  if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

  # Generate report for this project
  if (-not (Get-Command reportgenerator -ErrorAction SilentlyContinue)) {
    Write-Host "Installing reportgenerator tool..."
    dotnet tool install --global dotnet-reportgenerator-globaltool
  }

  $reportDir = Join-Path $projDir 'coveragereport'
  reportgenerator -reports:"$resultsDir\**\coverage.cobertura.xml" -targetdir:"$reportDir" -reporttypes:TextSummary | Out-Null

  $summaryFile = Join-Path $reportDir 'Summary.txt'
  if (-not (Test-Path $summaryFile)) {
    Write-Error "Coverage summary not generated for $proj"
    exit 3
  }

  $summary = Get-Content $summaryFile -Raw
  if ($summary -match 'Line coverage:\s*([0-9]+(?:\.[0-9]+)?)') {
    $coverage = [double]$matches[1]
  Write-Host "Detected line coverage for $proj - $coverage%"
    if ($coverage -lt $Threshold) {
      Write-Error "Coverage $coverage is below threshold $Threshold for project $proj"
      exit 1
    }
  } else {
    Write-Error "Could not parse coverage summary for $proj"
    exit 4
  }
}

Write-Host "All coverages meet the threshold of $Threshold%."