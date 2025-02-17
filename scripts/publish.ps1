# Publish script for LofiBeats
param(
    [Parameter(Mandatory=$true)]
    [ValidateSet("win-x64", "linux-x64", "linux-arm64", "osx-x64")]
    [string]$Runtime,
    [string]$Version = "1.0.0"
)

$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $PSScriptRoot

Write-Host "🎵 Publishing LofiBeats v$Version for $Runtime..." -ForegroundColor Cyan

# First run our build script
Write-Host "Running build script..." -ForegroundColor Yellow
& "$PSScriptRoot/build.ps1" -Release
if ($LASTEXITCODE -ne 0) { throw "Build failed" }

# Create publish directory
$publishDir = "$root/publish/$Runtime"
if (Test-Path $publishDir) {
    Remove-Item -Path $publishDir -Recurse -Force
}
New-Item -ItemType Directory -Path $publishDir | Out-Null

# Publish CLI and Service
$projects = @(
    "src/LofiBeats.Cli",
    "src/LofiBeats.Service"
)

# Determine if we should use single-file publishing
$isSingleFile = $Runtime -eq "linux-arm64"
$publishFlags = @(
    "--configuration Release",
    "--runtime $Runtime",
    "--self-contained false",
    "--output `"$publishDir`"",
    "-p:Version=$Version",
    "-p:UseAppHost=true",
    "-p:DebugType=embedded"
)

if ($isSingleFile) {
    $publishFlags += "-p:PublishSingleFile=true"
    $publishFlags += "-p:IncludeNativeLibrariesForSelfExtract=true"
    $publishFlags += "-p:EmbedAllSources=true"
    $publishFlags += "-p:DebugSymbols=true"
}

foreach ($project in $projects) {
    Write-Host "Publishing $project..." -ForegroundColor Yellow
    $command = "dotnet publish `"$root/$project`" $($publishFlags -join ' ')"
    Write-Host "Running: $command" -ForegroundColor DarkGray
    Invoke-Expression $command
    if ($LASTEXITCODE -ne 0) { throw "Publish failed for $project" }
}

# Verify the publish output
if ($isSingleFile) {
    # For single-file publishing, verify the executables exist
    $cliExe = "$publishDir/LofiBeats.Cli"
    $serviceExe = "$publishDir/LofiBeats.Service"
    if (-not (Test-Path $cliExe) -or -not (Test-Path $serviceExe)) {
        Write-Warning "Missing executables in publish output"
        throw "Publish output verification failed"
    }
} else {
    # For multi-file publishing, verify runtime config
    $runtimeConfig = "$publishDir/LofiBeats.Service.runtimeconfig.json"
    if (-not (Test-Path $runtimeConfig)) {
        Write-Warning "Runtime config not found at $runtimeConfig"
        throw "Missing runtime configuration file"
    }
}

# Create archive
$archiveName = "lofibeats-$Version-$Runtime"
$archivePath = "$root/publish/$archiveName"

Write-Host "Creating archive..." -ForegroundColor Yellow
if ($Runtime -like "win*") {
    Compress-Archive -Path "$publishDir/*" -DestinationPath "$archivePath.zip" -Force
    Write-Host "Created $archivePath.zip" -ForegroundColor Green
} else {
    # For Linux/macOS, we'll create a tar.gz
    tar -czf "$archivePath.tar.gz" -C $publishDir .
    Write-Host "Created $archivePath.tar.gz" -ForegroundColor Green
}

Write-Host "🎉 Publish completed successfully!" -ForegroundColor Green 