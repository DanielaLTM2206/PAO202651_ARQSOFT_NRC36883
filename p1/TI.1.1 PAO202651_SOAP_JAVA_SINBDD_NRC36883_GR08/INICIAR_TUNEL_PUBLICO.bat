@echo off
title Tunel Publico Cloudflare - Servidor SOAP 8080
echo ===================================================
echo   INICIANDO TUNEL PUBLICO PARA PRESENTACION
echo ===================================================
echo.
echo Conectando servidor local (puerto 8080) a Internet...
echo.
"%~dp0cloudflared.exe" tunnel --url http://127.0.0.1:8080
pause
