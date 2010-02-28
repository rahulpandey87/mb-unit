@echo off
sc create VMTool.Master binPath= "%~dp0VMTool.Master.exe" start= auto DisplayName= "VMTool Master"