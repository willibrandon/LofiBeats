# Run script for LofiBeats
param(
    [Parameter(Position=0, ValueFromRemainingArguments=$true)]
    [string[]]$Arguments
)

$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $PSScriptRoot

# Check if we need to build first
$cliPath = "$root/src/LofiBeats.Cli/bin/Debug/net9.0/LofiBeats.Cli.dll"
if (-not (Test-Path $cliPath)) {
    Write-Host "🔨 Building LofiBeats..." -ForegroundColor Yellow
    & dotnet build "$root/src/LofiBeats.Cli" --verbosity minimal
    if ($LASTEXITCODE -ne 0) { throw "Build failed" }
}

# Run the CLI with any provided arguments
Write-Host "🎵 Running LofiBeats..." -ForegroundColor Cyan
& dotnet exec $cliPath $Arguments

# Note: We don't need to handle errors here since the CLI will do that
# and we want to preserve its exit code
exit $LASTEXITCODE 