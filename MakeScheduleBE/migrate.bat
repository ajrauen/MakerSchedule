@echo off
REM Create a new EF Core migration and update the database for MakerSchedule

set /p MIGRATION_NAME=Enter migration name: 

dotnet ef migrations add %MIGRATION_NAME% --project MakerSchedule.Infrastructure --startup-project MakerSchedule.API

dotnet ef database update --project MakerSchedule.Infrastructure --startup-project MakerSchedule.API

pause 