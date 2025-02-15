# Ensure we're in the repository root
$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$repoRoot = Split-Path -Parent $scriptPath
Set-Location $repoRoot

Write-Host "🐳 Building Docker test image..." -ForegroundColor Cyan
docker build -t lofibeats-test -f Dockerfile.test .

Write-Host "`n🧪 Running tests in Linux container..." -ForegroundColor Cyan
docker run --rm --init lofibeats-test dotnet test --no-build --logger "console;verbosity=minimal" /p:ConsoleOutputLoggerMinimalMessages=true 