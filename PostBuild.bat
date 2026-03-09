@echo off

where git >nul 2>nul
if errorlevel 1 (
    echo Git is not installed. Using default commit hash.
    set COMMIT_HASH="NOT SET"
) else (
    for /f "tokens=1" %%i in ('git rev-parse --short HEAD') do set COMMIT_HASH=%%i
)

for /f "usebackq delims=" %%t in (`powershell -Command "( Get-Date -Format 'yyyyMMddHHmmss')"`) do (
    set TIMESTAMP=%%t
)

echo CommitHash="%COMMIT_HASH%"
echo BuildDate="%TIMESTAMP%"