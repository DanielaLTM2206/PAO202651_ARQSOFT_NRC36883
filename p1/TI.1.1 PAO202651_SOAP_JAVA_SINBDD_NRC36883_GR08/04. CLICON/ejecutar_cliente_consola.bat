@echo off
title Cliente Consola SOAP Java - Monsters Inc. GR08
echo ===================================================
echo   CLIENTE CONSOLA SOAP JAVA - GR08
echo ===================================================
echo.
set /p SERVER_IP="Ingrese la IP o Dominio del Servidor SOAP [Enter para localhost]: "
if "%SERVER_IP%"=="" set SERVER_IP=localhost

set SERVER_PORT=8080

:: Limpiar prefijos http:// o https:// y barras finales si el usuario los pega completos
set SERVER_IP=%SERVER_IP:https://=%
set SERVER_IP=%SERVER_IP:http://=%
set SERVER_IP=%SERVER_IP:/=%

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
java "-Dsoap.host=%SERVER_IP%" "-Dsoap.port=%SERVER_PORT%" -cp "%~dp0ClienteConsola.jar;%~dp0lib\*" ec.edu.monster.clienteconsola.ClienteConsola
pause
