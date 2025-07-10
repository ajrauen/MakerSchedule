@echo off
echo Updating SQL Server database (Production)...
cd /d "%~dp0..\.."
set ASPNETCORE_ENVIRONMENT=Production
dotnet ef database update --project MakerSchedule.Infrastructure --startup-project MakerSchedule.API
echo SQL Server database updated!
pause 