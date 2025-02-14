# Publish script for LofiBeats
param(
    [Parameter(Mandatory=$true)]
    [ValidateSet("win-x64", "linux-x64", "osx-x64")]
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

foreach ($project in $projects) {
    Write-Host "Publishing $project..." -ForegroundColor Yellow
    & dotnet publish "$root/$project" `
        --configuration Release `
        --runtime $Runtime `
        --self-contained true `
        --output "$publishDir" `
        -p:PublishSingleFile=true `
        -p:Version=$Version `
        -p:IncludeNativeLibrariesForSelfExtract=true
    if ($LASTEXITCODE -ne 0) { throw "Publish failed for $project" }
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