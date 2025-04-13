@echo off
echo Running database migrations...
cd ..\tools\BookTrail.Migrator
dotnet run
if %ERRORLEVEL% NEQ 0 (
    echo Error running migrations
    cd ..\..\build
    pause
    exit /b %ERRORLEVEL%
)
cd ..\..\build
echo Migrations completed successfully
pause