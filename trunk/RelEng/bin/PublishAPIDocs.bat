@if defined ECHO (echo %ECHO%) else (echo off)
REM
REM Publishes the specified Book folder to the web site.
REM
setlocal
set REMOTERSYNC=%~dp0RemoteRSync.bat
set TARGET=C:\Inetpub\www.gallio.org\api
set BASE=C:\RelEng\Distributables\APIDocs

set VERSION=%~1
if not defined VERSION (
    echo Usage: [version]
    echo.
    echo Available versions:
    dir /b "%BASE%"
    exit /b 1
)

set SOURCE=%BASE%\%VERSION%

echo.
echo Version: %VERSION%
echo Source : %SOURCE%
echo Target : %TARGET%
echo.

call :CHECKFILE Gallio.chm
if errorlevel 1 goto :ERROR
call :CHECKFILE index.html
if errorlevel 1 goto :ERROR

call "%REMOTERSYNC%" gallio.org "%SOURCE%" "%TARGET%"
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