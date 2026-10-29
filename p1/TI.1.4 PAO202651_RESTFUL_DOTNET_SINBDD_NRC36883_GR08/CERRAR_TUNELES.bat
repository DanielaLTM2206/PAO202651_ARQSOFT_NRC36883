@echo off
title Cerrar Todos los Tuneles Cloudflare - GR08
echo ======================================================
echo           CERRANDO TUNELES CLOUDFLARE
echo ======================================================
echo.
taskkill /F /IM cloudflared.exe >nul 2>&1
if %errorlevel% equ 0 (
    echo [OK] Todos los tuneles activos han sido cerrados correctamente.
) else (
    echo [INFO] No habia ningun tunel abierto en ejecucion.
)
echo.
echo ======================================================
pause
