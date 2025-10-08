<#
Install a pre-commit hook that runs a quick test check before committing.
This script copies hooks/pre-commit.ps1 into .git/hooks/pre-commit and makes it executable.

Usage (from repo root):
  pwsh -NoProfile -ExecutionPolicy Bypass -File .\tools\tests\Install-PreCommitHook.ps1
#>

function Find-GitRoot {
  param([string]$StartDir = (Get-Location).Path)

  $dir = Get-Item -Path $StartDir
  while ($dir -ne $null) {
    $gitPath = Join-Path $dir.FullName '.git'
    if (Test-Path $gitPath) { return $dir.FullName }
    $dir = $dir.Parent
  }
  return $null
}

$repoRoot = Find-GitRoot -StartDir (Get-Location).Path
if (-not $repoRoot) {
  Write-Error ".git directory not found. Please run this script from inside the repository (any subfolder under the repo) or pass the repo root as argument."
  exit 2
}

$gitHooks = Join-Path $repoRoot '.git\hooks'
$sourceDir = Join-Path $repoRoot 'tools\tests\hooks'
if (-not (Test-Path $sourceDir)) {
  Write-Error "Source hooks directory not found at $sourceDir";
  exit 3
}

if (-not (Test-Path $gitHooks)) {
  # Create hooks folder if it doesn't exist (rare)
  New-Item -ItemType Directory -Path $gitHooks -Force | Out-Null
}

# Files we expect in tools/tests/hooks
$filesToSync = @('pre-commit.ps1', 'pre-commit')

foreach ($f in $filesToSync) {
  $src = Join-Path $sourceDir $f
  if (-not (Test-Path $src)) {
    Write-Warning "Source hook file missing: $src — skipping"
    continue
  }
  $dst = Join-Path $gitHooks $f
  Copy-Item -Path $src -Destination $dst -Force
  Write-Host "Synced: $src -> $dst"

  # Try make shim executable on POSIX-like systems (Git Bash / WSL)
  try {
    if ($IsWindows -eq $false) {
      & chmod +x $dst 2>$null
    }
  } catch {
    # ignore chmod failures on Windows or if chmod not available
  }
}

Write-Host "Pre-commit hooks synchronized to: $gitHooks"