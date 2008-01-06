@echo off
REM Purges files from the distributables folder that have outlived
REM their usefulness.

c:\cygwin\bin\find c:\RelEng\Distributables -daystart -maxdepth 1 -mindepth 1 -type f -mtime +7 -delete
