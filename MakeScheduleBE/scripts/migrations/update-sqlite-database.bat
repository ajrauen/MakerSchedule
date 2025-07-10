@echo off
echo Updating SQLite database (Development)...
cd /d "%~dp0..\.."
set ASPNETCORE_ENVIRONMENT=Development
dotnet ef database update --project MakerSchedule.Infrastructure --startup-project MakerSchedule.API
echo SQLite database updated!
pause 