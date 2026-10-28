@echo off
title Tunel Publico Cloudflare - Servidor RESTful .NET 5000
echo ===================================================
echo   INICIANDO TUNEL PUBLICO PARA RESTful .NET 8
echo ===================================================
echo.
echo Conectando servidor Kestrel (puerto 5000) a Internet...
echo.
"%~dp0cloudflared.exe" tunnel --url http://127.0.0.1:5000
pause
