param(
    [Parameter()]
    [int]$RepeatCount = 1
)

# Ensure we're in the repository root
$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$repoRoot = Split-Path -Parent $scriptPath
Set-Location $repoRoot

Write-Host "🐳 Building Docker test image..." -ForegroundColor Cyan
docker build -t lofibeats-test -f Dockerfile.test .

Write-Host "`n🧪 Running tests in Linux container $RepeatCount time(s)..." -ForegroundColor Cyan

for ($i = 1; $i -le $RepeatCount; $i++) {
    Write-Host "`n📋 Test Run $i of $RepeatCount" -ForegroundColor Yellow
    Write-Host "----------------------------------------" -ForegroundColor Yellow
    
    docker run --rm --init lofibeats-test dotnet test --no-build --filter OpenALAudioOutputTests --logger "console;verbosity=minimal" /p:ConsoleOutputLoggerMinimalMessages=true
    
    Write-Host "----------------------------------------" -ForegroundColor Yellow
    
    if ($LASTEXITCODE -ne 0) {
        Write-Host "❌ Test run $i failed with exit code $LASTEXITCODE" -ForegroundColor Red
    } else {
        Write-Host "✅ Test run $i completed successfully" -ForegroundColor Green
    }
}

Write-Host "`n🏁 All test runs completed" -ForegroundColor Cyan 