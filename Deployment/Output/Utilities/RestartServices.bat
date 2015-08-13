@ECHO OFF

ECHO Server Kicker - Kicking the server.

IF "%1"=="tag" GOTO Tag

IF "%1"=="allsrv" GOTO AllSrv

IF "%1"=="lobby" GOTO Lobby

GOTO End

:Lobby
ECHO Restarting the lobby.

sc stop "TAGService"
sc stop "AllSrv"
sc stop "AllLobby"

sc start "AllLobby"
choice /T 5 /C Y /CS /D Y /M "Waiting for Lobby to warm up. Continue "

sc start "AllSrv"
choice /T 5 /C Y /CS /D Y /M "Waiting for AllSrv to warm up. Continue "

sc start "TAGService"
GOTO End


:AllSrv
ECHO Restarting AllSrv.

sc stop "TAGService"
sc stop "AllSrv"

sc start "AllSrv"
choice /T 5 /C Y /CS /D Y /M "Waiting for AllSrv to warm up. Continue "

sc start "TAGService"
GOTO End

:Tag
ECHO Restarting TAG.

sc stop "TAGService"

sc start "TAGService"


:End