@echo off
title Cliente Consola SOAP Java - Monsters Inc. GR08
echo ===================================================
echo   CLIENTE CONSOLA SOAP JAVA - GR08
echo ===================================================
echo.
set /p SERVER_IP="Ingrese la IP del Servidor SOAP [Enter para localhost]: "
if "%SERVER_IP%"=="" set SERVER_IP=localhost
echo.
echo Conectando con el servidor en: %SERVER_IP%:8080 ...
echo.
java -Dsoap.host=%SERVER_IP% -cp "%~dp0ClienteConsola\target\classes;%~dp0ClienteConsola\target\ClienteConsola-1.0-SNAPSHOT.jar;%~dp0lib\*" ec.edu.monster.clienteconsola.ClienteConsola
if %errorlevel% neq 0 (
    python "%~dp0ejecutar.py" %SERVER_IP%
)
pause
