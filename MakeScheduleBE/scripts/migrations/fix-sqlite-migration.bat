@echo off
echo Fixing SQLite migration history...
cd /d "%~dp0..\.."
set ASPNETCORE_ENVIRONMENT=Development

echo.
echo This script will manually insert the migration record into SQLite database.
echo.
echo Step 1: Creating the __EFMigrationsHistory table if it doesn't exist...
sqlite3 "MakerSchedule.Infrastructure\makerschedule.db" "CREATE TABLE IF NOT EXISTS __EFMigrationsHistory (MigrationId TEXT PRIMARY KEY, ProductVersion TEXT);"

echo.
echo Step 2: Inserting the migration record...
sqlite3 "MakerSchedule.Infrastructure\makerschedule.db" "INSERT OR IGNORE INTO __EFMigrationsHistory (MigrationId, ProductVersion) VALUES ('20250710033242_AddNewFeature', '8.0.0');"

echo.
echo Step 3: Verifying the migration status...
dotnet ef migrations list --project MakerSchedule.Infrastructure --startup-project MakerSchedule.API

echo.
echo Migration history fixed for SQLite!
pause 