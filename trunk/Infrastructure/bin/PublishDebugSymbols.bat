@if defined ECHO (echo %ECHO%) else (echo off)
REM
REM Publishes Symbols to a symbol store.
REM
setlocal
set SYMBOLS=D:\Symbols
set BASE=D:\Distributables
set SYMSTORE_EXE=C:\Program Files\Debugging Tools for Windows (x86)\symstore.exe

set VERSION=%~1
if not defined VERSION (
    echo Usage: [version]
    echo.
    echo Available versions:
    dir /b "%BASE%\Symbols"
    exit /b 1
)

set SOURCE=%BASE%\Symbols\%VERSION%

echo.
echo Version: %VERSION%
echo Source : %SOURCE%
echo Target : %TARGET%
echo.

"%SYMSTORE_EXE%" add /s "%SYMBOLS%" /r /f "%SOURCE%" /t Gallio /v %VERSION% /compress /o
if errorlevel 1 goto :ERROR
exit /b 0

:ERROR
exit /b %ERRORLEVEL%


:CHECKFILE
if not exist "%SOURCE%\%~1" (
    echo Missing %~1!  Source directory may be incorrect.
    exit /b 1
)

exit /b 0

