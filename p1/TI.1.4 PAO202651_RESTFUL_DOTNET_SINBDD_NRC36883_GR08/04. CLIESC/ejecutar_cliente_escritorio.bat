@echo off
title Cliente Escritorio RESTful .NET 8 - Monsters Inc. GR08
echo ===================================================
echo   CLIENTE ESCRITORIO RESTful .NET 8 (WPF) - GR08
echo ===================================================
echo.
set /p SERVER_IP="Ingrese la IP o Dominio del Servidor RESTful [Enter para localhost]: "
if "%SERVER_IP%"=="" set SERVER_IP=localhost

echo %SERVER_IP% | findstr /i "trycloudflare.com" >nul
if not errorlevel 1 (
    echo %SERVER_IP% | findstr /i "^http" >nul
    if errorlevel 1 set SERVER_IP=https://%SERVER_IP%
)

echo.
echo Conectando a: %SERVER_IP% ...
echo.
cd /d "%~dp0ConUni_CliEsc_GR08"
dotnet run -- %SERVER_IP%
pause
