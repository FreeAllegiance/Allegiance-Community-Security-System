USE CSS
GO


CREATE PROCEDURE dbo.ScheduledTaskGenerateBlackBoxes
AS
	SET NOCOUNT ON 

	DECLARE @availableKeyCount INT

	SELECT @availableKeyCount  = COUNT(*)
	FROM ActiveKey (NOLOCK)
	WHERE Id NOT IN (SELECT ActiveKeyId FROM UsedKey)

	IF @availableKeyCount < 100
	BEGIN
		exec xp_cmdshell 'C:\CSS\TaskHandler\TaskHandler.exe -generateblackboxes'
	END

	RETURN 0
GO

-- TODO: change permissions to the acutal local computer account running the proc.
GRANT EXECUTE ON ScheduledTaskGenerateBlackBoxes TO css_server 
GO
