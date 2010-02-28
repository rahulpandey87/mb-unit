@echo off
REM Builds and installs the tool into the bin directory and into the
REM CCNet server directory.

setlocal
set SRC_DIR=%~dp0
set BIN_DIR=%SRC_DIR%..\bin
set CCNET_SERVER_DIR=%SRC_DIR%..\ccnet\Server

echo Building projects.
%SystemRoot%\Microsoft.Net\Framework\v3.5\msbuild.exe "%SRC_DIR%VMTool\VMTool.sln"
echo.

echo Copying client, master and slave.
copy /y "%SRC_DIR%VMTool\VMTool.Client\bin\CommandLine.dll" "%BIN_DIR%"
copy /y "%SRC_DIR%VMTool\VMTool.Client\bin\Thrift.dll" "%BIN_DIR%"
copy /y "%SRC_DIR%VMTool\VMTool.Client\bin\VMTool.dll" "%BIN_DIR%"
copy /y "%SRC_DIR%VMTool\VMTool.Client\bin\VMTool.Client.exe" "%BIN_DIR%"
copy /y "%SRC_DIR%VMTool\VMTool.Client\bin\VMTool.Client.exe.config" "%BIN_DIR%"
copy /y "%SRC_DIR%VMTool\VMTool.Master\bin\log4net.dll" "%BIN_DIR%"
copy /y "%SRC_DIR%VMTool\VMTool.Master\bin\VMTool.Master.exe" "%BIN_DIR%"
copy /y "%SRC_DIR%VMTool\VMTool.Master\bin\VMTool.Master.exe.config" "%BIN_DIR%"
copy /y "%SRC_DIR%VMTool\VMTool.Slave\bin\VMTool.Slave.exe" "%BIN_DIR%"
copy /y "%SRC_DIR%VMTool\VMTool.Slave\bin\VMTool.Slave.exe.config" "%BIN_DIR%"
echo.

echo Copying CCNet server plugin.
copy /y "%SRC_DIR%VMTool\CCNet.VMTool.Plugin\bin\Thrift.dll" "%CCNET_SERVER_DIR%"
copy /y "%SRC_DIR%VMTool\CCNet.VMTool.Plugin\bin\VMTool.dll" "%CCNET_SERVER_DIR%"
copy /y "%SRC_DIR%VMTool\CCNet.VMTool.Plugin\bin\CCNet.VMTool.Plugin.dll" "%CCNET_SERVER_DIR%"
echo.
