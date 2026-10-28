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
java -Dsoap.host=%SERVER_IP% -cp "%~dp0ClienteEscritorio\target\classes;%~dp0ClienteEscritorio\target\ClienteEscritorio-1.0-SNAPSHOT.jar;%~dp0lib\*" ec.edu.monster.clienteescritorio.ClienteEscritorio
if %errorlevel% neq 0 (
    python "%~dp0ejecutar.py" %SERVER_IP%
)
pause
