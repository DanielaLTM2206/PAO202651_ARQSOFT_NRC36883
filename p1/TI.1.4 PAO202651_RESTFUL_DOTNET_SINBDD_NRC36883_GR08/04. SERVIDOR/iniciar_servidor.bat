@echo off
title Servidor RESTful .NET 8 - Monsters Inc. GR08
echo ===================================================
echo   INICIANDO SERVIDOR RESTful .NET 8 (Kestrel)
echo   Local (PC):    http://localhost:5000
echo   Red / Celular: http://192.168.100.15:5000
echo   Swagger:       http://localhost:5000/swagger
echo   Web App:       http://localhost:5000/app/index.html
echo ===================================================
echo Para detener el servidor, presiona Ctrl + C o cierra esta ventana.
echo.
cd /d "%~dp0WS_ConUni_DotNet_REST"
dotnet run --urls "http://0.0.0.0:5000"
pause
