Write-Host "Applying latest migrations to database..."

dotnet ef database update `
  --project src/Marap.Pulse.Infrastructure `
  --startup-project src/Marap.Pulse.Bootstrap