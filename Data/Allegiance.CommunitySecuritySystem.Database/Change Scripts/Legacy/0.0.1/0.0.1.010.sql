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
ALTER TABLE dbo.GroupMessage
	DROP CONSTRAINT FK_GroupMessage_Group
GO
ALTER TABLE dbo.[Group] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.GroupMessage
	DROP CONSTRAINT DF_GroupMessage_DateCreated
GO
ALTER TABLE dbo.GroupMessage
	DROP CONSTRAINT DF_GroupMessage_DateToSend
GO
CREATE TABLE dbo.Tmp_GroupMessage
	(
	Id int NOT NULL IDENTITY (1, 1),
	Subject nvarchar(50) NOT NULL,
	Message nvarchar(255) NOT NULL,
	GroupId int NOT NULL,
	DateCreated datetime NOT NULL,
	DateToSend datetime NOT NULL,
	DateExpires datetime NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_GroupMessage SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_GroupMessage ADD CONSTRAINT
	DF_GroupMessage_DateCreated DEFAULT (getdate()) FOR DateCreated
GO
ALTER TABLE dbo.Tmp_GroupMessage ADD CONSTRAINT
	DF_GroupMessage_DateToSend DEFAULT (getdate()) FOR DateToSend
GO
SET IDENTITY_INSERT dbo.Tmp_GroupMessage ON
GO
IF EXISTS(SELECT * FROM dbo.GroupMessage)
	 EXEC('INSERT INTO dbo.Tmp_GroupMessage (Id, Message, GroupId, DateCreated, DateToSend, DateExpires)
		SELECT Id, Message, GroupId, DateCreated, DateToSend, DateExpires FROM dbo.GroupMessage WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_GroupMessage OFF
GO
ALTER TABLE dbo.GroupMessage_Alias
	DROP CONSTRAINT FK_GroupMessage_Alias_GroupMessage
GO
DROP TABLE dbo.GroupMessage
GO
EXECUTE sp_rename N'dbo.Tmp_GroupMessage', N'GroupMessage', 'OBJECT' 
GO
ALTER TABLE dbo.GroupMessage ADD CONSTRAINT
	PK_GroupMessage PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

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
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.GroupMessage_Alias ADD CONSTRAINT
	FK_GroupMessage_Alias_GroupMessage FOREIGN KEY
	(
	GroupMessageId
	) REFERENCES dbo.GroupMessage
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.GroupMessage_Alias SET (LOCK_ESCALATION = TABLE)
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
ALTER TABLE dbo.PersonalMessage
	DROP CONSTRAINT FK_PersonalMessage_Login
GO
ALTER TABLE dbo.Login SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.PersonalMessage
	DROP CONSTRAINT DF_PersonalMessage_DateCreated
GO
ALTER TABLE dbo.PersonalMessage
	DROP CONSTRAINT DF_PersonalMessage_DateToSend
GO
CREATE TABLE dbo.Tmp_PersonalMessage
	(
	Id int NOT NULL IDENTITY (1, 1),
	Subject nvarchar(50) NOT NULL,
	Message nvarchar(255) NOT NULL,
	DateCreated datetime NOT NULL,
	DateToSend datetime NOT NULL,
	DateExpires datetime NULL,
	LoginId int NOT NULL,
	DateViewed datetime NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_PersonalMessage SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_PersonalMessage ADD CONSTRAINT
	DF_PersonalMessage_DateCreated DEFAULT (getdate()) FOR DateCreated
GO
ALTER TABLE dbo.Tmp_PersonalMessage ADD CONSTRAINT
	DF_PersonalMessage_DateToSend DEFAULT (getdate()) FOR DateToSend
GO
SET IDENTITY_INSERT dbo.Tmp_PersonalMessage ON
GO
IF EXISTS(SELECT * FROM dbo.PersonalMessage)
	 EXEC('INSERT INTO dbo.Tmp_PersonalMessage (Id, Message, DateCreated, DateToSend, DateExpires, LoginId, DateViewed)
		SELECT Id, Message, DateCreated, DateToSend, DateExpires, LoginId, DateViewed FROM dbo.PersonalMessage WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_PersonalMessage OFF
GO
DROP TABLE dbo.PersonalMessage
GO
EXECUTE sp_rename N'dbo.Tmp_PersonalMessage', N'PersonalMessage', 'OBJECT' 
GO
ALTER TABLE dbo.PersonalMessage ADD CONSTRAINT
	PK_PersonalMessage PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.PersonalMessage ADD CONSTRAINT
	FK_PersonalMessage_Login FOREIGN KEY
	(
	LoginId
	) REFERENCES dbo.Login
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
COMMIT
