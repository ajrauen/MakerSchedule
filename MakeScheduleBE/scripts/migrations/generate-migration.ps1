param(
    [Parameter(Mandatory=$true)][string]$MigrationName,
    [Parameter(Mandatory=$true)][int]$Env
)

# Usage info
if ($PSBoundParameters.Count -ne 2 -or ($Env -ne 0 -and $Env -ne 1)) {
    Write-Host "Usage: ./generate-migration.ps1 -MigrationName <Name> -Env 0^|1"
    exit 1
}

# Always go up two levels from the script's directory
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition
Set-Location (Join-Path $scriptDir ../..)
Write-Host "Now in: $(Get-Location)"

if ($Env -eq 0) {
    $env:ASPNETCORE_ENVIRONMENT = "Development"
    $migrationsDir = "Migrations-Sqlite"
    Write-Host "Generating SQLite (dev) migration: $MigrationName"
} else {
    $env:ASPNETCORE_ENVIRONMENT = "Production"
    $migrationsDir = "Migrations-SqlServer"
    Write-Host "Generating SQL Server (prod) migration: $MigrationName"
}

Write-Host "Will create migration in: $(Get-Location)/$migrationsDir"
if (-not (Test-Path $migrationsDir)) {
    New-Item -ItemType Directory -Path $migrationsDir | Out-Null
}

dotnet ef migrations add $MigrationName --output-dir $migrationsDir --project MakerSchedule.Infrastructure --startup-project MakerSchedule.API 

cd ./scripts/migrations