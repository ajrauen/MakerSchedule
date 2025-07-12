param(
    [Parameter(Mandatory=$true)][int]$Env
)

# Usage info
if ($PSBoundParameters.Count -ne 1 -or ($Env -ne 0 -and $Env -ne 1)) {
    Write-Host "Usage: ./update-database.ps1 -Env 0^|1" -ForegroundColor Yellow
    Write-Host "  0 = Local/Docker (Development)" -ForegroundColor Yellow
    Write-Host "  1 = Cloud (Production)" -ForegroundColor Yellow
    exit 1
}

# Get the script directory and navigate to the project root
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition
Set-Location (Join-Path $scriptDir ../..)
Write-Host "Now in: $(Get-Location)"

if ($Env -eq 0) {
    $env:ASPNETCORE_ENVIRONMENT = "Development"
    Write-Host "Updating Local/Docker database (Development)..." -ForegroundColor Green
} else {
    $env:ASPNETCORE_ENVIRONMENT = "Production"
    Write-Host "Updating Cloud database (Production)..." -ForegroundColor Green
}

# Run the database update
dotnet ef database update --project MakerSchedule.Infrastructure --startup-project MakerSchedule.API

Write-Host "Database updated!" -ForegroundColor Green 
cd ./scripts/migrations