@echo off
%~dp0VMTool.Client -m localhost -v "Gallio Build Server" savestate
%~dp0VMTool.Client -m localhost -v "Gallio Build Agent - Windows 2008 x64, Everything" savestate
%~dp0VMTool.Client -m localhost -v "Gallio Build Agent - Windows 2003 x86, Everything" savestate
