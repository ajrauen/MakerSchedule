@echo off
echo Creating migration for SQL Server (Production)...
cd /d "%~dp0..\.."
set ASPNETCORE_ENVIRONMENT=Production
dotnet ef migrations add %1 --project MakerSchedule.Infrastructure --startup-project MakerSchedule.API
echo Migration created for SQL Server!
pause 