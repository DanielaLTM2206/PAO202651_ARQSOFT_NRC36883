@echo off
rem ============================================================================
rem  ConUni GR08 - Servidor SOAP .NET (WS_ConUni_Dotnet_SOAP, puerto 5080)
rem  Prepara esta computadora para que los clientes de OTRAS computadoras
rem  (consola, escritorio, web o movil) se conecten a su servidor:
rem    1. Abre el puerto 5080 en el firewall de Windows (solo la primera vez).
rem    2. Muestra las IP de esta computadora para escribirlas en los clientes.
rem    3. Arranca el servidor SOAP.
rem ============================================================================
chcp 65001 >nul
setlocal
title ConUni GR08 - Servidor SOAP .NET (puerto 5080)

rem Abrir el puerto en el firewall necesita permisos de administrador
net session >nul 2>&1
if %errorlevel% neq 0 (
    echo Solicitando permisos de administrador...
    powershell -NoProfile -Command "Start-Process -FilePath '%~f0' -Verb RunAs"
    exit /b
)

set "REGLA=ConUni GR08 SOAP 5080"
netsh advfirewall firewall show rule name="%REGLA%" >nul 2>&1
if %errorlevel% neq 0 (
    netsh advfirewall firewall add rule name="%REGLA%" dir=in action=allow protocol=TCP localport=5080 profile=any >nul
    echo [OK] Se abrió el puerto 5080 en el firewall de Windows.
) else (
    echo [OK] El puerto 5080 ya estaba abierto en el firewall de Windows.
)

echo.
echo IP de esta computadora. En los clientes de la otra computadora use "Configurar servidor"
echo (o "Cambiar servidor" en el móvil) y escriba una de estas IP:
powershell -NoProfile -Command "Get-NetIPConfiguration | Where-Object { $_.IPv4DefaultGateway -and $_.NetAdapter.Status -eq 'Up' } | ForEach-Object { '    ' + $_.IPv4Address.IPAddress + '    (' + $_.InterfaceAlias + ')' }"
echo.
echo Para comprobar la conexion, en la otra computadora abra en el navegador esta direccion (copiela tal cual):
powershell -NoProfile -Command "Get-NetIPConfiguration | Where-Object { $_.IPv4DefaultGateway -and $_.NetAdapter.Status -eq 'Up' } | ForEach-Object { '    http://' + $_.IPv4Address.IPAddress + ':5080/WSConversorUnidades.svc?wsdl' }"
echo.
echo Ambas computadoras deben estar en la misma red (de preferencia el hotspot de un celular).
echo.

cd /d "%~dp0WS_ConUni_Dotnet_SOAP"
dotnet run --launch-profile http
pause
