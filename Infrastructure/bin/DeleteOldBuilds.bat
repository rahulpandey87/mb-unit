@echo off
REM Purges files from the distributables folder that have outlived
REM their usefulness.

call %~dp0DeleteOldBuildsInternal.bat >%~dp0DeleteOldBuilds.log 2>&1
exit /b %ERRORLEVEL%