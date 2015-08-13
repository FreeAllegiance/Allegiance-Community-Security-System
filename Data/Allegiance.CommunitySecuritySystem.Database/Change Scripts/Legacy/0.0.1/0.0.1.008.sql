﻿BEGIN TRANSACTION
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
ALTER TABLE dbo.PersonalMessage ADD
	DateViewed datetime NULL
GO
ALTER TABLE dbo.PersonalMessage SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

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
ALTER TABLE dbo.GroupMessage_Alias
	DROP CONSTRAINT FK_GroupMessage_Alias_GroupMessage
GO
ALTER TABLE dbo.GroupMessage SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.GroupMessage_Alias
	DROP CONSTRAINT FK_GroupMessage_Alias_Alias
GO
ALTER TABLE dbo.Alias SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.GroupMessage_Alias
	DROP CONSTRAINT DF_GroupMessage_Alias_DateViewed
GO
CREATE TABLE dbo.Tmp_GroupMessage_Alias
	(
	GroupMessageId int NOT NULL,
	AliasId int NOT NULL,
	DateViewed datetime NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_GroupMessage_Alias SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_GroupMessage_Alias ADD CONSTRAINT
	DF_GroupMessage_Alias_DateViewed DEFAULT (getdate()) FOR DateViewed
GO
IF EXISTS(SELECT * FROM dbo.GroupMessage_Alias)
	 EXEC('INSERT INTO dbo.Tmp_GroupMessage_Alias (GroupMessageId, AliasId, DateViewed)
		SELECT GroupMessageId, AliasId, DateViewed FROM dbo.GroupMessage_Alias WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.GroupMessage_Alias
GO
EXECUTE sp_rename N'dbo.Tmp_GroupMessage_Alias', N'GroupMessage_Alias', 'OBJECT' 
GO
ALTER TABLE dbo.GroupMessage_Alias ADD CONSTRAINT
	PK_GroupMessage_Alias PRIMARY KEY CLUSTERED 
	(
	GroupMessageId,
	AliasId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.GroupMessage_Alias ADD CONSTRAINT
	FK_GroupMessage_Alias_Alias FOREIGN KEY
	(
	AliasId
	) REFERENCES dbo.Alias
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
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
COMMIT