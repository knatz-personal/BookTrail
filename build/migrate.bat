@echo off
setlocal

echo Running database migrations...
pushd ..\tools\BookTrail.Migrator
dotnet run
if %ERRORLEVEL% NEQ 0 (
    echo Error running migrations
    popd
    pause
    exit /b %ERRORLEVEL%
)
popd
echo Migrations completed successfully
pause

endlocal