@echo off
sc create VMTool.Slave binPath= "%~dp0VMTool.Slave.exe" start= auto DisplayName= "VMTool Slave"