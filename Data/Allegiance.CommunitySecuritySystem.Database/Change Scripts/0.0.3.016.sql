USE [CSSStatsArchive]
GO

/*
DROP TABLE Game
DROP TABLE GameChatLog
DROP TABLE GameEvent
DROP TABLE GameEventType
DROP TABLE GameServer
DROP TABLE GameServerIP
DROP TABLE GameTeam
DROP TABLE GameTeamMember
*/



/****** Object:  Table [dbo].[GameEventType]    Script Date: 09/07/2014 21:30:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[GameEventType](
	[GameEventTypeID] [int] IDENTITY(1,1) NOT NULL,
	[GameEventID] [int] NOT NULL,
	[GameEventCode] [varchar](50) NOT NULL,
	[GameEventDesc] [varchar](255) NOT NULL,
 CONSTRAINT [PK_GameEventType] PRIMARY KEY CLUSTERED 
(
	[GameEventTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO




/****** Object:  Table [dbo].[GameServer]    Script Date: 09/07/2014 21:28:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[GameServer](
	[GameServerID] [int] IDENTITY(1,1) NOT NULL,
	[GameServerName] [varchar](255) NOT NULL,
	[GameServerOwnerName] [varchar](255) NOT NULL,
 CONSTRAINT [PK_GameServer] PRIMARY KEY CLUSTERED 
(
	[GameServerID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


/****** Object:  Table [dbo].[GameServerIP]    Script Date: 09/07/2014 21:28:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[GameServerIP](
	[GameServerIpID] [int] IDENTITY(1,1) NOT NULL,
	[GameServerID] [int] NOT NULL,
	[IPAddress] [varchar](20) NOT NULL,
 CONSTRAINT [PK_GameServerIP] PRIMARY KEY CLUSTERED 
(
	[GameServerIpID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[GameServerIP]  WITH CHECK ADD  CONSTRAINT [FK_GameServerIP_GameServer] FOREIGN KEY([GameServerID])
REFERENCES [dbo].[GameServer] ([GameServerID])
GO

ALTER TABLE [dbo].[GameServerIP] CHECK CONSTRAINT [FK_GameServerIP_GameServer]
GO







/****** Object:  Table [dbo].[Game]    Script Date: 09/07/2014 21:27:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO



CREATE TABLE [dbo].[Game](
	[GameIdentID] [int] IDENTITY(1,1) NOT NULL,
	[GameID] [int] NOT NULL,
	[GameServer] [int] NOT NULL,
	[GameName] [varchar](255) NOT NULL,
	[GameCore] [varchar](50) NOT NULL,
	[GameMap] [varchar](50) NOT NULL,
	[GameSquadGame] [bit] NOT NULL,
	[GameConquest] [bit] NOT NULL,
	[GameDeathMatch] [bit] NOT NULL,
	[GameDeathmatchGoal] [int] NULL,
	[GameFriendlyFire] [bit] NOT NULL,
	[GameRevealMap] [bit] NULL,
	[GameDevelopments] [bit] NOT NULL,
	[GameShipyards] [bit] NOT NULL,
	[GameDefections] [bit] NOT NULL,
	[GameInvulStations] [bit] NOT NULL,
	[GameStatsCount] [bit] NOT NULL,
	[GameMaxImbalance] [int] NOT NULL,
	[GameStartingMoney] [float] NOT NULL,
	[GameTotalMoney] [float] NOT NULL,
	[GameResources] [int] NOT NULL,
	[GameStartTime] [datetime] NOT NULL,
	[GameEndTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Game] PRIMARY KEY CLUSTERED 
(
	[GameIdentID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Game]  WITH CHECK ADD  CONSTRAINT [FK_Game_GameServer] FOREIGN KEY([GameServer])
REFERENCES [dbo].[GameServer] ([GameServerID])
GO

ALTER TABLE [dbo].[Game] CHECK CONSTRAINT [FK_Game_GameServer]
GO




/****** Object:  Table [dbo].[GameTeam]    Script Date: 09/07/2014 21:30:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[GameTeam](
	[GameTeamIdentID] [int] IDENTITY(1,1) NOT NULL,
	[GameTeamID] [int] NOT NULL,
	[GameID] [int] NOT NULL,
	[GameTeamNumber] [int] NOT NULL,
	[GameTeamName] [varchar](50) NOT NULL,
	[GameTeamCommander] [varchar](50) NOT NULL,
	[GameTeamCommanderLoginID] [int] NOT NULL,
	[GameTeamFaction] [varchar](50) NOT NULL,
	[GameTeamStarbase] [bit] NOT NULL,
	[GameTeamSupremacy] [bit] NOT NULL,
	[GameTeamTactical] [bit] NOT NULL,
	[GameTeamExpansion] [bit] NOT NULL,
	[GameTeamShipyard] [bit] NOT NULL,
	[GameTeamWinner] [bit] NOT NULL,
 CONSTRAINT [PK_GameTeam] PRIMARY KEY CLUSTERED 
(
	[GameTeamIdentID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[GameTeam]  WITH CHECK ADD  CONSTRAINT [FK_GameTeam_Game] FOREIGN KEY([GameID])
REFERENCES [dbo].[Game] ([GameIdentID])
GO

ALTER TABLE [dbo].[GameTeam] CHECK CONSTRAINT [FK_GameTeam_Game]
GO




/****** Object:  Table [dbo].[GameTeamMember]    Script Date: 09/07/2014 21:30:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[GameTeamMember](
	[GameTeamMemberIdentID] [int] IDENTITY(1,1) NOT NULL,
	[GameTeamID] [int] NOT NULL,
	[GameTeamMemberCallsign] [varchar](50) NOT NULL,
	[GameTeamMemberDuration] [int] NOT NULL,
	[GameTeamMemberLoginID] [int] NOT NULL,
	[Score] [int] NOT NULL,
 CONSTRAINT [PK_GameTeamMember] PRIMARY KEY CLUSTERED 
(
	[GameTeamMemberIdentID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[GameTeamMember]  WITH CHECK ADD  CONSTRAINT [FK_GameTeamMember_GameTeam] FOREIGN KEY([GameTeamID])
REFERENCES [dbo].[GameTeam] ([GameTeamIdentID])
GO

ALTER TABLE [dbo].[GameTeamMember] CHECK CONSTRAINT [FK_GameTeamMember_GameTeam]
GO

ALTER TABLE [dbo].[GameTeamMember] ADD  CONSTRAINT [DF_GameTeamMember_Score]  DEFAULT ((0)) FOR [Score]
GO




/****** Object:  Table [dbo].[GameEvent]    Script Date: 09/07/2014 21:29:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[GameEvent](
	[GameID] [int] NOT NULL,
	[GameEventID] [int] IDENTITY(1,1) NOT NULL,
	[EventID] [int] NOT NULL,
	[GameEventTime] [datetime] NOT NULL,
	[GameEventPerformerID] [int] NOT NULL,
	[GameEventPerformerLoginID] [int] NOT NULL,
	[GameEventPerformerName] [varchar](255) NOT NULL,
	[GameEventTargetID] [int] NOT NULL,
	[GameEventTargetLoginID] [int] NOT NULL,
	[GameEventTargetName] [varchar](255) NOT NULL,
	[GameEventIndirectID] [int] NOT NULL,
	[GameEventIndirectName] [varchar](255) NOT NULL,
 CONSTRAINT [PK_GameEvent] PRIMARY KEY CLUSTERED 
(
	[GameEventID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[GameEvent]  WITH CHECK ADD  CONSTRAINT [FK_GameEvent_Game1] FOREIGN KEY([GameID])
REFERENCES [dbo].[Game] ([GameIdentID])
GO

ALTER TABLE [dbo].[GameEvent] CHECK CONSTRAINT [FK_GameEvent_Game1]
GO




/****** Object:  Table [dbo].[GameChatLog]    Script Date: 09/07/2014 21:29:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[GameChatLog](
	[GameChatLogIdentID] [int] IDENTITY(1,1) NOT NULL,
	[GameID] [int] NOT NULL,
	[GameChatTime] [datetime] NOT NULL,
	[GameChatSpeakerName] [varchar](50) NOT NULL,
	[GameChatTargetName] [varchar](50) NOT NULL,
	[GameChatText] [varchar](2064) NOT NULL,
 CONSTRAINT [PK_GameChatLog] PRIMARY KEY CLUSTERED 
(
	[GameChatLogIdentID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[GameChatLog]  WITH CHECK ADD  CONSTRAINT [FK_GameChatLog_Game] FOREIGN KEY([GameID])
REFERENCES [dbo].[Game] ([GameIdentID])
GO

ALTER TABLE [dbo].[GameChatLog] CHECK CONSTRAINT [FK_GameChatLog_Game]
GO

