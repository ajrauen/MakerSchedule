@echo off
echo Creating migration for SQLite (Development)...
cd /d "%~dp0..\.."
set ASPNETCORE_ENVIRONMENT=Development
dotnet ef migrations add %1 --project MakerSchedule.Infrastructure --startup-project MakerSchedule.API
echo Migration created for SQLite!
pause 