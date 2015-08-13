SET ServerDeploymentProjectDir=%1

PUSHD %ServerDeploymentProjectDir%

IF EXIST "%ServerDeploymentProjectDir%Output" RMDIR /S /Q "%ServerDeploymentProjectDir%Output"

MKDIR Output

ECHO Writing to %ServerDeploymentProjectDir%Output


@REM ************************************************************************
@REM Copy the database scripts.
@REM ************************************************************************
XCOPY "%ServerDeploymentProjectDir%\..\Data\Allegiance.CommunitySecuritySystem.Database\Create Scripts\*.*.*.*.SQL" "%ServerDeploymentProjectDir%Output\Database\*.SQL"


@REM ************************************************************************
@REM Copy the root website files.
@REM ************************************************************************
XCOPY "%ServerDeploymentProjectDir%\..\Server\Allegiance.CommunitySecuritySystem.Server\bin\*.dll" "%ServerDeploymentProjectDir%Output\Website\bin\*.dll"
XCOPY /S "%ServerDeploymentProjectDir%\..\Server\Allegiance.CommunitySecuritySystem.Server\*.svc" "%ServerDeploymentProjectDir%Output\Website\*.svc"
ECHO F | XCOPY "%ServerDeploymentProjectDir%\..\Documents\Templates\Allegiance.CommunitySecuritySystem.Server.config" "%ServerDeploymentProjectDir%Output\Website\web.config.sample"


@REM ************************************************************************
@REM Copy the autoupdate service files.
@REM ************************************************************************
MKDIR "%ServerDeploymentProjectDir%Output\Website\AutoUpdate"
XCOPY "%ServerDeploymentProjectDir%\..\Server\Allegiance.CommunitySecuritySystem.AutoUpdate\bin\*.dll" "%ServerDeploymentProjectDir%Output\Website\AutoUpdate\bin\*.dll"
ECHO F | XCOPY "%ServerDeploymentProjectDir%\..\Documents\Templates\Allegiance.CommunitySecuritySystem.AutoUpdate.config" "%ServerDeploymentProjectDir%Output\Website\AutoUpdate\web.sample.config"


@REM ************************************************************************
@REM Copy the Lobby service files.
@REM ************************************************************************
XCOPY "%ServerDeploymentProjectDir%\..\Server\Allegiance.CommunitySecuritySystem.Lobby\bin\*.dll" "%ServerDeploymentProjectDir%Output\Website\LobbyAuthentication\bin\*.dll"
ECHO F | XCOPY "%ServerDeploymentProjectDir%\..\Documents\Templates\Allegiance.CommunitySecuritySystem.Lobby.config" "%ServerDeploymentProjectDir%Output\Website\LobbyAuthentication\web.sample.config"


@REM ************************************************************************
@REM Copy the BlackBox Generator files.
@REM ************************************************************************
MKDIR "%ServerDeploymentProjectDir%Output\TaskHandler\Output"
XCOPY "%ServerDeploymentProjectDir%\..\Tasks\Allegiance.CommunitySecuritySystem.TaskHandler\bin\Debug\*.dll" "%ServerDeploymentProjectDir%Output\TaskHandler\*.dll"
ECHO F | XCOPY "%ServerDeploymentProjectDir%\..\Tasks\Allegiance.CommunitySecuritySystem.TaskHandler\bin\Debug\TaskHandler.exe" "%ServerDeploymentProjectDir%Output\TaskHandler\TaskHandler.exe"
XCOPY "%ServerDeploymentProjectDir%\..\Tasks\Allegiance.CommunitySecuritySystem.BlackboxGenerator\Resources\*.txt" "%ServerDeploymentProjectDir%Output\TaskHandler\Resources\*.txt"

ECHO F | XCOPY "%ServerDeploymentProjectDir%\..\Documents\Templates\Allegiance.CommunitySecuritySystem.TaskHandler.config" "%ServerDeploymentProjectDir%Output\TaskHandler\TaskHandler.exe.sample.config"

@REM ************************************************************************
@REM Copy all utilities.
@REM ************************************************************************
MKDIR "%ServerDeploymentProjectDir%Output\Utilities"
XCOPY /S "%ServerDeploymentProjectDir%Allegiance.CommunitySecuritySystem.ServerDeployment\Utilities\*.*" "%ServerDeploymentProjectDir%Output\Utilities\"

ECHO.
ECHO.
ECHO.
ECHO **************************************************************
ECHO Files are at: %ServerDeploymentProjectDir%Output
ECHO **************************************************************

POPD