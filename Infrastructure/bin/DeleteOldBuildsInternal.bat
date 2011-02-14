@echo off
REM Purges files from the distributables folder that have outlived
REM their usefulness.

c:\cygwin\bin\find /cygdrive/d/Distributables -daystart -maxdepth 1 -mindepth 1 -type f -mtime +14 -delete
c:\cygwin\bin\find /cygdrive/d/Distributables/APIDocs -daystart -maxdepth 1 -mindepth 1 -type d -mtime +14 -execdir c:\cygwin\bin\rm -rf "{}" ";"
c:\cygwin\bin\find /cygdrive/d/Distributables/LibCheck -daystart -maxdepth 1 -mindepth 1 -type d -mtime +14 -execdir c:\cygwin\bin\rm -rf "{}" ";"
c:\cygwin\bin\find /cygdrive/d/Distributables/GallioBook -daystart -maxdepth 1 -mindepth 1 -type d -mtime +14 -execdir c:\cygwin\bin\rm -rf "{}" ";"
c:\cygwin\bin\find /cygdrive/d/Distributables/Symbols -daystart -maxdepth 1 -mindepth 1 -type d -mtime +14 -execdir c:\cygwin\bin\rm -rf "{}" ";"
c:\cygwin\bin\find /cygdrive/d/Distributables/WebSites -daystart -maxdepth 1 -mindepth 1 -type d -mtime +14 -execdir c:\cygwin\bin\rm -rf "{}" ";"

exit /b 0