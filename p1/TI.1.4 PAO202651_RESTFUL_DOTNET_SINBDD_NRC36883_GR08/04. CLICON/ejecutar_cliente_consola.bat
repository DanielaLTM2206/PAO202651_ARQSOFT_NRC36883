@echo off
title Cliente Consola RESTful .NET 8 - Monsters Inc. GR08
echo ===================================================
echo   CLIENTE CONSOLA RESTful .NET 8 - MONSTERS INC GR08
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
cd /d "%~dp0ConUni_Client_DotNet"
dotnet run -- %SERVER_IP%
pause
