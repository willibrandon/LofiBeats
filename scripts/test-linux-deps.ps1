[CmdletBinding()]
param(
    [Parameter(HelpMessage="Distributions to test. If not specified, all distributions will be tested.")]
    [string[]]$DistrosToTest,

    [Parameter(HelpMessage="Automatically clean up Docker images after testing.")]
    [switch]$AutoCleanup,

    [Parameter(HelpMessage="Skip the cleanup prompt and keep all images.")]
    [switch]$KeepImages,

    [Parameter(HelpMessage="Run tests in parallel (experimental).")]
    [switch]$Parallel
)

# Test matrix - Distribution:Tag pairs
$DISTROS = @{
    "debian:12"                = "Debian Bookworm"
    "debian:11"                = "Debian Bullseye"
    "ubuntu:22.04"             = "Ubuntu LTS"
    "ubuntu:23.10"             = "Ubuntu Latest"
    "fedora:39"                = "Fedora Latest"
    "fedora:38"                = "Fedora Previous"
    "almalinux:9"              = "RHEL 9 Compatible"
    "rockylinux:9"             = "RHEL 9 Compatible"
    "archlinux:latest"         = "Arch Linux"
    "manjarolinux/base:latest" = "Manjaro"
}

# If specific distros were provided, filter the hashtable
if ($DistrosToTest) {
    $DISTROS = $DISTROS.GetEnumerator() | Where-Object { $_.Key -in $DistrosToTest } | 
               ForEach-Object { @{$_.Key = $_.Value} }
    
    if ($DISTROS.Count -eq 0) {
        Write-Error "None of the specified distributions were found in the test matrix."
        exit 1
    }
}

# Check if Docker is available
if (-not (Get-Command "docker" -ErrorAction SilentlyContinue)) {
    Write-Error "Docker is not installed or not in PATH"
    exit 1
}

# Check if Docker daemon is running
try {
    docker info | Out-Null
} catch {
    Write-Error "Docker daemon is not running"
    exit 1
}

# Get script directory without changing current directory
$scriptDir = $PSScriptRoot
$installScript = Join-Path $scriptDir "install-linux-deps.sh"
$testScript = Join-Path $scriptDir "test.sh"
$logsDir = Join-Path $scriptDir "logs"
$timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
$logArchive = Join-Path $scriptDir "logs_$timestamp.zip"

# Ensure required files exist
if (-not (Test-Path $installScript)) {
    Write-Error "install-linux-deps.sh not found at: $installScript"
    exit 1
}
if (-not (Test-Path $testScript)) {
    Write-Error "test.sh not found at: $testScript"
    exit 1
}

# Create logs directory
New-Item -ItemType Directory -Force -Path $logsDir | Out-Null

# Function to create test Dockerfile
function Create-Dockerfile {
    param (
        [string]$distro,
        [string]$dockerfilePath
    )
    
    @"
FROM ${distro}

# Install basic requirements for the script
RUN if command -v apt-get > /dev/null; then \
        apt-get update && apt-get install -y bash dos2unix file; \
    elif command -v dnf > /dev/null; then \
        dnf install -y bash dos2unix file; \
    elif command -v pacman > /dev/null; then \
        pacman -Sy --noconfirm bash dos2unix file; \
    fi

# Create working directory
WORKDIR /app

# Copy test scripts
COPY install-linux-deps.sh .
COPY test.sh .
RUN chmod +x install-linux-deps.sh test.sh && \
    dos2unix install-linux-deps.sh test.sh

CMD ["./test.sh"]
"@ | Set-Content -Path $dockerfilePath -NoNewline
}

# Function to run test for a distribution
function Test-Distribution {
    param (
        [string]$distro,
        [string]$description
    )
    
    $test_name = $distro -replace '[:/]', '-'
    
    Write-Host "`nTesting $distro ($description)" -ForegroundColor Yellow
    Write-Host "================================"
    
    # Create Dockerfile in script directory
    $dockerfilePath = Join-Path $scriptDir "Dockerfile.$test_name"
    Create-Dockerfile $distro $dockerfilePath
    
    try {
        # Build the test container
        Write-Host "Building test container..." -ForegroundColor Yellow
        $buildOutput = docker build -t "lofibeats-test-${test_name}" -f $dockerfilePath $scriptDir 2>&1
        $buildOutput | Out-File -FilePath (Join-Path $logsDir "${test_name}-build.log")
        
        if ($LASTEXITCODE -ne 0) {
            Write-Host "Build failed - see logs/${test_name}-build.log" -ForegroundColor Red
            Write-Host "Last 10 lines of build log:" -ForegroundColor Yellow
            Get-Content (Join-Path $logsDir "${test_name}-build.log") | Select-Object -Last 10
            return $false
        }
        
        Write-Host "Build successful" -ForegroundColor Green
        
        # Run the test
        Write-Host "Running installation test..." -ForegroundColor Yellow
        $runOutput = docker run --rm "lofibeats-test-${test_name}" 2>&1
        $runOutput | Out-File -FilePath (Join-Path $logsDir "${test_name}-run.log")
        $testExitCode = $LASTEXITCODE
        
        # Always show the output
        Write-Host "Test output:" -ForegroundColor Cyan
        $runOutput | ForEach-Object { Write-Host "  $_" }
        
        if ($testExitCode -eq 0) {
            Write-Host "Test passed ✅" -ForegroundColor Green
            return $true
        } else {
            Write-Host "Test failed ❌ (Exit code: $testExitCode)" -ForegroundColor Red
            return $false
        }
    }
    finally {
        # Cleanup Dockerfile
        Remove-Item -Path $dockerfilePath -ErrorAction SilentlyContinue
    }
}

