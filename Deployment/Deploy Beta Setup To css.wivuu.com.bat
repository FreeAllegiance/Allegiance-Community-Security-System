@ECHO Did you build it in x86 release mode?? Did you increment the product code??
PAUSE

PUSHD C:\Source\Allegiance\CSS\Deployment\Allegiance.CommunitySecuritySystem.BetaSetup\Release

7z a 458620d4-ab87-4caf-9c7e-b838da88f522.zip *.*

XCOPY /Y 458620d4-ab87-4caf-9c7e-b838da88f522.zip \\css.wivuu.com\c$\Inetpub\CSSManagement\458620d4-ab87-4caf-9c7e-b838da88f522.zip

POPD 

PUSHD C:\Source\Allegiance\CSS\Client\Allegiance.CommunitySecuritySystem.Client\bin\x86\Release

7z a Launcher.zip Launcher.exe
7z a Launcher.zip Launcher.pdb

XCOPY /Y Launcher.zip \\css.wivuu.com\c$\1

POPD 

PAUSE