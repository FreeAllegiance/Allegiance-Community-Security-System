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
ALTER TABLE dbo.Lobby SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.AutoUpdateFile SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.AutoUpdateFile_Lobby
	DROP CONSTRAINT DF_AutoUpdateFile_Lobby_DateCreated
GO
ALTER TABLE dbo.AutoUpdateFile_Lobby
	DROP CONSTRAINT DF_AutoUpdateFile_Lobby_DateModified
GO
CREATE TABLE dbo.Tmp_AutoUpdateFile_Lobby
	(
	AutoUpdateFileId int NOT NULL,
	LobbyId int NOT NULL,
	ValidChecksum nchar(20) NULL,
	CurrentVersion nvarchar(15) NOT NULL,
	DateCreated datetime NOT NULL,
	DateModified datetime NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_AutoUpdateFile_Lobby SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_AutoUpdateFile_Lobby ADD CONSTRAINT
	DF_AutoUpdateFile_Lobby_DateCreated DEFAULT (getdate()) FOR DateCreated
GO
ALTER TABLE dbo.Tmp_AutoUpdateFile_Lobby ADD CONSTRAINT
	DF_AutoUpdateFile_Lobby_DateModified DEFAULT (getdate()) FOR DateModified
GO
IF EXISTS(SELECT * FROM dbo.AutoUpdateFile_Lobby)
	 EXEC('INSERT INTO dbo.Tmp_AutoUpdateFile_Lobby (AutoUpdateFileId, LobbyId, ValidChecksum, CurrentVersion, DateCreated, DateModified)
		SELECT AutoUpdateFileId, LobbyId, ValidChecksum, CurrentVersion, DateCreated, DateModified FROM dbo.AutoUpdateFile_Lobby WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.AutoUpdateFile_Lobby
GO
EXECUTE sp_rename N'dbo.Tmp_AutoUpdateFile_Lobby', N'AutoUpdateFile_Lobby', 'OBJECT' 
GO
ALTER TABLE dbo.AutoUpdateFile_Lobby ADD CONSTRAINT
	PK_AutoUpdateFile_Lobby PRIMARY KEY NONCLUSTERED 
	(
	AutoUpdateFileId,
	LobbyId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE CLUSTERED INDEX IX_AutoUpdateFile_Lobby ON dbo.AutoUpdateFile_Lobby
	(
	AutoUpdateFileId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_AutoUpdateFile_Lobby_1 ON dbo.AutoUpdateFile_Lobby
	(
	LobbyId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE dbo.AutoUpdateFile_Lobby ADD CONSTRAINT
	FK_AutoUpdateFile_Lobby_AutoUpdateFile FOREIGN KEY
	(
	AutoUpdateFileId
	) REFERENCES dbo.AutoUpdateFile
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.AutoUpdateFile_Lobby ADD CONSTRAINT
	FK_AutoUpdateFile_Lobby_Lobby FOREIGN KEY
	(
	LobbyId
	) REFERENCES dbo.Lobby
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
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
ALTER TABLE dbo.Lobby
	DROP CONSTRAINT DF_Lobby_IsRestrictive
GO
ALTER TABLE dbo.Lobby
	DROP CONSTRAINT DF_Lobby_IsEnabled
GO
CREATE TABLE dbo.Tmp_Lobby
	(
	Id int NOT NULL IDENTITY (1, 1),
	Name nvarchar(50) NOT NULL,
	Host nvarchar(50) NOT NULL,
	BasePath nvarchar(255) NOT NULL,
	IsRestrictive bit NOT NULL,
	IsEnabled bit NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Lobby SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_Lobby ADD CONSTRAINT
	DF_Lobby_IsRestrictive DEFAULT ((0)) FOR IsRestrictive
GO
ALTER TABLE dbo.Tmp_Lobby ADD CONSTRAINT
	DF_Lobby_IsEnabled DEFAULT ((1)) FOR IsEnabled
GO
SET IDENTITY_INSERT dbo.Tmp_Lobby ON
GO
IF EXISTS(SELECT * FROM dbo.Lobby)
	 EXEC('INSERT INTO dbo.Tmp_Lobby (Id, Name, Host, BasePath, IsRestrictive, IsEnabled)
		SELECT Id, Name, Host, BasePath, IsRestrictive, IsEnabled FROM dbo.Lobby WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Lobby OFF
GO
ALTER TABLE dbo.AutoUpdateFile_Lobby
	DROP CONSTRAINT FK_AutoUpdateFile_Lobby_Lobby
GO
ALTER TABLE dbo.Lobby_Login
	DROP CONSTRAINT FK_Lobby_Login_Lobby
GO
DROP TABLE dbo.Lobby
GO
EXECUTE sp_rename N'dbo.Tmp_Lobby', N'Lobby', 'OBJECT' 
GO
ALTER TABLE dbo.Lobby ADD CONSTRAINT
	PK_Lobby PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Lobby_Login ADD CONSTRAINT
	FK_Lobby_Login_Lobby FOREIGN KEY
	(
	LobbyId
	) REFERENCES dbo.Lobby
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Lobby_Login SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.AutoUpdateFile_Lobby ADD CONSTRAINT
	FK_AutoUpdateFile_Lobby_Lobby FOREIGN KEY
	(
	LobbyId
	) REFERENCES dbo.Lobby
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.AutoUpdateFile_Lobby SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

/****** Object:  UserDefinedFunction [dbo].[FindAutoUpdateFiles]    Script Date: 09/04/2009 20:15:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER FUNCTION [dbo].[FindAutoUpdateFiles] 
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
		WHERE LobbyId = 0 OR LobbyId = @LobbyId
		GROUP BY [Filename]
	) AD ON AD.[Filename] = AO.[Filename] AND (AL.LobbyId = AD.Lobbyid OR AD.Lobbyid = 0)

)

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
ALTER TABLE dbo.AutoUpdateFile_Lobby
	DROP CONSTRAINT FK_AutoUpdateFile_Lobby_Lobby
GO
ALTER TABLE dbo.Lobby SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.AutoUpdateFile_Lobby
	DROP CONSTRAINT FK_AutoUpdateFile_Lobby_AutoUpdateFile
GO
ALTER TABLE dbo.AutoUpdateFile SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.AutoUpdateFile_Lobby
	DROP CONSTRAINT DF_AutoUpdateFile_Lobby_DateCreated
GO
ALTER TABLE dbo.AutoUpdateFile_Lobby
	DROP CONSTRAINT DF_AutoUpdateFile_Lobby_DateModified
GO
CREATE TABLE dbo.Tmp_AutoUpdateFile_Lobby
	(
	AutoUpdateFileId int NOT NULL,
	LobbyId int NOT NULL,
	ValidChecksum nchar(28) NULL,
	CurrentVersion nvarchar(15) NOT NULL,
	DateCreated datetime NOT NULL,
	DateModified datetime NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_AutoUpdateFile_Lobby SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_AutoUpdateFile_Lobby ADD CONSTRAINT
	DF_AutoUpdateFile_Lobby_DateCreated DEFAULT (getdate()) FOR DateCreated
GO
ALTER TABLE dbo.Tmp_AutoUpdateFile_Lobby ADD CONSTRAINT
	DF_AutoUpdateFile_Lobby_DateModified DEFAULT (getdate()) FOR DateModified
GO
IF EXISTS(SELECT * FROM dbo.AutoUpdateFile_Lobby)
	 EXEC('INSERT INTO dbo.Tmp_AutoUpdateFile_Lobby (AutoUpdateFileId, LobbyId, ValidChecksum, CurrentVersion, DateCreated, DateModified)
		SELECT AutoUpdateFileId, LobbyId, ValidChecksum, CurrentVersion, DateCreated, DateModified FROM dbo.AutoUpdateFile_Lobby WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.AutoUpdateFile_Lobby
GO
EXECUTE sp_rename N'dbo.Tmp_AutoUpdateFile_Lobby', N'AutoUpdateFile_Lobby', 'OBJECT' 
GO
ALTER TABLE dbo.AutoUpdateFile_Lobby ADD CONSTRAINT
	PK_AutoUpdateFile_Lobby PRIMARY KEY NONCLUSTERED 
	(
	AutoUpdateFileId,
	LobbyId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE CLUSTERED INDEX IX_AutoUpdateFile_Lobby ON dbo.AutoUpdateFile_Lobby
	(
	AutoUpdateFileId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_AutoUpdateFile_Lobby_1 ON dbo.AutoUpdateFile_Lobby
	(
	LobbyId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE dbo.AutoUpdateFile_Lobby ADD CONSTRAINT
	FK_AutoUpdateFile_Lobby_AutoUpdateFile FOREIGN KEY
	(
	AutoUpdateFileId
	) REFERENCES dbo.AutoUpdateFile
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.AutoUpdateFile_Lobby ADD CONSTRAINT
	FK_AutoUpdateFile_Lobby_Lobby FOREIGN KEY
	(
	LobbyId
	) REFERENCES dbo.Lobby
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
COMMIT
