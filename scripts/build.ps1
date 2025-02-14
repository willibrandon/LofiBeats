# Build script for LofiBeats
param(
    [switch]$Release,
    [switch]$NoBuild,
    [switch]$NoTest
)

$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $PSScriptRoot

Write-Host "🎵 Building LofiBeats..." -ForegroundColor Cyan

# Try to gracefully shutdown any running service
Write-Host "Ensuring no service is running..." -ForegroundColor Yellow
$output = & dotnet run --project "$root/src/LofiBeats.Cli" -- shutdown 2>&1
if ($LASTEXITCODE -eq 0) {
    Write-Host "Service stopped gracefully" -ForegroundColor Gray
} else {
    Write-Host "No running service found" -ForegroundColor Gray
}

# Force kill any remaining dotnet processes that might be our service
Get-Process -Name "dotnet" | Where-Object { $_.MainWindowTitle -eq "" } | ForEach-Object {
    try {
        $_.Kill()
        Write-Host "Cleaned up process $($_.Id)" -ForegroundColor Gray
    } catch {
        Write-Host "Failed to clean up process $($_.Id)" -ForegroundColor Red
    }
}

if (-not $NoBuild) {
    # Clean
    Write-Host "Cleaning solution..." -ForegroundColor Yellow
    & dotnet clean "$root/LofiBeats.sln" --verbosity minimal
    if ($LASTEXITCODE -ne 0) { throw "Clean failed" }

    # Restore
    Write-Host "Restoring packages..." -ForegroundColor Yellow
    & dotnet restore "$root/LofiBeats.sln"
    if ($LASTEXITCODE -ne 0) { throw "Restore failed" }

    # Build
    $configuration = if ($Release) { "Release" } else { "Debug" }
    Write-Host "Building solution ($configuration)..." -ForegroundColor Yellow
    & dotnet build "$root/LofiBeats.sln" --configuration $configuration --no-restore
    if ($LASTEXITCODE -ne 0) { throw "Build failed" }
}

if (-not $NoTest) {
    # Run tests
    Write-Host "Running tests..." -ForegroundColor Yellow
    & dotnet test "$root/LofiBeats.sln" --no-build
    if ($LASTEXITCODE -ne 0) { throw "Tests failed" }
}

Write-Host "🎉 Build completed successfully!" -ForegroundColor Green 