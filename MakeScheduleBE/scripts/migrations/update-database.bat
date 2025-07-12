@echo off
setlocal

REM Usage: update-database.bat 0|1
if "%~1"=="" goto usage
if "%~1" NEQ "0" if "%~1" NEQ "1" goto usage

set ENV=%~1

if %ENV%==0 (
    set ASPNETCORE_ENVIRONMENT=Development
    set MIGRATIONS_DIR=..\..\MakerSchedule.Infrastructure\Migrations.Sqlite
    echo Updating SQLite (dev) database...
) else (
    set ASPNETCORE_ENVIRONMENT=Production
    set MIGRATIONS_DIR=..\..\MakerSchedule.Infrastructure\Migrations.SqlServer
    echo Updating SQL Server (prod) database...
)

cd /d "%~dp0\..\.."
dotnet ef database update --project MakerSchedule.Infrastructure --startup-project MakerSchedule.API --migrations %MIGRATIONS_DIR%
echo Database updated!
pause
exit /b 0

:usage
echo Usage: %0 0^|1
exit /b 1 