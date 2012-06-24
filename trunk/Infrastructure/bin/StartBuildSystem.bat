@echo off
%~dp0VMTool.Client -m localhost -v "Gallio Web Server" start
%~dp0VMTool.Client -m localhost -v "Gallio Build Server" start
REM %~dp0VMTool.Client -m localhost -v "Gallio Development Box - Windows 2008 x64" start
pause