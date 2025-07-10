@echo off
echo Fixing migration history for Azure SQL database...
cd /d "%~dp0..\.."
set ASPNETCORE_ENVIRONMENT=Production

echo.
echo Step 1: Creating __EFMigrationsHistory table if it doesn't exist...
dotnet ef database update 0 --project MakerSchedule.Infrastructure --startup-project MakerSchedule.API --connection "Server=tcp:makerschedule.database.windows.net,1433;Initial Catalog=makerschedule-db;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication=Active Directory Default;"

echo.
echo Step 2: Marking existing migrations as applied...
dotnet ef database update --project MakerSchedule.Infrastructure --startup-project MakerSchedule.API --connection "Server=tcp:makerschedule.database.windows.net,1433;Initial Catalog=makerschedule-db;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication=Active Directory Default;"

echo.
echo Migration history fixed!
echo.
echo If you still get errors about existing tables, you may need to:
echo 1. Remove the problematic migration file
echo 2. Create a new migration that only includes new changes
echo 3. Or manually mark the migration as applied in the database
echo.
pause 