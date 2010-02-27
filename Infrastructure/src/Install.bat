@echo off
REM Builds and installs the tool into the bin directory and into the
REM CCNet server directory.

setlocal
set SRC_DIR=%~dp0
set BIN_DIR=%SRC_DIR%..\bin
set CCNET_SERVER_DIR=%SRC_DIR%..\public\CCNet\Server

echo Building projects.
%SystemRoot%\Microsoft.Net\Framework\v3.5\msbuild.exe "%SRC_DIR%VMTool\VMTool.sln"
echo.

echo Copying client.
copy /y "%SRC_DIR%VMTool\VMTool.Client\bin\*.dll" "%BIN_DIR%"
copy /y "%SRC_DIR%VMTool\VMTool.Client\bin\*.exe" "%BIN_DIR%"
copy /y "%SRC_DIR%VMTool\VMTool.Client\bin\*.exe.config" "%BIN_DIR%"
echo.

echo Copying CCNet server plugin.
copy /y "%SRC_DIR%VMTool\CCNet.VMTool.Plugin\bin\*.dll" "%CCNET_SERVER_DIR%"
echo.
