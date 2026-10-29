@echo off
net session >nul 2>&1
if %errorLevel% neq 0 (
    powershell -Command "Start-Process '%~0' -Verb RunAs"
    exit /b
)

title Reactivar Firewall de Windows
echo ====================================================================
echo             REACTIVANDO FIREWALL DE WINDOWS
echo ====================================================================
echo.
netsh advfirewall set allprofiles state on
echo Listo! El firewall ha sido reactivado normalmente.
pause
