@echo off
rem ============================================================================
rem  ConUni GR08 - Servidor RESTful Java (Payara, puerto 8080)
rem  Prepara esta computadora para que los clientes de OTRAS computadoras
rem  (consola, escritorio, web o movil) se conecten a su servidor Payara:
rem    1. Abre el puerto 8080 en el firewall de Windows (solo la primera vez).
rem    2. Avisa si el firewall tiene bloqueado Java (eso impide las conexiones).
rem    3. Muestra las IP de esta computadora para escribirlas en los clientes.
rem  Payara se sigue iniciando desde NetBeans (Run del proyecto WS_ConUni_Java_RESTFUL).
rem ============================================================================
chcp 65001 >nul
setlocal
title ConUni GR08 - Servidor RESTful Java (puerto 8080)

rem Abrir el puerto en el firewall necesita permisos de administrador
net session >nul 2>&1
if %errorlevel% neq 0 (
    echo Solicitando permisos de administrador...
    powershell -NoProfile -Command "Start-Process -FilePath '%~f0' -Verb RunAs"
    exit /b
)

set "REGLA=ConUni GR08 REST 8080"
netsh advfirewall firewall show rule name="%REGLA%" >nul 2>&1
if %errorlevel% neq 0 (
    netsh advfirewall firewall add rule name="%REGLA%" dir=in action=allow protocol=TCP localport=8080 profile=any >nul
    echo [OK] Se abrió el puerto 8080 en el firewall de Windows.
) else (
    echo [OK] El puerto 8080 ya estaba abierto en el firewall de Windows.
)

rem Una regla que BLOQUEA java.exe gana sobre cualquier regla que permite el puerto
powershell -NoProfile -Command "$bloqueos = Get-NetFirewallRule -Direction Inbound -Action Block -Enabled True -ErrorAction SilentlyContinue | Where-Object { ($_ | Get-NetFirewallApplicationFilter).Program -like '*java*' }; if ($bloqueos) { Write-Host ''; Write-Host '[AVISO] El firewall tiene reglas que BLOQUEAN Java. Payara no recibira conexiones de otras computadoras:' -ForegroundColor Yellow; $bloqueos | ForEach-Object { Write-Host ('    ' + $_.DisplayName) -ForegroundColor Yellow }; Write-Host '    Desactivelas en: Firewall de Windows Defender > Configuracion avanzada > Reglas de entrada.' -ForegroundColor Yellow } else { Write-Host '[OK] El firewall no bloquea Java.' }"

echo.
echo IP de esta computadora. En los clientes de la otra computadora use "Configurar servidor"
echo (o "Cambiar servidor" en el móvil) y escriba una de estas IP:
powershell -NoProfile -Command "Get-NetIPConfiguration | Where-Object { $_.IPv4DefaultGateway -and $_.NetAdapter.Status -eq 'Up' } | ForEach-Object { '    ' + $_.IPv4Address.IPAddress + '    (' + $_.InterfaceAlias + ')' }"
echo.
echo Para comprobar la conexion, inicie Payara desde NetBeans y en la otra computadora abra en el
echo navegador esta direccion (copiela tal cual). Debe mostrar {"Mensaje":"..."} pidiendo el token:
powershell -NoProfile -Command "Get-NetIPConfiguration | Where-Object { $_.IPv4DefaultGateway -and $_.NetAdapter.Status -eq 'Up' } | ForEach-Object { '    http://' + $_.IPv4Address.IPAddress + ':8080/WS_ConUni_Java_RESTFUL/api/unidades' }"
echo.
echo Ambas computadoras deben estar en la misma red (de preferencia el hotspot de un celular).
echo.
pause
