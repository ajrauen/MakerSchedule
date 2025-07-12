param(
    [Parameter(Mandatory=$true)][string]$MigrationName
)

# Usage info
if ($PSBoundParameters.Count -ne 1) {
    Write-Host "Usage: ./generate-migration.ps1 -MigrationName <Name>"
    exit 1
}

# Always go up two levels from the script's directory
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition
Set-Location (Join-Path $scriptDir ../..)
Write-Host "Now in: $(Get-Location)"

# Use Development environment for migration generation (doesn't matter for SQL Server)
$env:ASPNETCORE_ENVIRONMENT = "Development"
Write-Host "Generating SQL Server migration: $MigrationName"

dotnet ef migrations add $MigrationName --project MakerSchedule.Infrastructure --startup-project MakerSchedule.API 

cd ./scripts/migrations