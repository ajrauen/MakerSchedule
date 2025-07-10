@echo off
echo Manual migration fix for AspNet tables issue...
cd /d "%~dp0..\.."
set ASPNETCORE_ENVIRONMENT=Production

echo.
echo This script provides SQL commands to manually fix the migration history.
echo.
echo The issue is that the migration '20250710032319_AddNewFeature' is trying to create
echo tables that already exist in your database.
echo.
echo To fix this, you need to manually insert the migration record into the database.
echo.
echo Step 1: Connect to your SQL Server database using SQL Server Management Studio
echo or Azure Data Studio with this connection string:
echo Server=tcp:makerschedule.database.windows.net,1433;Initial Catalog=makerschedule-db;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication=Active Directory Default;
echo.
echo Step 2: Run this SQL command in your database:
echo.
echo INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
echo VALUES ('20250710032319_AddNewFeature', '8.0.0');
echo.
echo Step 3: After running the SQL command, test the migration again:
echo.
echo dotnet ef database update --project MakerSchedule.Infrastructure --startup-project MakerSchedule.API
echo.
echo This should resolve the "There is already an object named 'AspNetRoles'" error.
echo.
pause 