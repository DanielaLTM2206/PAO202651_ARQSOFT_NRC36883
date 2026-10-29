@echo off
:: Solicitar permisos de Administrador automáticamente si no los tiene
net session >nul 2>&1
if %errorLevel% neq 0 (
    echo Solicitando permisos de Administrador...
    powershell -Command "Start-Process '%~0' -Verb RunAs"
    exit /b
)

title Desbloquear Firewall - Monsters Inc. RESTful .NET 8
echo ====================================================================
echo      DESBLOQUEANDO FIREWALL DE WINDOWS PARA EL SERVIDOR REST
echo ====================================================================
echo.
echo 1. Cambiando reglas de bloqueo a PERMITIR (Allow)...
powershell -Command "Get-NetFirewallRule | Where-Object { $_.DisplayName -like '*ws_conuni_dotnet_rest*' } | Set-NetFirewallRule -Action Allow"

echo 2. Abriendo puerto 5000 TCP para el celular...
netsh advfirewall firewall add rule name="Kestrel_REST_5000" dir=in action=allow protocol=TCP localport=5000 profile=any

echo.
echo ====================================================================
echo      LISTO! EL FIREWALL HA SIDO DESBLOQUEADO CON EXITO
echo      Tu celular ahora podra conectarse a: 192.168.100.15
echo ====================================================================
echo.
pause
