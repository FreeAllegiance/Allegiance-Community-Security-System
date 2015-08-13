USE CSSStats
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
ALTER TABLE dbo.GameTeamMember ADD
	Score int NOT NULL CONSTRAINT DF_GameTeamMember_Score DEFAULT 0
GO
ALTER TABLE dbo.GameTeamMember SET (LOCK_ESCALATION = TABLE)
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
ALTER TABLE dbo.StatsLeaderboard ADD
	Xp int NOT NULL CONSTRAINT DF_StatsLeaderboard_Xp DEFAULT 0
GO
ALTER TABLE dbo.StatsLeaderboard SET (LOCK_ESCALATION = TABLE)
GO
COMMIT


/****** Object:  Table [dbo].[WinFactor]    Script Date: 02/10/2013 15:35:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WinFactor](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MinLevel] [int] NOT NULL,
	[MaxLevel] [int] NOT NULL,
	[Factor] [int] NOT NULL,
 CONSTRAINT [PK_WinFactor] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[WinFactor] ON
INSERT [dbo].[WinFactor] ([ID], [MinLevel], [MaxLevel], [Factor]) VALUES (1, 0, 40, 100)
INSERT [dbo].[WinFactor] ([ID], [MinLevel], [MaxLevel], [Factor]) VALUES (2, 41, 41, 95)
INSERT [dbo].[WinFactor] ([ID], [MinLevel], [MaxLevel], [Factor]) VALUES (3, 42, 42, 90)
INSERT [dbo].[WinFactor] ([ID], [MinLevel], [MaxLevel], [Factor]) VALUES (4, 43, 43, 85)
INSERT [dbo].[WinFactor] ([ID], [MinLevel], [MaxLevel], [Factor]) VALUES (5, 44, 44, 80)
INSERT [dbo].[WinFactor] ([ID], [MinLevel], [MaxLevel], [Factor]) VALUES (6, 45, 45, 70)
INSERT [dbo].[WinFactor] ([ID], [MinLevel], [MaxLevel], [Factor]) VALUES (7, 46, 46, 65)
INSERT [dbo].[WinFactor] ([ID], [MinLevel], [MaxLevel], [Factor]) VALUES (8, 47, 47, 60)
INSERT [dbo].[WinFactor] ([ID], [MinLevel], [MaxLevel], [Factor]) VALUES (9, 48, 48, 55)
INSERT [dbo].[WinFactor] ([ID], [MinLevel], [MaxLevel], [Factor]) VALUES (10, 49, 9999, 50)
SET IDENTITY_INSERT [dbo].[WinFactor] OFF
/****** Object:  Table [dbo].[LossFactor]    Script Date: 02/10/2013 15:35:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LossFactor](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MinLevel] [int] NULL,
	[MaxLevel] [int] NULL,
	[Factor] [int] NULL,
 CONSTRAINT [PK_LossFactor] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[LossFactor] ON
INSERT [dbo].[LossFactor] ([ID], [MinLevel], [MaxLevel], [Factor]) VALUES (1, 0, 0, 0)
INSERT [dbo].[LossFactor] ([ID], [MinLevel], [MaxLevel], [Factor]) VALUES (2, 1, 1, 5)
INSERT [dbo].[LossFactor] ([ID], [MinLevel], [MaxLevel], [Factor]) VALUES (3, 2, 2, 10)
INSERT [dbo].[LossFactor] ([ID], [MinLevel], [MaxLevel], [Factor]) VALUES (4, 3, 3, 20)
INSERT [dbo].[LossFactor] ([ID], [MinLevel], [MaxLevel], [Factor]) VALUES (5, 4, 4, 35)
INSERT [dbo].[LossFactor] ([ID], [MinLevel], [MaxLevel], [Factor]) VALUES (6, 5, 5, 45)
INSERT [dbo].[LossFactor] ([ID], [MinLevel], [MaxLevel], [Factor]) VALUES (7, 6, 6, 55)
INSERT [dbo].[LossFactor] ([ID], [MinLevel], [MaxLevel], [Factor]) VALUES (8, 7, 7, 60)
INSERT [dbo].[LossFactor] ([ID], [MinLevel], [MaxLevel], [Factor]) VALUES (9, 8, 8, 65)
INSERT [dbo].[LossFactor] ([ID], [MinLevel], [MaxLevel], [Factor]) VALUES (10, 9, 9, 70)
INSERT [dbo].[LossFactor] ([ID], [MinLevel], [MaxLevel], [Factor]) VALUES (11, 10, 10, 75)
INSERT [dbo].[LossFactor] ([ID], [MinLevel], [MaxLevel], [Factor]) VALUES (12, 11, 11, 80)
INSERT [dbo].[LossFactor] ([ID], [MinLevel], [MaxLevel], [Factor]) VALUES (13, 12, 12, 85)
INSERT [dbo].[LossFactor] ([ID], [MinLevel], [MaxLevel], [Factor]) VALUES (14, 13, 13, 90)
INSERT [dbo].[LossFactor] ([ID], [MinLevel], [MaxLevel], [Factor]) VALUES (15, 14, 14, 95)
INSERT [dbo].[LossFactor] ([ID], [MinLevel], [MaxLevel], [Factor]) VALUES (16, 15, 9999, 100)
SET IDENTITY_INSERT [dbo].[LossFactor] OFF
/****** Object:  Table [dbo].[Level]    Script Date: 02/10/2013 15:35:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Level](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Level] [int] NOT NULL,
	[MinXP] [int] NOT NULL,
	[MaxXP] [int] NOT NULL,
	[ImageUrl] [nvarchar](255) NULL,
	[Caption] [nvarchar](50) NULL
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Level] ON
INSERT [dbo].[Level] ([ID], [Level], [MinXP], [MaxXP], [ImageUrl], [Caption]) VALUES (1, 1, 0, 99, null, N'Newbie')
INSERT [dbo].[Level] ([ID], [Level], [MinXP], [MaxXP], [ImageUrl], [Caption]) VALUES (2, 2, 100, 199, null, N'Novice 1')
INSERT [dbo].[Level] ([ID], [Level], [MinXP], [MaxXP], [ImageUrl], [Caption]) VALUES (3, 3, 200, 399, null, N'Novice 2')
INSERT [dbo].[Level] ([ID], [Level], [MinXP], [MaxXP], [ImageUrl], [Caption]) VALUES (4, 4, 400, 599, null, N'Novice 3')
INSERT [dbo].[Level] ([ID], [Level], [MinXP], [MaxXP], [ImageUrl], [Caption]) VALUES (5, 5, 600, 899, null, N'Novice 4')
INSERT [dbo].[Level] ([ID], [Level], [MinXP], [MaxXP], [ImageUrl], [Caption]) VALUES (6, 6, 900, 1199, null, N'Novice 5')
INSERT [dbo].[Level] ([ID], [Level], [MinXP], [MaxXP], [ImageUrl], [Caption]) VALUES (7, 7, 1200, 1599, null, N'Novice 6')
INSERT [dbo].[Level] ([ID], [Level], [MinXP], [MaxXP], [ImageUrl], [Caption]) VALUES (8, 8, 1600, 1999, null, N'Novice 7')
INSERT [dbo].[Level] ([ID], [Level], [MinXP], [MaxXP], [ImageUrl], [Caption]) VALUES (9, 9, 2000, 2499, null, N'Inter. 1')
INSERT [dbo].[Level] ([ID], [Level], [MinXP], [MaxXP], [ImageUrl], [Caption]) VALUES (10, 10, 2500, 2999, null, N'Inter. 2')
INSERT [dbo].[Level] ([ID], [Level], [MinXP], [MaxXP], [ImageUrl], [Caption]) VALUES (11, 11, 3000, 3499, null, N'Inter. 3')
INSERT [dbo].[Level] ([ID], [Level], [MinXP], [MaxXP], [ImageUrl], [Caption]) VALUES (12, 12, 3500, 3999, null, N'Inter. 4')
INSERT [dbo].[Level] ([ID], [Level], [MinXP], [MaxXP], [ImageUrl], [Caption]) VALUES (13, 13, 4000, 4499, null, N'Inter. 5')
INSERT [dbo].[Level] ([ID], [Level], [MinXP], [MaxXP], [ImageUrl], [Caption]) VALUES (14, 14, 4500, 5499, null, N'Inter. 6')
INSERT [dbo].[Level] ([ID], [Level], [MinXP], [MaxXP], [ImageUrl], [Caption]) VALUES (15, 15, 5500, 6499, null, N'Inter. 7')
INSERT [dbo].[Level] ([ID], [Level], [MinXP], [MaxXP], [ImageUrl], [Caption]) VALUES (16, 16, 6500, 8999, null, N'Veteran 1')
INSERT [dbo].[Level] ([ID], [Level], [MinXP], [MaxXP], [ImageUrl], [Caption]) VALUES (17, 17, 9000, 10499, null, N'Veteran 2')
INSERT [dbo].[Level] ([ID], [Level], [MinXP], [MaxXP], [ImageUrl], [Caption]) VALUES (18, 18, 10500, 11999, null, N'Veteran 3')
INSERT [dbo].[Level] ([ID], [Level], [MinXP], [MaxXP], [ImageUrl], [Caption]) VALUES (19, 19, 12000, 13499, null, N'Veteran 4')
INSERT [dbo].[Level] ([ID], [Level], [MinXP], [MaxXP], [ImageUrl], [Caption]) VALUES (20, 20, 13500, 14999, null, N'Veteran 5')
INSERT [dbo].[Level] ([ID], [Level], [MinXP], [MaxXP], [ImageUrl], [Caption]) VALUES (21, 21, 15000, 16499, null, N'Veteran 6')
INSERT [dbo].[Level] ([ID], [Level], [MinXP], [MaxXP], [ImageUrl], [Caption]) VALUES (22, 22, 16500, 17999, null, N'Veteran 7')
INSERT [dbo].[Level] ([ID], [Level], [MinXP], [MaxXP], [ImageUrl], [Caption]) VALUES (23, 23, 18000, 18899, null, N'Expert 1')
INSERT [dbo].[Level] ([ID], [Level], [MinXP], [MaxXP], [ImageUrl], [Caption]) VALUES (24, 24, 18900, 19999, null, N'Expert 2')
INSERT [dbo].[Level] ([ID], [Level], [MinXP], [MaxXP], [ImageUrl], [Caption]) VALUES (25, 25, 20000, 21499, null, N'Expert 3')
INSERT [dbo].[Level] ([ID], [Level], [MinXP], [MaxXP], [ImageUrl], [Caption]) VALUES (26, 26, 21500, 23499, null, N'Expert 4')
INSERT [dbo].[Level] ([ID], [Level], [MinXP], [MaxXP], [ImageUrl], [Caption]) VALUES (27, 27, 23500, 33499, null, N'Expert 5')
INSERT [dbo].[Level] ([ID], [Level], [MinXP], [MaxXP], [ImageUrl], [Caption]) VALUES (28, 28, 33500, 44499, null, N'Expert 6')
INSERT [dbo].[Level] ([ID], [Level], [MinXP], [MaxXP], [ImageUrl], [Caption]) VALUES (29, 29, 44500, 99999999, null, N'Expert 7')
SET IDENTITY_INSERT [dbo].[Level] OFF
/****** Object:  Table [dbo].[ExperianceExchange]    Script Date: 02/10/2013 15:35:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExperianceExchange](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LevelDiffMin] [int] NOT NULL,
	[LevelDiffMax] [int] NOT NULL,
	[HigherWin] [int] NOT NULL,
	[HigherLoss] [int] NOT NULL,
	[LowerWin] [int] NOT NULL,
	[LowerLoss] [int] NOT NULL,
 CONSTRAINT [PK_ExperianceExchange] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[ExperianceExchange] ON
INSERT [dbo].[ExperianceExchange] ([ID], [LevelDiffMin], [LevelDiffMax], [HigherWin], [HigherLoss], [LowerWin], [LowerLoss]) VALUES (1, 0, 0, 100, 100, 110, 100)
INSERT [dbo].[ExperianceExchange] ([ID], [LevelDiffMin], [LevelDiffMax], [HigherWin], [HigherLoss], [LowerWin], [LowerLoss]) VALUES (2, 1, 1, 90, 110, 110, 90)
INSERT [dbo].[ExperianceExchange] ([ID], [LevelDiffMin], [LevelDiffMax], [HigherWin], [HigherLoss], [LowerWin], [LowerLoss]) VALUES (3, 2, 2, 80, 120, 120, 80)
INSERT [dbo].[ExperianceExchange] ([ID], [LevelDiffMin], [LevelDiffMax], [HigherWin], [HigherLoss], [LowerWin], [LowerLoss]) VALUES (4, 3, 3, 70, 130, 130, 70)
INSERT [dbo].[ExperianceExchange] ([ID], [LevelDiffMin], [LevelDiffMax], [HigherWin], [HigherLoss], [LowerWin], [LowerLoss]) VALUES (5, 4, 4, 60, 140, 140, 60)
INSERT [dbo].[ExperianceExchange] ([ID], [LevelDiffMin], [LevelDiffMax], [HigherWin], [HigherLoss], [LowerWin], [LowerLoss]) VALUES (6, 5, 5, 50, 150, 150, 50)
INSERT [dbo].[ExperianceExchange] ([ID], [LevelDiffMin], [LevelDiffMax], [HigherWin], [HigherLoss], [LowerWin], [LowerLoss]) VALUES (7, 6, 6, 40, 160, 160, 40)
INSERT [dbo].[ExperianceExchange] ([ID], [LevelDiffMin], [LevelDiffMax], [HigherWin], [HigherLoss], [LowerWin], [LowerLoss]) VALUES (8, 7, 7, 30, 170, 170, 30)
INSERT [dbo].[ExperianceExchange] ([ID], [LevelDiffMin], [LevelDiffMax], [HigherWin], [HigherLoss], [LowerWin], [LowerLoss]) VALUES (9, 8, 9999, 20, 180, 180, 20)
SET IDENTITY_INSERT [dbo].[ExperianceExchange] OFF


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
	PRank int NOT NULL CONSTRAINT DF_StatsLeaderboard_PRank DEFAULT 0
GO
ALTER TABLE dbo.StatsLeaderboard SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
