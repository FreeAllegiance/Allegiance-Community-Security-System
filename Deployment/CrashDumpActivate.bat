@echo off

@rem ----[ This code block detects if the script is being running with admin PRIVILEGES If it isn't it pauses and then quits]-------

NET SESSION >nul 2>&1
IF %ERRORLEVEL% EQU 0 (
    ECHO Administrator PRIVILEGES Detected! 
) ELSE (
   echo ######## ########  ########   #######  ########  
   echo ##       ##     ## ##     ## ##     ## ##     ## 
   echo ##       ##     ## ##     ## ##     ## ##     ## 
   echo ######   ########  ########  ##     ## ########  
   echo ##       ##   ##   ##   ##   ##     ## ##   ##   
   echo ##       ##    ##  ##    ##  ##     ## ##    ##  
   echo ######## ##     ## ##     ##  #######  ##     ## 
   echo.
   echo.
   echo ####### ERROR: ADMINISTRATOR PRIVILEGES REQUIRED #########
   echo This script must be run as administrator to work properly!  
   echo If you're seeing this after clicking on a start menu icon, then right click on the shortcut and select "Run As Administrator".
   echo ##########################################################
   echo.
   PAUSE
   EXIT /B 1
)


PUSHD %~dp0


IF NOT EXIST AeDebugBackup.reg regedit /E AeDebugBackup.reg "HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Windows NT\CurrentVersion\AeDebug"

echo Windows Registry Editor Version 5.00 > tempreg.reg
echo. >> tempreg.reg
echo [HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Windows NT\CurrentVersion\AeDebug] >> tempreg.reg 

set currentPath=%CD%
set currentPath=%currentPath:\=\\%


echo "Debugger"="\"%currentPath%\\procdump.exe\" -mp %%ld \"%currentPath%\\minidump.dmp\"" >> tempreg.reg 
REM echo "Auto"="1" >> tempreg.reg 


regedit /S tempreg.reg

echo AeDebug handler now configured in registry for automatic crash dumps. Please run CrashDumpDeactivate.bat when done.

POPD

pause