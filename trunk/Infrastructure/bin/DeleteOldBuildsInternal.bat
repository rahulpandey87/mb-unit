@echo off
REM Purges files from the distributables folder that have outlived
REM their usefulness.

c:\cygwin\bin\find c:\Infrastructure\data\Distributables -daystart -maxdepth 1 -mindepth 1 -type f -mtime +7 -delete
c:\cygwin\bin\find c:\Infrastructure\data\Distributables\APIDocs -daystart -maxdepth 1 -mindepth 1 -type d -mtime +7 -execdir c:\cygwin\bin\rm -rf "{}" ";"
c:\cygwin\bin\find c:\Infrastructure\data\Distributables\LibCheck -daystart -maxdepth 1 -mindepth 1 -type d -mtime +7 -execdir c:\cygwin\bin\rm -rf "{}" ";"
c:\cygwin\bin\find c:\Infrastructure\data\Distributables\GallioBook -daystart -maxdepth 1 -mindepth 1 -type d -mtime +7 -execdir c:\cygwin\bin\rm -rf "{}" ";"

exit /b 0