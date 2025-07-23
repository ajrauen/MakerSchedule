
# Get the script's directory
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition
$startingDir = Get-Location

try {
    # Set location to the docker folder using an absolute path
    Set-Location (Join-Path $scriptDir "..\\docker")
    docker compose down --volumes --remove-orphans
    if ($LASTEXITCODE -ne 0) { throw "docker compose down failed" }

    # Start Docker Compose in detached mode
    docker compose up -d
    if ($LASTEXITCODE -ne 0) { throw "docker compose up failed" }

    # Delete all files in Infrastructure/Migrations (use absolute path)
    $infraMigrations = Join-Path $scriptDir "..\\MakerSchedule.Infrastructure\\Migrations\\*"
    Remove-Item -Path $infraMigrations -Force -ErrorAction SilentlyContinue
    if ($LASTEXITCODE -ne 0) { throw "Failed to delete migration files" }

    # Run the generate-migration script with 'initial' parameter (use absolute path)
    $generateMigration = Join-Path $scriptDir "..\\scripts\\migrations\\generate-migration.ps1"
    & $generateMigration initial
    if ($LASTEXITCODE -ne 0) { throw "generate-migration script failed" }

    # Run the update-database script with '0' parameter (use absolute path)
    $updateDatabase = Join-Path $scriptDir "..\\scripts\\migrations\\update-database.ps1"
    & $updateDatabase 0
    if ($LASTEXITCODE -ne 0) { throw "update-database script failed" }
}
finally {
    Set-Location $startingDir
}