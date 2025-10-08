<#
Run unit tests for the repository's test projects.
This script should be executed from the repository root.
It will run tests located under the 'tests' folder.
#>

param(
  [string]$TestsFolder = "tests",
  [string]$Configuration = "Release"
)

$testProjects = Get-ChildItem -Path $TestsFolder -Recurse -Filter "*.csproj" | Select-Object -ExpandProperty FullName
if (-not $testProjects) {
  Write-Error "No test projects found under '$TestsFolder'"
  exit 2
}

foreach ($proj in $testProjects) {
  Write-Host "Running tests for: $proj"
  dotnet test $proj --configuration $Configuration --logger "trx;LogFileName=results.trx" -v minimal
  if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
}

Write-Host "All tests completed successfully."