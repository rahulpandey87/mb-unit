@echo off
%~dp0VMTool.Client -m localhost -v "Gallio Web Server" savestate
%~dp0VMTool.Client -m localhost -v "Gallio Build Server" savestate
%~dp0VMTool.Client -m localhost -v "Gallio Build Agent - Windows 2008 x64, Everything" poweroff
%~dp0VMTool.Client -m localhost -v "Gallio Build Agent - Windows 2003 x86, Everything" poweroff
pause