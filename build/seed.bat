@echo off
echo Running database seeder...
cd ..\tools\BookTrail.Seeder
dotnet run
if %ERRORLEVEL% NEQ 0 (
    echo Error running seeder
    cd ..\..\build
    pause
    exit /b %ERRORLEVEL%
)
cd ..\..\build
echo Seeding completed successfully
pause