@echo off
title Cliente Escritorio SOAP Java - Monsters Inc. GR08
echo ===================================================
echo   CLIENTE ESCRITORIO SOAP JAVA (SWING) - GR08
echo ===================================================
echo.
set /p SERVER_IP="Ingrese la IP o Dominio del Servidor SOAP [Enter para localhost]: "
if "%SERVER_IP%"=="" set SERVER_IP=localhost

set SERVER_PORT=8080
echo %SERVER_IP% | findstr /i "trycloudflare.com" >nul
if not errorlevel 1 set SERVER_PORT=80

for /f "tokens=1,2 delims=:" %%a in ("%SERVER_IP%") do (
    if not "%%b"=="" (
        set SERVER_IP=%%a
        set SERVER_PORT=%%b
    )
)

echo.
echo Conectando con el servidor en: %SERVER_IP%:%SERVER_PORT% ...
echo.
java "-Dsoap.host=%SERVER_IP%" "-Dsoap.port=%SERVER_PORT%" -cp "%~dp0ClienteEscritorio.jar;%~dp0lib\*" ec.edu.monster.clienteescritorio.ClienteEscritorio
pause
