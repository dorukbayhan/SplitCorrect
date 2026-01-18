@echo off
echo Starting SplitCorrect Backend...
cd /d "%~dp0src\SplitCorrect.Api"
start "SplitCorrect API" dotnet run

timeout /t 5 /nobreak >nul

echo Starting SplitCorrect Frontend...
cd /d "%~dp0frontend"
start "SplitCorrect Frontend" npm run dev

echo.
echo ========================================
echo    SplitCorrect is starting...
echo ========================================
echo.
echo Frontend:  http://localhost:3000
echo Backend:   http://localhost:5016
echo Swagger:   http://localhost:5016/swagger
echo.
echo Press any key to stop all services...
pause >nul

taskkill /FI "WINDOWTITLE eq SplitCorrect*" /F
