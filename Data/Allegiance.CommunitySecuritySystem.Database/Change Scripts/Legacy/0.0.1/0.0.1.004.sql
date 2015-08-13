/****** Object:  Table [dbo].[Lobby]    Script Date: 08/30/2009 13:04:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Lobby]') AND type in (N'U'))
DROP TABLE [dbo].[Lobby]
GO
/****** Object:  Table [dbo].[Lobby]    Script Date: 08/30/2009 13:04:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Lobby]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Lobby](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Host] [nvarchar](50) NOT NULL,
	[BasePath] [nvarchar](100) NOT NULL,
	[IsRestrictive] [bit] NOT NULL,
	[IsEnabled] [bit] NOT NULL,
 CONSTRAINT [PK_Lobby] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Lobby_IsRestrictive]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Lobby] ADD  CONSTRAINT [DF_Lobby_IsRestrictive]  DEFAULT ((0)) FOR [IsRestrictive]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Lobby_IsEnabled]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Lobby] ADD  CONSTRAINT [DF_Lobby_IsEnabled]  DEFAULT ((1)) FOR [IsEnabled]
END

GO

 /****** Object:  Table [dbo].[Lobby_Login]    Script Date: 08/29/2009 11:19:42 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Lobby_Login]') AND type in (N'U'))
DROP TABLE [dbo].[Lobby_Login]
GO
/****** Object:  Table [dbo].[Lobby_Login]    Script Date: 08/29/2009 11:19:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Lobby_Login]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Lobby_Login](
	[LobbyId] [int] NOT NULL,
	[LoginId] [int] NOT NULL,
 CONSTRAINT [PK_Lobby_Login] PRIMARY KEY CLUSTERED 
(
	[LobbyId] ASC,
	[LoginId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Lobby_Login_Lobby]') AND parent_object_id = OBJECT_ID(N'[dbo].[Lobby_Login]'))
ALTER TABLE [dbo].[Lobby_Login]  WITH CHECK ADD  CONSTRAINT [FK_Lobby_Login_Lobby] FOREIGN KEY([LobbyId])
REFERENCES [dbo].[Lobby] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Lobby_Login_Lobby]') AND parent_object_id = OBJECT_ID(N'[dbo].[Lobby_Login]'))
ALTER TABLE [dbo].[Lobby_Login] CHECK CONSTRAINT [FK_Lobby_Login_Lobby]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Lobby_Login_Login]') AND parent_object_id = OBJECT_ID(N'[dbo].[Lobby_Login]'))
ALTER TABLE [dbo].[Lobby_Login]  WITH CHECK ADD  CONSTRAINT [FK_Lobby_Login_Login] FOREIGN KEY([LoginId])
REFERENCES [dbo].[Login] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Lobby_Login_Login]') AND parent_object_id = OBJECT_ID(N'[dbo].[Lobby_Login]'))
ALTER TABLE [dbo].[Lobby_Login] CHECK CONSTRAINT [FK_Lobby_Login_Login]
GO

/****** Object:  Table [dbo].[AutoUpdateFile]    Script Date: 08/30/2009 13:04:59 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AutoUpdateFile]') AND type in (N'U'))
DROP TABLE [dbo].[AutoUpdateFile]
GO
/****** Object:  Table [dbo].[AutoUpdateFile]    Script Date: 08/30/2009 13:04:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AutoUpdateFile]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AutoUpdateFile](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Filename] [nvarchar](100) NOT NULL,
	[IsProtected] [bit] NOT NULL,
 CONSTRAINT [PK_AutoUpdateFile] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_AutoUpdateFile_IsProtected]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[AutoUpdateFile] ADD  CONSTRAINT [DF_AutoUpdateFile_IsProtected]  DEFAULT ((0)) FOR [IsProtected]
END

GO

/****** Object:  Table [dbo].[AutoUpdateFile_Lobby]    Script Date: 08/30/2009 13:05:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AutoUpdateFile_Lobby]') AND type in (N'U'))
DROP TABLE [dbo].[AutoUpdateFile_Lobby]
GO
/****** Object:  Table [dbo].[AutoUpdateFile_Lobby]    Script Date: 08/30/2009 13:05:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AutoUpdateFile_Lobby]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AutoUpdateFile_Lobby](
	[AutoUpdateFileId] [int] NOT NULL,
	[LobbyId] [int] NULL,
	[ValidChecksum] [nchar](20) NULL,
	[CurrentVersion] [nvarchar](15) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL
) ON [PRIMARY]
END
GO

/****** Object:  Index [IX_AutoUpdateFile_Lobby]    Script Date: 08/30/2009 13:05:01 ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[AutoUpdateFile_Lobby]') AND name = N'IX_AutoUpdateFile_Lobby')
CREATE CLUSTERED INDEX [IX_AutoUpdateFile_Lobby] ON [dbo].[AutoUpdateFile_Lobby] 
(
	[AutoUpdateFileId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

/****** Object:  Index [IX_AutoUpdateFile_Lobby_1]    Script Date: 08/30/2009 13:05:01 ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[AutoUpdateFile_Lobby]') AND name = N'IX_AutoUpdateFile_Lobby_1')
CREATE NONCLUSTERED INDEX [IX_AutoUpdateFile_Lobby_1] ON [dbo].[AutoUpdateFile_Lobby] 
(
	[LobbyId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AutoUpdateFile_Lobby_AutoUpdateFile]') AND parent_object_id = OBJECT_ID(N'[dbo].[AutoUpdateFile_Lobby]'))
ALTER TABLE [dbo].[AutoUpdateFile_Lobby]  WITH CHECK ADD  CONSTRAINT [FK_AutoUpdateFile_Lobby_AutoUpdateFile] FOREIGN KEY([AutoUpdateFileId])
REFERENCES [dbo].[AutoUpdateFile] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AutoUpdateFile_Lobby_AutoUpdateFile]') AND parent_object_id = OBJECT_ID(N'[dbo].[AutoUpdateFile_Lobby]'))
ALTER TABLE [dbo].[AutoUpdateFile_Lobby] CHECK CONSTRAINT [FK_AutoUpdateFile_Lobby_AutoUpdateFile]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AutoUpdateFile_Lobby_Lobby]') AND parent_object_id = OBJECT_ID(N'[dbo].[AutoUpdateFile_Lobby]'))
ALTER TABLE [dbo].[AutoUpdateFile_Lobby]  WITH CHECK ADD  CONSTRAINT [FK_AutoUpdateFile_Lobby_Lobby] FOREIGN KEY([LobbyId])
REFERENCES [dbo].[Lobby] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AutoUpdateFile_Lobby_Lobby]') AND parent_object_id = OBJECT_ID(N'[dbo].[AutoUpdateFile_Lobby]'))
ALTER TABLE [dbo].[AutoUpdateFile_Lobby] CHECK CONSTRAINT [FK_AutoUpdateFile_Lobby_Lobby]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_AutoUpdateFile_Lobby_DateCreated]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[AutoUpdateFile_Lobby] ADD  CONSTRAINT [DF_AutoUpdateFile_Lobby_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_AutoUpdateFile_Lobby_DateModified]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[AutoUpdateFile_Lobby] ADD  CONSTRAINT [DF_AutoUpdateFile_Lobby_DateModified]  DEFAULT (getdate()) FOR [DateModified]
END

GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION FindAutoUpdateFiles 
(	
	@LobbyId int
)
RETURNS TABLE 
AS
RETURN 
(

	SELECT AL.*, AO.Filename, AO.IsProtected
	FROM AutoUpdateFile AO
		INNER JOIN AutoUpdateFile_Lobby AL ON AL.AutoUpdateFileId = AO.Id
	INNER JOIN 
	(
		SELECT MAX(AL.LobbyId) as LobbyId, AF.[Filename]
		FROM AutoUpdateFile AF
			LEFT JOIN AutoUpdateFile_Lobby AL ON AL.AutoUpdateFileId = AF.Id
		WHERE LobbyId IS NULL OR LobbyId = @LobbyId
		GROUP BY [Filename]
	) AD ON AD.[Filename] = AO.[Filename] AND (AL.LobbyId = AD.Lobbyid OR AD.Lobbyid IS NULL)

)