# Track results
$RESULTS = @{}
$PASSED = 0
$FAILED = 0

# Run tests for each distribution
if ($Parallel -and (Get-Command Start-ThreadJob -ErrorAction SilentlyContinue)) {
    Write-Host "Running tests in parallel..." -ForegroundColor Yellow
    $jobs = @()
    
    foreach ($distro in $DISTROS.Keys) {
        $jobs += Start-ThreadJob -Name $distro -ScriptBlock {
            param($distro, $description)
            Test-Distribution $distro $description
        } -ArgumentList $distro,$DISTROS[$distro]
    }
    
    foreach ($job in $jobs) {
        $result = Receive-Job -Job $job -Wait
        if ($result) {
            $RESULTS[$job.Name] = "✅ PASSED"
            $PASSED++
        } else {
            $RESULTS[$job.Name] = "❌ FAILED"
            $FAILED++
        }
        Remove-Job -Job $job
    }
} else {
    foreach ($distro in $DISTROS.Keys) {
        if (Test-Distribution $distro $DISTROS[$distro]) {
            $RESULTS[$distro] = "✅ PASSED"
            $PASSED++
        } else {
            $RESULTS[$distro] = "❌ FAILED"
            $FAILED++
        }
    }
}

# Print summary with more detail
Write-Host "`nTest Summary" -ForegroundColor Green
Write-Host "=============="

foreach ($distro in $RESULTS.Keys) {
    $status = $RESULTS[$distro]
    $color = if ($status -eq "✅ PASSED") { "Green" } else { "Red" }
    Write-Host "$distro ($($DISTROS[$distro])): $status" -ForegroundColor $color
}

Write-Host "`n=============="
Write-Host "Total: $($PASSED + $FAILED)"
Write-Host "Passed: $PASSED" -ForegroundColor Green
Write-Host "Failed: $FAILED" -ForegroundColor Red

# Archive logs
Write-Host "`nArchiving logs..." -ForegroundColor Yellow
Compress-Archive -Path $logsDir/* -DestinationPath $logArchive -Force
Write-Host "Logs archived to: $logArchive" -ForegroundColor Green

# Cleanup
Write-Host "`nCleaning up..." -ForegroundColor Yellow
Get-ChildItem -Path $scriptDir -Filter "Dockerfile.*" | Remove-Item

# Handle image cleanup based on parameters
if ($AutoCleanup) {
    Write-Host "Auto-cleaning test images..." -ForegroundColor Yellow
    foreach ($distro in $DISTROS.Keys) {
        $test_name = $distro -replace '[:/]', '-'
        docker rmi "lofibeats-test-${test_name}" 2>&1 | Out-Null
    }
    Write-Host "Cleanup complete" -ForegroundColor Green
}
elseif (-not $KeepImages) {
    $cleanup = Read-Host "`nDo you want to remove the test Docker images? (y/N)"
    if ($cleanup -match "^[Yy]$") {
        Write-Host "Removing test images..." -ForegroundColor Yellow
        foreach ($distro in $DISTROS.Keys) {
            $test_name = $distro -replace '[:/]', '-'
            docker rmi "lofibeats-test-${test_name}" 2>&1 | Out-Null
        }
        Write-Host "Cleanup complete" -ForegroundColor Green
    }
}

# Exit with status based on results
if ($FAILED -eq 0) {
    Write-Host "`nAll tests passed!" -ForegroundColor Green
    exit 0
} else {
    Write-Host "`nSome tests failed. Check logs directory for details." -ForegroundColor Red
    Write-Host "Archived logs available at: $logArchive" -ForegroundColor Yellow
    exit 1
} 