@echo off
echo ========================================
echo Resetting Database and Migrations
echo ========================================

echo.
echo Step 1: Deleting current database file...
if exist "..\MakerSchedule.Infrastructure\makerschedule.db" (
    del "..\MakerSchedule.Infrastructure\makerschedule.db"
    echo Database file deleted.
) else (
    echo Database file not found, continuing...
)

echo.
echo Step 2: Deleting existing migrations...
if exist "..\MakerSchedule.Infrastructure\Migrations" (
    rmdir /s /q "..\MakerSchedule.Infrastructure\Migrations"
    echo Migrations folder deleted.
) else (
    echo Migrations folder not found, continuing...
)

echo.
echo Step 3: Generating new initial migration...
cd ..
dotnet ef migrations add Initial --project MakerSchedule.Infrastructure --startup-project MakerSchedule.API
if %errorlevel% neq 0 (
    echo ERROR: Failed to generate migration
    pause
    exit /b 1
)
echo Migration generated successfully.

echo.
echo Step 4: Updating database...
dotnet ef database update --project MakerSchedule.Infrastructure --startup-project MakerSchedule.API
if %errorlevel% neq 0 (
    echo ERROR: Failed to update database
    pause
    exit /b 1
)
echo Database updated successfully.

echo.
echo ========================================
echo Database reset completed successfully!
echo ========================================
echo.
echo The database has been reset with:
echo - Fresh database file
echo - New initial migration
echo - All tables created
echo - Seed data will be applied on next app startup
echo.
pause 