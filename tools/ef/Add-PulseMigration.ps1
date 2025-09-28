param(
    [Parameter(Mandatory = $true)]
    [string]$Name
)

# Generate timestamp prefix
$timestamp = Get-Date -Format "yyyyMMdd_HHmm"
$migrationName = "${timestamp}_$Name"

Write-Host "Creating migration: $migrationName"

dotnet ef migrations add $migrationName `
  --project src/Marap.Pulse.Infrastructure `
  --startup-project src/Marap.Pulse.Bootstrap `
  --output-dir Persistence/Migrations