@echo off
title Cliente Consola RESTful .NET 8 - Monsters Inc. GR08
echo ===================================================
echo   INICIANDO CLIENTE CONSOLA RESTful .NET 8
echo   Conectando a: http://localhost:5000/api
echo ===================================================
echo.
echo Recuerda que el SERVIDOR RESTful debe estar prendido antes.
echo.
cd /d "%~dp0ConUni_Client_DotNet"
dotnet run
pause
