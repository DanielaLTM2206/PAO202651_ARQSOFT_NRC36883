@echo off
title Conectar Celular para Presentacion - Monsters Inc. GR08
echo ====================================================================
echo     PREPARANDO CONEXION PARA PRESENTACION (SOAP JAVA GR08)
echo ====================================================================
echo.
echo 1. Verificando celular conectado por USB...
"C:\Users\danie\AppData\Local\Android\Sdk\platform-tools\adb.exe" devices
echo.
echo 2. Activando tunel inverso USB (Puerto 8080)...
"C:\Users\danie\AppData\Local\Android\Sdk\platform-tools\adb.exe" reverse tcp:8080 tcp:8080
echo.
echo 3. Abriendo la aplicacion en el celular...
"C:\Users\danie\AppData\Local\Android\Sdk\platform-tools\adb.exe" shell monkey -p ec.edu.monster.conuniclientemovil -c android.intent.category.LAUNCHER 1 >nul 2>&1
echo.
echo ====================================================================
echo  LISTO! Tu celular ya esta conectado directamente al Servidor SOAP.
echo  No dependes del Wi-Fi de la universidad. Funciona 100% offline.
echo ====================================================================
echo.
pause