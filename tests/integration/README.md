Integration tests — quick start

This folder contains a small scaffold to run shallow (in-memory/file) and deep (Postgres container) integration tests against the running API.

Shallow tests (fast)
- Run the API locally (it uses the connection string from `PULSE_DB_CONNECTION` environment variable or `appsettings.json`).
- To test without Postgres, set `PULSE_DB_CONNECTION` to an in-memory or file-based provider connection (see below) or configure a test DbContext in your test project to use SQLite in-memory.

Deep tests (Postgres container)
- Start Postgres with the provided docker-compose (ARM-compatible image where possible):

  cd tests/docker
  docker compose -f docker-compose.postgres.yml up -d

- Wait for the database to be ready. The docker-compose uses DB: pulsedb, user: pulse, password: pulse, listening on host port 5432.

- Set environment variable for API (PowerShell):

  $env:PULSE_DB_CONNECTION = "Host=localhost;Port=5432;Database=pulsedb;Username=pulse;Password=pulse"

- Start the API (from `src/Marap.Pulse.Api`):

  dotnet run --project src/Marap.Pulse.Api/Marap.Pulse.Api.csproj

- Use the `src/Marap.Pulse.Api/requests/PurchaseOrders.http` file with the VS Code REST Client extension or import to Postman to exercise the API endpoints.

Notes
- The project is designed so integration tests can be run either using TestServer/WebApplicationFactory with an in-memory DB or with an actual Postgres instance. Keep the tests shallow where possible to avoid flaky CI runs. Deep tests are useful to validate full EF mappings and infra concerns on target platforms (Raspberry Pi).