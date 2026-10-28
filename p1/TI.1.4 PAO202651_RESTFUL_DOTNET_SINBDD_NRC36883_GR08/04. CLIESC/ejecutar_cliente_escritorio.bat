@echo off
title Cliente Escritorio RESTful .NET 8 - Monsters Inc. GR08
echo ===================================================
echo   CLIENTE ESCRITORIO RESTful .NET 8 (WPF) - GR08
echo ===================================================
echo.
set /p SERVER_IP="Ingrese la IP del Servidor RESTful [Enter para localhost]: "
if "%SERVER_IP%"=="" set SERVER_IP=localhost
echo.
echo Conectando a: http://%SERVER_IP%:5000/api ...
echo.
cd /d "%~dp0ConUni_CliEsc_GR08"
dotnet run -- %SERVER_IP%
