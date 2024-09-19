@echo off
setlocal

where git >nul 2>nul
if errorlevel 1 (
    echo Git is not installed. Using default commit hash.
    set COMMIT_HASH="NOT SET"
) else (
    for /f "tokens=1" %%i in ('git rev-parse --short HEAD') do set COMMIT_HASH=%%i
)

echo using System; > BuildInfo.cs
echo namespace SinmaiAssist { >> BuildInfo.cs
echo     public static partial class BuildInfo { >> BuildInfo.cs
echo         public const string CommitHash = "%COMMIT_HASH%"; >> BuildInfo.cs
echo     } >> BuildInfo.cs
echo } >> BuildInfo.cs

endlocal