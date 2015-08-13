@ECHO Did you build it in x86 release mode?? Did you increment the product code??
PAUSE

PUSHD C:\Source\Allegiance\CSS\Deployment\Allegiance.CommunitySecuritySystem.BetaSetup\Release

del ACSS.zip

7z a ACSS.zip *.*

XCOPY /Y ACSS.zip c:\1\CSS

POPD 

PUSHD C:\Source\Allegiance\CSS\Client\Allegiance.CommunitySecuritySystem.Client\bin\x86\Release

7z a Launcher.zip Launcher.exe
7z a Launcher.zip Launcher.pdb

XCOPY /Y Launcher.zip c:\1\CSS

POPD 



PUSHD C:\Source\Allegiance\Allegiance\branch\ACSS_R6\objs10\FZRetail\Wintrek

7z a Allegiance.zip Allegiance.exe
@REM 7z a Allegiance.zip Allegiance.pdb

XCOPY /Y Allegiance.zip c:\1\CSS
XCOPY /Y Allegiance.pdb c:\1\CSS

POPD 



PUSHD C:\Source\Allegiance\Allegiance\branch\ACSS_R6\objs10\FZRetail\Lobby

7z a Lobby.zip AllLobby.exe
7z a Lobby.zip AllLobby.pdb

XCOPY /Y Lobby.zip c:\1\CSS

POPD 



PUSHD C:\Source\Allegiance\Allegiance\branch\ACSS_R6\objs10\FZRetail\FedSrv

7z a Server.zip AllSrv.exe
7z a Server.zip AllSrv.pdb
7z a Server.zip ..\AGC\AGC.dll
7z a Server.zip ..\AGC\AGC.pdb

XCOPY /Y Server.zip c:\1\CSS

POPD 


PAUSE