@echo off
title Cliente Escritorio SOAP Java - Monsters Inc. GR08
echo ===================================================
echo   CLIENTE ESCRITORIO SOAP JAVA (SWING) - GR08
echo ===================================================
echo.
set /p SERVER_IP="Ingrese la IP del Servidor SOAP [Enter para localhost]: "
if "%SERVER_IP%"=="" set SERVER_IP=localhost
echo.
echo Conectando con el servidor en: %SERVER_IP%:8080 ...
echo.
java "-Dsoap.host=%SERVER_IP%" -cp "%~dp0ClienteEscritorio.jar;%~dp0lib\*" ec.edu.monster.clienteescritorio.ClienteEscritorio
pause
