@if defined ECHO (echo %ECHO%) else (echo off)
REM
REM Publishes the specified API documentation folder to the web site.
REM
setlocal
set REMOTERSYNC=%~dp0RemoteRSync.bat
set TARGET=C:\Inetpub\www.gallio.org\book
set BASE=C:\Infrastructure\data\Distributables

set VERSION=%~1
if not defined VERSION (
    echo Usage: [version]
    echo.
    echo Available versions:
    dir /b "%BASE%\GallioBook"
    exit /b 1
)

set SOURCE=%BASE%\GallioBook\%VERSION%

echo.
echo Version: %VERSION%
echo Source : %SOURCE%
echo Target : %TARGET%
echo.

call :CHECKFILE GallioBook.chm
if errorlevel 1 goto :ERROR
call :CHECKFILE GallioBook.pdf
if errorlevel 1 goto :ERROR
call :CHECKFILE XHtml\index.html
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

