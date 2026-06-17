@echo off
setlocal enabledelayedexpansion

for /f "tokens=*" %%L in ('git lfs locks') do (
set "line=%%L"

rem Find lines containing "ID:"
echo !line! | findstr "ID:" >nul
if not errorlevel 1 (
    for /f "tokens=2 delims=:" %%I in ("!line!") do (
        set "id=%%I"
        set "id=!id: =!"
        echo Unlocking ID !id!
        git lfs unlock --id !id!
    )
)

)

echo Done.
pause
