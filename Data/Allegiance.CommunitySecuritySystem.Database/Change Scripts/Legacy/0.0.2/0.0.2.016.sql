/*
   Thursday, March 25, 20102:24:13 PM
   User: 
   Server: localhost
   Database: CSS
   Application: 
*/

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
ALTER TABLE dbo.Motd ADD
	LobbyId int NULL
GO
ALTER TABLE dbo.Motd ADD CONSTRAINT
	FK_Motd_Lobby FOREIGN KEY
	(
	LobbyId
	) REFERENCES dbo.Lobby
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Motd SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
