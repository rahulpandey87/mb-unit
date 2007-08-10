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
xcopy /I /Y Html\*.* Build\Html
xcopy /I /Y Icons\*.* Build\Icons
xcopy /I /Y Lib\*.* Build\Lib
xcopy /I /Y Presentation\*.* Build\Presentation
xcopy /I /Y Scripts\*.* Build\Scripts
xcopy /I /Y Styles\*.* Build\Styles
xcopy /I /Y bin\*.* Build\bin
xcopy /I /Y *.* Build
