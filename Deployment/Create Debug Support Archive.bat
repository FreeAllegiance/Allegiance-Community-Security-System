PUSHD C:\Source\Allegiance\CSS\Deployment

del DebugSupport.zip
del AllegiancePDB.zip


7z a DebugSupport.zip procdump.exe
7z a DebugSupport.zip CrashDumpActivate.bat
7z a DebugSupport.zip CrashDumpDeactivate.bat

POPD

PUSHD C:\Source\Allegiance\Allegiance\branch\ACSS_R6\objs10\FZRetail\Wintrek



7z a C:\Source\Allegiance\CSS\Deployment\AllegiancePDB.zip Allegiance.pdb

POPD


PAUSE