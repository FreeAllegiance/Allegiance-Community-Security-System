/****** Object:  UserDefinedFunction [dbo].[FindAutoUpdateFiles]    Script Date: 09/06/2009 20:45:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[FindAutoUpdateFiles] 
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

GO


/****** Object:  UserDefinedFunction [dbo].[AvailableKey]    Script Date: 09/06/2009 20:45:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
CREATE FUNCTION [dbo].[AvailableKey]
(	
	@LoginId int
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT K.Id, K.Token, K.[Filename], K.DateCreated, K.TransformMethodId, K.IsValid
	FROM ActiveKey K
		LEFT OUTER JOIN UsedKey UK ON K.Id = UK.ActiveKeyId AND UK.LoginId = @LoginId
	GROUP BY K.Id, K.Token, K.[Filename], K.DateCreated, K.TransformMethodId, K.IsValid
	HAVING COUNT(UK.ActiveKeyId) = 0
)

GO


