@echo off
title Servidor SOAP Java Payara - Monsters Inc. GR08
echo ===================================================
echo   INICIANDO SERVIDOR SOAP JAVA (Payara Server 7)
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
        echo Por favor verifica que Payara este instalado en C:\payara-7.2026.9 o en tu PATH.
        echo.
        pause
        exit /b 1
    )
)

echo Localizado Payara: %ASADMIN%
echo.
echo Verificando estado del dominio [domain1]...
call %ASADMIN% list-domains > "%TEMP%\payara_domains.txt"
findstr /i "domain1.*running" "%TEMP%\payara_domains.txt" >nul
if not errorlevel 1 goto ya_corriendo

echo Iniciando domain1 de Payara Server...
call %ASADMIN% start-domain domain1
goto mostrar_info

:ya_corriendo
echo [OK] El servidor Payara [domain1] ya se encuentra ENCENDIDO y en ejecucion.

:mostrar_info
echo.
echo ===================================================
echo   SERVIDOR SOAP JAVA LISTO Y ACTIVO
echo   Local WSDL:    http://localhost:8080/WS_ConUni_Java_SOAP/WSConversorUnidades?wsdl
echo   Cliente Web:   http://localhost:8080/ClienteWeb/
echo   Admin Console: http://localhost:4848
echo ===================================================
echo.
pause
