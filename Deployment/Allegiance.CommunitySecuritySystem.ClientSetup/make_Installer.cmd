REM @echo off

PUSHD %1

DIR

REM This script needs NSIS and plugins installed into it's default localtion (e.g. C:\Program Files\NSIS)

REM Set this to the build output root (objs12) for the allegiance output files.
SET AllegianceObjsDir=..\..\..\Allegiance\objs12\AZRetail

REM Remove Read-only/Archive flag from all files
REM Somehow this cased problems with build installer (crash during downloading filelist.txt/MotD)
REM I think it's a problem with TortoiseSVN, so this is just a dirty workaround
attrib -R -A /S *.*

REM Copy allegiance.exe from build output folder.
COPY /Y %AllegianceObjsDir%\Wintrek\Allegiance.exe Resources\Allegiance\Allegiance.exe
COPY /Y %AllegianceObjsDir%\Reloader\Reloader.exe Resources\Allegiance\Reloader.exe

REM Run from 32 bit systems default location
if EXIST "%ProgramFiles%\NSIS\makensis.exe" "%ProgramFiles%\NSIS\makensis.exe" Allegiance.nsi

REM Run from 64 bit systems default location
if EXIST "%ProgramFiles(x86)%\NSIS\makensis.exe" "%ProgramFiles(x86)%\NSIS\makensis.exe" Allegiance.nsi

POPD

REM Don't close the window automaticly, if finished.
pause

