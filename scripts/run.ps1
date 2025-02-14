# Run script for LofiBeats
param(
    [Parameter(Position=0, ValueFromRemainingArguments=$true)]
    [string[]]$Arguments
)

$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $PSScriptRoot

# Run the CLI with any provided arguments
Write-Host "🎵 Running LofiBeats..." -ForegroundColor Cyan
& dotnet run --project "$root/src/LofiBeats.Cli" -- $Arguments

# Note: We don't need to handle errors here since the CLI will do that
# and we want to preserve its exit code
exit $LASTEXITCODE 