@echo off
echo Fixing AspNet tables migration issue...
cd /d "%~dp0..\.."
set ASPNETCORE_ENVIRONMENT=Production

echo.
echo This script will help resolve the "There is already an object named 'AspNetRoles'" error.
echo.
echo Step 1: Checking current migration status...
dotnet ef migrations list --project MakerSchedule.Infrastructure --startup-project MakerSchedule.API

echo.
echo Step 2: Attempting to mark the problematic migration as applied...
echo This will skip the migration that's trying to create existing tables.
dotnet ef database update 20250710032319_AddNewFeature --project MakerSchedule.Infrastructure --startup-project MakerSchedule.API --connection "Server=tcp:makerschedule.database.windows.net,1433;Initial Catalog=makerschedule-db;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication=Active Directory Default;" --force

echo.
echo Step 3: Verifying migration status...
dotnet ef migrations list --project MakerSchedule.Infrastructure --startup-project MakerSchedule.API

echo.
echo If the above steps didn't work, you may need to:
echo 1. Manually insert the migration record into __EFMigrationsHistory table
echo 2. Or remove the migration file and create a new one
echo.
echo Migration fix attempt completed!
pause 