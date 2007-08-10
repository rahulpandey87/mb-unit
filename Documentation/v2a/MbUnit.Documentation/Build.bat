@echo off
REM MbUnit.Documentation v2 build script
REM This must be running with Administrator privileges

REM Removing Read Only Status
attrib -R /S /D ..\..\*.*

REM Copying Custom Topics...
xcopy /E /Y ..\..\..\Docs\*.* "Custom Topics"


REM Building Project...
%SystemRoot%\Microsoft.NET\Framework\v2.0.50727\msbuild.exe "MbUnit.Documentation.csproj" /v:minimal

REM Copying to Build folder...
xcopy /I /E /Y Html\*.* Build\Html
xcopy /I /E /Y Icons\*.* Build\Icons
xcopy /I /E /Y Presentation\*.* Build\Presentation
xcopy /I /E /Y Scripts\*.* Build\Scripts
xcopy /I /E /Y Styles\*.* Build\Styles
xcopy /I /E /Y bin\*.* Build\bin
xcopy /I /E /Y *.* Build
