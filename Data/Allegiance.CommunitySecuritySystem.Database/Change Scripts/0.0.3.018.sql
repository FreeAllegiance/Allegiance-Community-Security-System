USE [CSSStats]
GO



/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.StatsLeaderboard ADD
	Description nvarchar(2064) NULL
GO
ALTER TABLE dbo.StatsLeaderboard SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

USE [CSS]
GO

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.GroupRole ADD
	AllegianceRoleID int NOT NULL CONSTRAINT DF_GroupRole_AllegianceRoleID DEFAULT 0
GO
ALTER TABLE dbo.GroupRole SET (LOCK_ESCALATION = TABLE)
GO
COMMIT


update GroupRole SET AllegianceRoleID = 7 where Id = 1
update GroupRole SET AllegianceRoleID = 6 where Id = 2
update GroupRole SET AllegianceRoleID = 5 where Id = 6

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.[Group] ADD
	Description nvarchar(510) NULL
GO
ALTER TABLE dbo.[Group] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.[Group] ADD
	URL nvarchar(255) NULL
GO
ALTER TABLE dbo.[Group] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT


  INSERT INTO GroupRequestType (Id, Name)
  VALUES(1, 'JoinSquad')


  insert into CSS..GroupRequestType VALUES(2, 'Rejected')


USE [CSS]
GO

update [Group] set Description = '', URL = '' where Description IS NULL

  /* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.[Group]
	DROP CONSTRAINT DF_Group_IsSquad
GO
ALTER TABLE dbo.[Group]
	DROP CONSTRAINT DF_Group_DateCreated
GO
CREATE TABLE dbo.Tmp_Group
	(
	Id int NOT NULL IDENTITY (1, 1),
	Name nvarchar(25) NOT NULL,
	Tag nvarchar(5) NOT NULL,
	IsSquad bit NOT NULL,
	DateCreated datetime NOT NULL,
	Description nvarchar(510) NOT NULL,
	URL nvarchar(255) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Group SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_Group ADD CONSTRAINT
	DF_Group_IsSquad DEFAULT ((0)) FOR IsSquad
GO
ALTER TABLE dbo.Tmp_Group ADD CONSTRAINT
	DF_Group_DateCreated DEFAULT (getdate()) FOR DateCreated
GO
ALTER TABLE dbo.Tmp_Group ADD CONSTRAINT
	DF_Group_Description DEFAULT '' FOR Description
GO
ALTER TABLE dbo.Tmp_Group ADD CONSTRAINT
	DF_Group_URL DEFAULT '' FOR URL
GO
SET IDENTITY_INSERT dbo.Tmp_Group ON
GO
IF EXISTS(SELECT * FROM dbo.[Group])
	 EXEC('INSERT INTO dbo.Tmp_Group (Id, Name, Tag, IsSquad, DateCreated, Description, URL)
		SELECT Id, Name, Tag, IsSquad, DateCreated, Description, URL FROM dbo.[Group] WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Group OFF
GO
ALTER TABLE dbo.GroupMessage
	DROP CONSTRAINT FK_GroupMessage_Group
GO
ALTER TABLE dbo.Group_Alias_GroupRole
	DROP CONSTRAINT FK_Group_Alias_GroupRole_Group
GO
DROP TABLE dbo.[Group]
GO
EXECUTE sp_rename N'dbo.Tmp_Group', N'Group', 'OBJECT' 
GO
ALTER TABLE dbo.[Group] ADD CONSTRAINT
	PK_Group PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Group_Alias_GroupRole ADD CONSTRAINT
	FK_Group_Alias_GroupRole_Group FOREIGN KEY
	(
	GroupId
	) REFERENCES dbo.[Group]
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Group_Alias_GroupRole SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.GroupMessage ADD CONSTRAINT
	FK_GroupMessage_Group FOREIGN KEY
	(
	GroupId
	) REFERENCES dbo.[Group]
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.GroupMessage SET (LOCK_ESCALATION = TABLE)
GO
COMMIT





USE [CSSStats]
GO

/****** Object:  Table [dbo].[ScoreQueue]    Script Date: 08/07/2015 19:28:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ScoreQueue](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[LoginId] [int] NOT NULL,
	[GameGuid] [uniqueidentifier] NOT NULL,
	[Score] [float] NOT NULL,
	[PilotBaseKills] [int] NOT NULL,
	[PilotBaseCaptures] [int] NOT NULL,
	[WarpsSpotted] [float] NOT NULL,
	[AsteroidsSpotted] [float] NOT NULL,
	[MinerKills] [float] NOT NULL,
	[BuilderKills] [float] NOT NULL,
	[LayerKills] [float] NOT NULL,
	[CarrierKills] [float] NOT NULL,
	[PlayerKills] [float] NOT NULL,
	[BaseKills] [float] NOT NULL,
	[BaseCaptures] [float] NOT NULL,
	[TechsRecovered] [float] NOT NULL,
	[Flags] [int] NOT NULL,
	[Artifacts] [int] NOT NULL,
	[Rescues] [int] NOT NULL,
	[Kills] [int] NOT NULL,
	[Assists] [int] NOT NULL,
	[Deaths] [int] NOT NULL,
	[Ejections] [int] NOT NULL,
	[CombatRating] [float] NULL,
	[Win] [bit] NULL,
	[Lose] [bit] NULL,
	[CommandWin] [bit] NULL,
	[CommandLose] [bit] NULL,
	[TimePlayed] [float] NULL,
	[TimeCommanded] [float] NULL,
	[CommandCredit] [bit] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_ScoreQueue] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[ScoreQueue] ADD  CONSTRAINT [DF_ScoreQueue_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO


USE [CSSStats]
GO

/****** Object:  Table [dbo].[Medals]    Script Date: 08/07/2015 19:29:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Medals](
	[MedalID] [int] IDENTITY(1,1) NOT NULL,
	[MedalName] [nvarchar](48) NOT NULL,
	[MedalDescription] [nvarchar](256) NOT NULL,
	[MedalBitmap] [nvarchar](12) NOT NULL,
	[MedalSortOrder] [int] NOT NULL,
 CONSTRAINT [PK_Medals] PRIMARY KEY CLUSTERED 
(
	[MedalID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Medals] ADD  CONSTRAINT [DF_Medals_MedalSortOrder]  DEFAULT ((0)) FOR [MedalSortOrder]
GO


USE [CSSStats]
GO

/****** Object:  Table [dbo].[CharMedals]    Script Date: 08/07/2015 19:29:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CharMedals](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MedalID] [int] NOT NULL,
	[SpecificInfo] [nvarchar](48) NULL,
	[CharacterID] [int] NOT NULL,
	[CivID] [int] NOT NULL,
 CONSTRAINT [PK_CharMedals] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[CharMedals] ADD  CONSTRAINT [DF_CharMedals_CivID]  DEFAULT ((-1)) FOR [CivID]
GO

