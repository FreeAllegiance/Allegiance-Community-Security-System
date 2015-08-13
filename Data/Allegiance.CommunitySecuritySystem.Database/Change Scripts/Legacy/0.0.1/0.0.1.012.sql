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
ALTER TABLE dbo.MachineRecord
	DROP CONSTRAINT PK_MachineRecord
GO
ALTER TABLE dbo.MachineRecord SET (LOCK_ESCALATION = TABLE)
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
ALTER TABLE dbo.MachineRecord
	DROP CONSTRAINT FK_MachineRecord_MachineRecordType
GO
ALTER TABLE dbo.MachineRecordType SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.MachineRecord
	DROP CONSTRAINT FK_MachineRecord_Identity
GO
ALTER TABLE dbo.[Identity] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_MachineRecord
	(
	Id bigint NOT NULL IDENTITY (1, 1),
	IdentityId int NOT NULL,
	RecordTypeId int NOT NULL,
	Identifier nvarchar(255) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_MachineRecord SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_MachineRecord OFF
GO
IF EXISTS(SELECT * FROM dbo.MachineRecord)
	 EXEC('INSERT INTO dbo.Tmp_MachineRecord (IdentityId, RecordTypeId, Identifier)
		SELECT IdentityId, RecordTypeId, Identifier FROM dbo.MachineRecord WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.MachineRecord
GO
EXECUTE sp_rename N'dbo.Tmp_MachineRecord', N'MachineRecord', 'OBJECT' 
GO
ALTER TABLE dbo.MachineRecord ADD CONSTRAINT
	PK_MachineRecord PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.MachineRecord ADD CONSTRAINT
	FK_MachineRecord_Identity FOREIGN KEY
	(
	IdentityId
	) REFERENCES dbo.[Identity]
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.MachineRecord ADD CONSTRAINT
	FK_MachineRecord_MachineRecordType FOREIGN KEY
	(
	RecordTypeId
	) REFERENCES dbo.MachineRecordType
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
COMMIT
