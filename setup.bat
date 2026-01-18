@echo off
setlocal enabledelayedexpansion

echo.
echo ========================================
echo    SplitCorrect - Quick Setup Script
echo ========================================
echo.

REM Check prerequisites
echo [1/4] Checking prerequisites...
echo.

where docker >nul 2>nul
if %errorlevel% neq 0 (
    echo [X] Docker is not installed. Please install Docker Desktop.
    echo     Download from: https://www.docker.com/products/docker-desktop
    pause
    exit /b 1
)

where dotnet >nul 2>nul
if %errorlevel% neq 0 (
    echo [X] .NET 8 SDK is not installed.
    echo     Download from: https://dotnet.microsoft.com/download/dotnet/8.0
    pause
    exit /b 1
)

where node >nul 2>nul
if %errorlevel% neq 0 (
    echo [X] Node.js is not installed.
    echo     Download from: https://nodejs.org/
    pause
    exit /b 1
)

echo [OK] All prerequisites are installed!
echo.

REM Start Docker services
echo [2/4] Starting Docker services...
docker compose up -d

echo.
echo [*] Waiting for PostgreSQL to be ready...
timeout /t 10 /nobreak >nul

REM Setup Backend
echo.
echo [3/4] Setting up Backend...
cd src\SplitCorrect.Infrastructure
call dotnet restore
call dotnet ef database update --startup-project ..\SplitCorrect.Api

if %errorlevel% neq 0 (
    echo [X] Database migration failed!
    cd ..\..
    pause
    exit /b 1
)

cd ..\..

REM Setup Frontend
echo.
echo [4/4] Setting up Frontend...
cd frontend

if not exist .env (
    echo [*] Creating .env file...
    copy .env.example .env
)

call npm install

if %errorlevel% neq 0 (
    echo [X] npm install failed!
    cd ..
    pause
    exit /b 1
)

cd ..

echo.
echo ========================================
echo    Setup Complete!
echo ========================================
echo.
echo To start the application:
echo.
echo Terminal 1 (Backend):
echo   cd src\SplitCorrect.Api
echo   dotnet run
echo.
echo Terminal 2 (Frontend):
echo   cd frontend
echo   npm run dev
echo.
echo Access points:
echo   Frontend:  http://localhost:3000
echo   Backend:   http://localhost:5016
echo   Swagger:   http://localhost:5016/swagger
echo   pgAdmin:   http://localhost:5050
echo.
pause
