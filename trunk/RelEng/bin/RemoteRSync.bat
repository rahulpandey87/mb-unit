@echo off
REM Runs rsync using a remote host.
setlocal
set HOST=%~1
set SRC=%~2
set DEST=%~3
if not defined DEST (
    echo Usage: [host] [src] [dest]
	exit /b 1
)

for /F %%V in ('echo %SRC% ^| c:\cygwin\bin\sed -e "s/\\\\/\\//g;s/://"') do set SRC_CYGPATH=/cygdrive/%%V
for /F %%V in ('echo %DEST% ^| c:\cygwin\bin\sed -e "s/\\\\/\\//g;s/://"') do set DEST_CYGPATH=/cygdrive/%%V

echo RSync from %SRC_CYGPATH% to %DEST_CYGPATH% on %HOST%.

REM Hangs due to bad interaction of rsync and ssh pipes on cygwin.
REM c:\cygwin\bin\rsync -rt --delete --perms --chmod=ugo+rwx,Dugo+x -e "c:\cygwin\bin\ssh" "%SRC_CYGPATH%/" "sync@%HOST%:%DEST_CYGPATH%"

REM Uses local rsync tunnel configured seperately using autossh.
REM c:\cygwin\bin\rsync -rvt --delete "%SRC_CYGPATH%/" "rsync://localhost:65532%DEST_CYGPATH%"

c:\cygwin\bin\rsync -rvt --delete "%SRC_CYGPATH%/" "rsync://192.168.128.12%DEST_CYGPATH%"

exit /b %ERRORLEVEL%
