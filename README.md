# pulse
Always in stock. Always on time.

[![Build](https://github.com/countryfarm/pulse/actions/workflows/dotnet.yml/badge.svg)](https://github.com/countryfarm/pulse/actions/workflows/dotnet.yml)

Here’s a concise, developer-friendly snippet you can drop straight into your repo’s **README.md**. It explains how to configure the connection string safely without committing secrets:

---

## Local Database Setup

The Pulse app uses **PostgreSQL** for persistence. To keep credentials safe, we don’t commit passwords or full connection strings into source control. Each developer configures their own local connection string using either **environment variables** or **.NET user secrets**.

### 1. Environment variable (recommended for containers / CI)
Set an environment variable named `PULSE_DB_CONNECTION`:

**Linux / macOS (bash/zsh):**
```bash
export PULSE_DB_CONNECTION="Host=raspberrypi.local;Port=5432;Database=pulse;Username=pulse_user;Password=yourpassword"
```

**Windows (PowerShell):**
```powershell
$env:PULSE_DB_CONNECTION="Host=raspberrypi.local;Port=5432;Database=pulse;Username=pulse_user;Password=yourpassword"
```

### 2. .NET User Secrets (recommended for local dev)
From the Infrastructure project folder, run:

```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:PulseDb" "Host=raspberrypi.local;Port=5432;Database=pulse;Username=pulse_user;Password=yourpassword"
```

This stores the secret outside the repo, in your user profile.

### 3. Application configuration
The app will automatically pick up the connection string from either:
- `ConnectionStrings:PulseDb` (user secrets), or
- `PULSE_DB_CONNECTION` (environment variable).

No passwords should ever be committed to `appsettings.json`.

---

### Sample appsettings.json file

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    // Do not put passwords here!
    // Use environment variables or user secrets to override this value.
    "PulseDb": "Host=192.168.1.68;Port=5432;Database=pulse"
  }
}
```