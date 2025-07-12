@echo off
setlocal

REM Usage: generate-migration.bat MigrationName 0|1
if "%~1"=="" goto usage
if "%~2"=="" goto usage
if "%~2" NEQ "0" if "%~2" NEQ "1" goto usage

set MIGRATION_NAME=%~1
set ENV=%~2

REM Always go to the solution root (MakerScheduleBE)
cd /d "%~dp0\..\.."

REM Confirm we are in the right place
if not exist MakerSchedule.Infrastructure (
    echo ERROR: Not in the solution root! Aborting.
    exit /b 1
)

if %ENV%==0 (
    set ASPNETCORE_ENVIRONMENT=Development
    set MIGRATIONS_DIR=MakerSchedule.Infrastructure\Migrations.Sqlite
    echo Generating SQLite (dev) migration: %MIGRATION_NAME%
) else (
    set ASPNETCORE_ENVIRONMENT=Production
    set MIGRATIONS_DIR=MakerSchedule.Infrastructure\Migrations.SqlServer
    echo Generating SQL Server (prod) migration: %MIGRATION_NAME%
)

if not exist "%MIGRATIONS_DIR%" mkdir "%MIGRATIONS_DIR%"
dotnet ef migrations add %MIGRATION_NAME% --output-dir "%MIGRATIONS_DIR%" --project MakerSchedule.Infrastructure --startup-project MakerSchedule.API
exit /b 0

:usage
echo Usage: %0 MigrationName 0^|1
exit /b 1