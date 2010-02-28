@echo off
sc create VMTool.Master binPath= "%~dp0VMTool.Master.exe" start= auto DisplayName= "VMTool Master"
echo.
echo Remember to change service account to user "Infrastructure".
echo.