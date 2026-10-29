@echo off
net session >nul 2>&1
if %errorLevel% neq 0 (
    powershell -Command "Start-Process '%~0' -Verb RunAs"
    exit /b
)

title Desbloquear Conectividad Total - Servidor SOAP Payara (8080)
echo ====================================================================
echo      DESBLOQUEANDO CONEXION INALAMBRICA PARA EL CELULAR / CLIENTES
echo ====================================================================
echo.
echo 1. Cambiando conexion a Red Privada...
powershell -Command "Get-NetConnectionProfile | Set-NetConnectionProfile -NetworkCategory Private"

echo 2. Desactivando temporalmente el Firewall de Windows...
netsh advfirewall set allprofiles state off

echo.
echo ====================================================================
echo   EXITO: EL FIREWALL ESTA DESACTIVADO Y EL PUERTO 8080 ESTA ABIERTO
echo ====================================================================
echo.
powershell -Command "$ip = (Get-NetIPAddress -InterfaceAlias 'Wi-Fi' -AddressFamily IPv4).IPAddress; Write-Host 'En tu celular escribe esta IP exacta: ' -NoNewline; Write-Host $ip -ForegroundColor Green -BackgroundColor Black"
echo.
echo (Nota: Cuando termines tu presentacion, puedes volver a encender
echo        el firewall ejecutando: netsh advfirewall set allprofiles state on)
echo.
pause
