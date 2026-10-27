@echo off
title Cliente de Escritorio - Monsters Inc RESTful (GR08)
color 0B
echo ======================================================================
echo    MONSTERS INC. - CONVERSOR DE UNIDADES RESTFUL .NET 8 (GR08)
echo                     CLIENTE DE ESCRITORIO (WPF)
echo ======================================================================
echo.
echo Verificando compilacion y ejecutando aplicacion WPF...
echo.

cd /d "%~dp0ConUni_CliEsc_GR08"
dotnet run -c Release --no-build

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo Compilando y ejecutando...
    dotnet run
)

pause
