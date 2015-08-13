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
ALTER TABLE dbo.Ban ADD
	InEffect bit NOT NULL CONSTRAINT DF_Ban_InEffect DEFAULT 1
GO
ALTER TABLE dbo.Ban SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
CREATE FUNCTION AvailableKey
(	
	@LoginId int
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT K.Id, K.Token, K.[Filename], K.DateCreated, K.TransformMethodId
	FROM ActiveKey K
		LEFT OUTER JOIN UsedKey UK ON K.Id = UK.ActiveKeyId AND UK.LoginId = @LoginId
	GROUP BY K.Id, K.Token, K.[Filename], K.DateCreated, K.TransformMethodId
	HAVING COUNT(UK.ActiveKeyId) = 0
)
GO

INSERT INTO SessionStatus VALUES(1, 'PendingVerification')
INSERT INTO SessionStatus VALUES(2, 'Active')
INSERT INTO SessionStatus VALUES(3, 'Closed')