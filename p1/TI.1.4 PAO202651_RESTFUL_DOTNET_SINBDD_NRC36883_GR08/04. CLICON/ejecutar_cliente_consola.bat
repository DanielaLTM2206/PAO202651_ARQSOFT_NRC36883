@echo off
title Cliente Consola RESTful .NET 8 - Monsters Inc. GR08
echo ===================================================
echo   CLIENTE CONSOLA RESTful .NET 8 - MONSTERS INC GR08
echo ===================================================
echo.
set /p SERVER_IP="Ingrese la IP o Dominio del Servidor RESTful [Enter para localhost]: "
if "%SERVER_IP%"=="" set SERVER_IP=localhost

:: Quitar espacios y prefijos accidentales para normalizar
for /f "tokens=* delims= " %%a in ("%SERVER_IP%") do set SERVER_IP=%%a
set SERVER_IP=%SERVER_IP:https:// =%
set SERVER_IP=%SERVER_IP:http:// =%
set SERVER_IP=%SERVER_IP:https://=%
set SERVER_IP=%SERVER_IP:http://=%
set SERVER_IP=%SERVER_IP:/api=%
set SERVER_IP=%SERVER_IP:/=%

echo %SERVER_IP% | findstr /i "trycloudflare.com" >nul
if not errorlevel 1 (
    set SERVER_IP=https://%SERVER_IP%
)

echo.
echo Conectando a: %SERVER_IP% ...
echo.

if exist "%~dp0publish_consola\ConUni_Client_DotNet.exe" (
    "%~dp0publish_consola\ConUni_Client_DotNet.exe" "%SERVER_IP%"
) else (
    cd /d "%~dp0ConUni_Client_DotNet"
    dotnet run -- "%SERVER_IP%"
)

pause
