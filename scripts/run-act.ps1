# Ensure we're in the repository root
$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$repoRoot = Split-Path -Parent $scriptPath
Set-Location $repoRoot

Write-Host "🚀 Running GitHub Actions workflow locally with Act..." -ForegroundColor Cyan
Write-Host "Using catthehacker/ubuntu:act-latest as the runner image" -ForegroundColor Gray

# Run the test job with verbose output
act -j build -P ubuntu-latest=catthehacker/ubuntu:act-latest

# Check the exit code
if ($LASTEXITCODE -eq 0) {
    Write-Host "`n✅ Workflow completed successfully!" -ForegroundColor Green
} else {
    Write-Host "`n❌ Workflow failed with exit code $LASTEXITCODE" -ForegroundColor Red
} 