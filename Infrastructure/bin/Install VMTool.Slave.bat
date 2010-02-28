@echo off
sc create VMTool.Slave binPath= "%~dp0VMTool.Slave.exe" start= auto DisplayName= "VMTool Slave"
echo.
echo Remember to change service account to user "Infrastructure".
echo.