@echo off
title Detener Servidor SOAP Java Payara - Monsters Inc. GR08
echo ===================================================
echo   DETENIENDO SERVIDOR SOAP JAVA (Payara Server 7)
echo ===================================================
echo.

set ASADMIN=""
if exist "C:\payara-7.2026.9\payara7\bin\asadmin.bat" set ASADMIN="C:\payara-7.2026.9\payara7\bin\asadmin.bat"
if exist "C:\payara7\bin\asadmin.bat" set ASADMIN="C:\payara7\bin\asadmin.bat"
if exist "C:\payara\bin\asadmin.bat" set ASADMIN="C:\payara\bin\asadmin.bat"
if %ASADMIN%=="" (
    for /d %%d in (C:\payara* C:\Users\%USERNAME%\Downloads\payara*) do (
        if exist "%%d\payara7\bin\asadmin.bat" set ASADMIN="%%d\payara7\bin\asadmin.bat"
        if exist "%%d\bin\asadmin.bat" set ASADMIN="%%d\bin\asadmin.bat"
    )
)

if %ASADMIN%=="" (
    where asadmin.bat >nul 2>&1
    if not errorlevel 1 (
        set ASADMIN=asadmin.bat
    ) else (
        echo [ERROR] No se encontro asadmin.bat de Payara Server.
        echo.
        pause
        exit /b 1
    )
)

echo Deteniendo domain1 de Payara Server...
call %ASADMIN% stop-domain domain1

echo.
echo ===================================================
echo   [OK] Servidor Payara detenido exitosamente.
echo ===================================================
echo.
pause
