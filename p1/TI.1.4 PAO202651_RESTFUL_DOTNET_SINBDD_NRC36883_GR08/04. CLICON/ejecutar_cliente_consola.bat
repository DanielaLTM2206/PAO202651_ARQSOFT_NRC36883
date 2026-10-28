@echo off
title Cliente Consola RESTful .NET 8 - Monsters Inc. GR08
echo ===================================================
echo   CLIENTE CONSOLA RESTful .NET 8 - MONSTERS INC GR08
echo ===================================================
echo.
set /p SERVER_IP="Ingrese la IP del Servidor RESTful [Enter para localhost]: "
if "%SERVER_IP%"=="" set SERVER_IP=localhost
echo.
echo Conectando a: http://%SERVER_IP%:5000/api ...
echo.
cd /d "%~dp0ConUni_Client_DotNet"
dotnet run -- %SERVER_IP%
pause
