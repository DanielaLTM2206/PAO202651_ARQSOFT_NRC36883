@echo off
net session >nul 2>&1
if %errorLevel% neq 0 (
    powershell -Command "Start-Process '%~0' -Verb RunAs"
    exit /b
)

title Desbloquear Firewall - Servidor SOAP Payara (Puerto 8080)
echo ====================================================================
echo      DESBLOQUEANDO FIREWALL DE WINDOWS PARA EL SERVIDOR SOAP (8080)
echo ====================================================================
echo.
echo Abriendo puerto 8080 TCP para celulares y clientes externos...
netsh advfirewall firewall add rule name="Payara_SOAP_8080" dir=in action=allow protocol=TCP localport=8080 profile=any

echo.
echo ====================================================================
echo      LISTO! EL PUERTO 8080 HA SIDO DESBLOQUEADO CON EXITO
echo      Tu celular ahora podra conectarse por Wi-Fi a: 192.168.100.15:8080
echo ====================================================================
timeout /t 3 >nul