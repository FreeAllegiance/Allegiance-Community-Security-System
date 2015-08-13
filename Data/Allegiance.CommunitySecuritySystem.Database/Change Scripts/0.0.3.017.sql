USE [CSSStats]
GO
/****** Object:  StoredProcedure [dbo].[ArchiveData]    Script Date: 09/08/2014 14:52:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		BackTrak
-- Create date: 9/7/2014
-- Description:	Archive large tables and remove older data from CSSStats
-- =============================================
CREATE PROCEDURE [dbo].[ArchiveData] 
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    
    DECLARE @ArchiveDate DATETIME

SELECT @ArchiveDate = DATEADD(MONTH, -1, GetDate())


SET Identity_insert CSSStatsArchive.dbo.GameServer ON

INSERT INTO CSSStatsArchive.dbo.GameServer
(GameServerID, GameServerName, GameServerOwnerName)
	SELECT * 
	FROM CSSStats.dbo.GameServer gs
	WHERE gs.GameServerID NOT IN (SELECT GameServerID FROM CSSStatsArchive.dbo.GameServer)

SET Identity_insert CSSStatsArchive.dbo.GameServer OFF

SET Identity_insert CSSStatsArchive.dbo.GameServerIP ON

INSERT INTO CSSStatsArchive.dbo.GameServerIP
([GameServerIpID]
      ,[GameServerID]
      ,[IPAddress])
	SELECT * 
	FROM CSSStats.dbo.GameServerIP 
	WHERE [GameServerIpID] NOT IN (SELECT GameServerIpID FROM CSSStatsArchive.dbo.GameServerIP)

SET Identity_insert CSSStatsArchive.dbo.GameServerIP OFF


SET Identity_insert CSSStatsArchive.dbo.Game ON

INSERT INTO CSSStatsArchive.dbo.Game
(
		[GameIdentID]
      ,[GameID]
      ,[GameServer]
      ,[GameName]
      ,[GameCore]
      ,[GameMap]
      ,[GameSquadGame]
      ,[GameConquest]
      ,[GameDeathMatch]
      ,[GameDeathmatchGoal]
      ,[GameFriendlyFire]
      ,[GameRevealMap]
      ,[GameDevelopments]
      ,[GameShipyards]
      ,[GameDefections]
      ,[GameInvulStations]
      ,[GameStatsCount]
      ,[GameMaxImbalance]
      ,[GameStartingMoney]
      ,[GameTotalMoney]
      ,[GameResources]
      ,[GameStartTime]
      ,[GameEndTime]
)
	SELECT * 
	FROM CSSStats.dbo.Game 
	WHERE [GameIdentID] NOT IN (SELECT [GameIdentID] FROM CSSStatsArchive.dbo.Game)
	AND GameEndTime < @ArchiveDate

SET Identity_insert CSSStatsArchive.dbo.Game OFF

SET Identity_insert CSSStatsArchive.dbo.GameChatLog ON

INSERT INTO CSSStatsArchive.dbo.GameChatLog
(
	[GameChatLogIdentID]
	,[GameID]
	,[GameChatTime]
	,[GameChatSpeakerName]
	,[GameChatTargetName]
	,[GameChatText]	
)
	SELECT * 
	FROM CSSStats.dbo.GameChatLog 
	WHERE GameChatLogIdentID NOT IN (SELECT GameChatLogIdentID FROM CSSStatsArchive.dbo.GameChatLog)
	AND GameID IN (SELECT GameIdentID FROM CSSStatsArchive.dbo.Game) 

SET Identity_insert CSSStatsArchive.dbo.GameChatLog OFF


SET Identity_insert CSSStatsArchive.dbo.GameEvent ON

INSERT INTO CSSStatsArchive.dbo.GameEvent
(
	[GameID]
	,[GameEventID]
	,[EventID]
	,[GameEventTime]
	,[GameEventPerformerID]
	,[GameEventPerformerLoginID]
	,[GameEventPerformerName]
	,[GameEventTargetID]
	,[GameEventTargetLoginID]
	,[GameEventTargetName]
	,[GameEventIndirectID]
	,[GameEventIndirectName]
)
	SELECT * 
	FROM CSSStats.dbo.GameEvent 
	WHERE GameEventID NOT IN (SELECT GameEventID FROM CSSStatsArchive.dbo.GameEvent)
	AND GameID IN (SELECT GameIdentID FROM CSSStatsArchive.dbo.Game) 

SET Identity_insert CSSStatsArchive.dbo.GameEvent OFF


SET Identity_insert CSSStatsArchive.dbo.GameEventType ON

INSERT INTO CSSStatsArchive.dbo.GameEventType
(
	[GameEventTypeID]
	,[GameEventID]
	,[GameEventCode]
	,[GameEventDesc]
)
	SELECT * 
	FROM CSSStats.dbo.GameEventType 
	WHERE GameEventTypeID NOT IN (SELECT GameEventTypeID FROM CSSStatsArchive.dbo.GameEventType)

SET Identity_insert CSSStatsArchive.dbo.GameEventType OFF


SET Identity_insert CSSStatsArchive.dbo.GameTeam ON

INSERT INTO CSSStatsArchive.dbo.GameTeam
(
	[GameTeamIdentID]
	,[GameTeamID]
	,[GameID]
	,[GameTeamNumber]
	,[GameTeamName]
	,[GameTeamCommander]
	,[GameTeamCommanderLoginID]
	,[GameTeamFaction]
	,[GameTeamStarbase]
	,[GameTeamSupremacy]
	,[GameTeamTactical]
	,[GameTeamExpansion]
	,[GameTeamShipyard]
	,[GameTeamWinner]
)
	SELECT * 
	FROM CSSStats.dbo.GameTeam 
	WHERE [GameTeamIdentID] NOT IN (SELECT [GameTeamIdentID] FROM CSSStatsArchive.dbo.GameTeam)
	AND GameID IN (SELECT GameIdentID FROM CSSStatsArchive.dbo.Game) 

SET Identity_insert CSSStatsArchive.dbo.GameTeam OFF


SET Identity_insert CSSStatsArchive.dbo.GameTeamMember ON

INSERT INTO CSSStatsArchive.dbo.GameTeamMember
(
	[GameTeamMemberIdentID]
	,[GameTeamID]
	,[GameTeamMemberCallsign]
	,[GameTeamMemberDuration]
	,[GameTeamMemberLoginID]
	,[Score]
)
	SELECT * 
	FROM CSSStats.dbo.GameTeamMember 
	WHERE [GameTeamMemberIdentID] NOT IN (SELECT [GameTeamMemberIdentID] FROM CSSStatsArchive.dbo.GameTeamMember)
	AND GameTeamID IN (SELECT GameTeamIdentID FROM CSSStatsArchive.dbo.GameTeam) 

SET Identity_insert CSSStatsArchive.dbo.GameTeamMember OFF

-- Batch delete GameChatLog
declare @GameIDs table ( GameID int )
declare @TotalDeleted int = 0
while (1 = 1)
begin

	SET NOCOUNT ON  

	delete @GameIDs
	
	insert into @GameIDs
	(GameID)
	select top 100 GameID from CSSStats.dbo.GameChatLog (NOLOCK) where GameID in (SELECT GameIdentID FROM CSSStatsArchive.dbo.Game)
	
	if @@ROWCOUNT = 0
	begin
		break
	end
	
	select @TotalDeleted += count(*) from @GameIDs
	
	SET NOCOUNT OFF
	delete CSSStats.dbo.GameChatLog where GameID in (select GameID from @GameIDs)
	SET NOCOUNT ON 
	
	print 'GameChatLog Deleted ' + convert(varchar(20), @TotalDeleted)
end

print 'GameChatLog Deleted ' + convert(varchar(20), @TotalDeleted)
	

-- Batch delete GameEvent

while (1 = 1)
begin

	SET NOCOUNT ON  

	delete @GameIDs
	
	insert into @GameIDs
	(GameID)
	select top 100 GameID from CSSStats.dbo.GameEvent (NOLOCK) where GameID in (SELECT GameIdentID FROM CSSStatsArchive.dbo.Game)
	
	if @@ROWCOUNT = 0
	begin
		break
	end
	
	select @TotalDeleted += count(*) from @GameIDs
	
	SET NOCOUNT OFF
	delete CSSStats.dbo.GameEvent where GameID in (select GameID from @GameIDs)
	SET NOCOUNT ON 
	
	print 'GameEvent Deleted ' + convert(varchar(20), @TotalDeleted)
end

print 'GameEvent Deleted ' + convert(varchar(20), @TotalDeleted)
    
END
