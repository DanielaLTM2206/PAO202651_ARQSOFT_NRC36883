@echo off
title Detener Servidor RESTful .NET 8
echo ===================================================
echo   DETENIENDO SERVIDOR RESTful .NET 8
echo ===================================================
echo Deteniendo procesos WS_ConUni_DotNet_REST.exe...
taskkill /IM WS_ConUni_DotNet_REST.exe /F 2>nul
echo Liberando puerto 5000...
for /f "tokens=5" %%a in ('netstat -aon ^| findstr ":5000" ^| findstr "LISTENING"') do (
    taskkill /F /PID %%a 2>nul
)
echo.
echo Servidor detenido y puerto 5000 liberado.
echo.
pause
