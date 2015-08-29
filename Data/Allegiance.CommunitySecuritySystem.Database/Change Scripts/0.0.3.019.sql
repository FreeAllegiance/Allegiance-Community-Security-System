USE [CSSStats]
GO
/****** Object:  StoredProcedure [dbo].[SquadEdit]    Script Date: 08/07/2015 19:31:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nick Pirocanac (BackTrak)
-- 
-- Description:	Club Integration
-- =============================================
CREATE PROCEDURE [dbo].[SquadEdit]
	@SquadID int,
	@Description NVARCHAR(510),
	@URL NVARCHAR(255),
	@CivID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE CSS..[Group] SET 
		[Description] = LTRIM(RTRIM(@Description)),
		URL = LTRIM(RTRIM(@URL)) 
	WHERE Id = @SquadID
	
	SELECT 0, ''
END
GO
/****** Object:  StoredProcedure [dbo].[SquadDetails]    Script Date: 08/07/2015 19:31:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nick Pirocanac (BackTrak)
-- Club integration.
-- =============================================
CREATE PROCEDURE [dbo].[SquadDetails] 
	@SquadID INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT TOP 1
		g.Name + '@' + g.Tag as Name, 
		l.Username as LeaderName,
		g.Description as [Description], 
		'' as FavoriteGame1,
		'' as FavoriteGame2,
		'' as FavoriteGame3,
		'' as FavoriteGame4,
		'' as FavoriteGame5,
		g.URL as Url,
		CAST(0 AS BIT) as Closed,
		CAST(0 AS BIT) as Award,
		g.Id as SquadID,
		'None' as EditRestrictions,
		CONVERT(DateTime, '1/1/2000') as InceptionDate,
		-1 as CivID 
	FROM CSS..Login l
	JOIN CSS..Alias a on a.LoginId = l.Id
	JOIN CSS..Group_Alias_GroupRole gagr on gagr.AliasId = a.Id and gagr.GroupRoleId = 1 -- Squad Leader
	JOIN CSS..[Group] g on g.Id = gagr.GroupId
	--JOIN CSSStats..StatsLeaderboard sl on sl.LoginID = l.Id
	WHERE  g.IsSquad = 1
	AND g.Id = @SquadID
END
GO
/****** Object:  StoredProcedure [dbo].[SquadDemoteToMember]    Script Date: 08/07/2015 19:31:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nick Pirocanac (BackTrak)
-- Description:	Club Integration
-- =============================================
CREATE PROCEDURE [dbo].[SquadDemoteToMember]
	@LoginID INT,
	@SquadID INT
AS
BEGIN

	SET NOCOUNT ON;
	
	DECLARE @Results AS TABLE
	(
		ErrorCode INT, 
		Status INT
	)
	
	UPDATE CSS..Group_Alias_GroupRole
		SET  GroupRoleId = 6 -- Pilot
	FROM CSS..Group_Alias_GroupRole gagr
	JOIN CSS..Alias a on a.Id = gagr.AliasId
	WHERE a.LoginId = @LoginID
	AND gagr.GroupId = @SquadID
	
	IF @@ROWCOUNT <= 0
	BEGIN
		SELECT 11001 -- No member found to update to Pilot
		RETURN 0
	END
	
	SELECT 0
	
END
GO
/****** Object:  StoredProcedure [dbo].[SquadCreate]    Script Date: 08/07/2015 19:31:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nick Pirocanac (BackTrak)
--
-- Description:	Club Integration
-- =============================================
CREATE PROCEDURE [dbo].[SquadCreate] 
	-- Add the parameters for the stored procedure here
	@Name NVARCHAR(32),
	@SquadDescription NVARCHAR(510),
	@URL NVARCHAR(255),
	@LoginID int,
	@CivID int
AS
BEGIN

	SET NOCOUNT ON;
	
	SELECT @Name = RTRIM(LTRIM(@Name))

	DECLARE @Index INT
	SELECT @Index = CHARINDEX('@', @Name)
	IF @Index <= 0
	BEGIN
		SELECT -1, 11001, 'Please specify your squad in the format of <Name>@<Tag>. Example: Black Shadow@BS.'
		RETURN 0;
	END

	DECLARE @NamePart NVARCHAR(32)
	SELECT @NamePart = RTRIM(LTRIM(LEFT(@Name, @Index - 1)))

	IF LEN(@NamePart) <= 0 OR LEN(@NamePart) > 24
	BEGIN
		SELECT -1, 110012, 'Squad Name is too long or empty. Squad Name must be 24 or less characters.'
		RETURN 0;
	END

	DECLARE @TagPart NVARCHAR(32)
	SELECT @TagPart = RTRIM(LTRIM(RIGHT(@Name, LEN(@Name) - @Index)))

	IF LEN(@TagPart) <= 0 OR LEN(@TagPart) > 3
	BEGIN
		--SELECT -1, 11003, 'Tag is too long or empty. Tag must be 3 or less characters. ' + @NamePart + ' ' + @TagPart + ' ' + CONVERT(NVARCHAR(32), @Index)
		SELECT -1, 11003, '(' + @Name + ') (' + @NamePart + ') ('  + @TagPart + ') ('  + CONVERT(NVARCHAR(32), @Index) + ') ('  + RIGHT(@Name, LEN(@Name) - @Index) + ')'
		RETURN 0;
	END

	IF EXISTS (SELECT TOP 1 Id FROM CSS..[Group] WHERE Tag = @TagPart)
	BEGIN
		SELECT -1, 11004, 'Tag already exists for another squad. Please select a different tag.'
		RETURN 0;
	END

	IF EXISTS (SELECT TOP 1 Id FROM CSS..[Group] WHERE Name = @NamePart)
	BEGIN
		SELECT -1, 11005, 'Squad already exists. Please select a different squad name.'
		RETURN 0;
	END
	
	DECLARE @PrimaryAliasID INT
	
	-- Get the user's primary alias.
	SELECT TOP 1 @PrimaryAliasID = a.Id 
	FROM CSS..Alias a
	WHERE a.LoginId = @LoginID
	ORDER BY a.IsDefault DESC

	IF ISNULL(@PrimaryAliasID, 0) <= 0 BEGIN
		SELECT 11006
		RETURN 0;
	END
	
	DECLARE @GroupID INT

	INSERT INTO [CSS]..[Group] (Name, Tag, IsSquad, DateCreated, [Description], URL)
	VALUES(@NamePart, @TagPart, 1, GETDATE(), LTRIM(RTRIM(@SquadDescription)), LTRIM(RTRIM(@URL)))
	
	SELECT @GroupID = @@IDENTITY
	
	INSERT INTO CSS..Group_Alias_GroupRole (GroupId, AliasId, GroupRoleId)
	VALUES(@GroupID, @PrimaryAliasID, 1 /*Squad Leader */)
	
	SELECT @GroupID, 0, ''
	
END
GO
/****** Object:  StoredProcedure [dbo].[SquadCancelJoinRequest]    Script Date: 08/07/2015 19:31:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nick Pirocanac
-- Club Integration
-- =============================================
CREATE PROCEDURE [dbo].[SquadCancelJoinRequest] 
	@LoginID INT,
	@SquadID INT
AS
BEGIN

	SET NOCOUNT ON;

    IF NOT EXISTS 
    (
		SELECT TOP 1 gr.Id
		FROM CSS..GroupRequest gr
		JOIN CSS..Alias a on a.Id = gr.AliasId
		WHERE a.LoginId = @LoginID AND gr.GroupId = @SquadID
    )
    BEGIN
		SELECT 11001
		RETURN 0
    END
    
    DELETE CSS..GroupRequest
    FROM CSS..GroupRequest gr
    JOIN CSS..Alias a on a.Id = gr.AliasId
    WHERE a.LoginId = @LoginID AND gr.GroupId = @SquadID
    
    SELECT @@Error
    
END
GO
/****** Object:  StoredProcedure [dbo].[SquadBootMember]    Script Date: 08/07/2015 19:31:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nick Pirocanac (BackTrak)
-- Description:	Club Integration
-- =============================================
CREATE PROCEDURE [dbo].[SquadBootMember]
	@LoginID INT,
	@SquadID INT
AS
BEGIN

	SET NOCOUNT ON;

	DELETE CSS..Group_Alias_GroupRole
	FROM CSS..Group_Alias_GroupRole gr
	JOIN CSS..Alias a on a.Id = gr.AliasId
	JOIN CSS..Login l on l.Id = a.LoginId
	WHERE l.Id = @LoginID
	AND gr.GroupId = @SquadID
	
	IF @@ROWCOUNT = 0
	BEGIN
		SELECT 11001, 0 -- Couldn't find member to remove from group.
		RETURN 0
	END
	
	IF NOT EXISTS (SELECT TOP 1 gr.GroupId FROM CSS..Group_Alias_GroupRole gr WHERE gr.GroupId = @SquadID)
	BEGIN
	
		DELETE CSS..[Group] 
		FROM CSS..[Group] g
		WHERE g.Id = @SquadID
	
		IF @@ROWCOUNT = 0
		BEGIN
			SELECT 11002, 0 -- Couldn't delete group.
			RETURN 0
		END
		ELSE
		BEGIN
			SELECT 0, -1 -- Group removed.
			RETURN 0
		END
	END
	
	-- Member removed from group.
	SELECT 0, 0
	
END
GO
/****** Object:  StoredProcedure [dbo].[SquadAcceptJoinRequest]    Script Date: 08/07/2015 19:31:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nick Pirocanac (BackTrak)
-- Description:	Club Integration
-- =============================================
CREATE PROCEDURE [dbo].[SquadAcceptJoinRequest]
	@LoginID INT,
	@SquadID INT
AS
BEGIN

	SET NOCOUNT ON;

	INSERT INTO CSS..Group_Alias_GroupRole (GroupId, AliasId, GroupRoleId)
	SELECT gr.GroupId, gr.AliasId, 6 /* Pilot */ 
	FROM CSS..GroupRequest gr
	JOIN CSS..Alias a on a.Id = gr.AliasId
	JOIN CSS..Login l on l.Id = a.LoginId
	WHERE l.Id = @LoginID
	AND gr.GroupId = @SquadID
	AND gr.RequestTypeId = 1 -- JoinRequest
	
	IF @@ROWCOUNT <= 0
	BEGIN
		SELECT 11001 -- Nothing found to accept.
		RETURN 0
	END
	
	DELETE CSS..GroupRequest
	FROM CSS..GroupRequest gr
	JOIN CSS..Alias a on a.Id = gr.AliasId
	JOIN CSS..Login l on l.Id = a.LoginId
	WHERE l.Id = @LoginID
	AND gr.GroupId = @SquadID
	AND gr.RequestTypeId = 1 -- JoinRequest
	
	-- We don't care if nothing was deleted.
   
   SELECT 0
   
END
GO
/****** Object:  StoredProcedure [dbo].[SquadGetNear]    Script Date: 08/07/2015 19:31:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nick Pirocanac (BackTrak)
-- Club integration
-- =============================================
CREATE PROCEDURE [dbo].[SquadGetNear] 
	@Column int, 
	@SquadID int
AS
BEGIN
SELECT 
		CAST(FLOOR(AVG(ISNULL(sl.Rank, 0))) AS INT) as Ranking,
		g.Id as SquadID,
		g.Name as Name, 
		CAST(FLOOR(AVG(ISNULL(sl.Xp, 0))) AS INT) as Score, 
		CAST(FLOOR(AVG(ISNULL(sl.Wins, 0))) AS INT) as Wins,
		CAST(FLOOR(AVG(ISNULL(sl.Losses, 0))) AS INT) as Losses,
		'' as [Log],
		-1 as CivID 
	FROM CSS..[Group] g 
	JOIN CSS..Group_Alias_GroupRole gagr on g.Id = gagr.GroupId   
	JOIN CSS..Alias a on gagr.AliasId = a.Id 
	JOIN CSS..Login l on l.Id = a.LoginId
	LEFT JOIN CSSStats..StatsLeaderboard sl on sl.LoginID = l.Id
	WHERE  g.IsSquad = 1
	GROUP BY g.Id, g.Name
	ORDER BY 
		CASE WHEN @Column = 1 THEN AVG(ISNULL(sl.Rank, 0)) ELSE NULL END DESC,
		CASE WHEN @Column = 2 THEN g.Id ELSE NULL END,
		CASE WHEN @Column = 3 THEN g.Name ELSE NULL END,
		CASE WHEN @Column = 4 THEN AVG(ISNULL(sl.Xp, 0)) ELSE NULL END DESC,
		CASE WHEN @Column = 5 THEN AVG(ISNULL(sl.Wins, 0)) ELSE NULL END DESC,
		CASE WHEN @Column = 6 THEN AVG(ISNULL(sl.Losses, 0)) ELSE NULL END DESC
END
GO
/****** Object:  StoredProcedure [dbo].[SquadGetDudeXSquads]    Script Date: 08/07/2015 19:31:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SquadGetDudeXSquads] 
	@CharacterID int
AS
BEGIN

	SET NOCOUNT ON;

	SELECT DISTINCT
		g.Id as SquadID,
		g.Name as Name, 
		sl.Xp as Score, 
		sl.Wins as Wins,
		sl.Losses as Losses,
		'' as [Log],
		-1 as CivID 
	FROM CSS..Login l
	JOIN CSS..Alias a on a.LoginId = l.Id
	JOIN CSS..Group_Alias_GroupRole gagr on gagr.AliasId = a.Id
	JOIN CSS..[Group] g on g.Id = gagr.GroupId
	JOIN CSSStats..StatsLeaderboard sl on sl.LoginID = l.Id
	WHERE l.Id = @CharacterID
	and g.IsSquad = 1
		
END
GO
/****** Object:  StoredProcedure [dbo].[SquadDetailsPlayers]    Script Date: 08/07/2015 19:31:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nick Pirocanac (BackTrak)
-- Club integration
-- =============================================
CREATE PROCEDURE [dbo].[SquadDetailsPlayers] 
	@SquadID AS INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT DISTINCT
		l.Id as MemberID, 
		l.Username as MemberName,
		
		-- When Status = 0 then the user is SL. It it's 2 then it's "Pending Membership". 1 is not specifically defined in the Allegiance code.
		CASE WHEN gr.Id = 1 /* Squad Leader */ THEN 0 ELSE 1 END as Status, 
	
		-- When Status2 = 1 then the user is ASL. If it's 0, then they are just "Member". LULZ...
		CASE WHEN gr.Id = 2 /* Squad Leader */ THEN 1 ELSE 0 END as Status2, 
		
		l.DateCreated as Granted,
		ISNULL(sl.Rank, 1) as Rank
	FROM CSS..Login l
	JOIN CSS..Alias a on a.LoginId = l.Id
	JOIN CSS..Group_Alias_GroupRole gagr on gagr.AliasId = a.Id
	JOIN CSS..GroupRole gr on gr.Id = gagr.GroupRoleId 
	JOIN CSS..[Group] g on g.Id = gagr.GroupId
	LEFT JOIN CSSStats..StatsLeaderboard sl on sl.LoginID = l.Id
	WHERE  g.IsSquad = 1
	AND g.Id = @SquadID
	
	UNION
	
	SELECT 
		l.Id as MemberID, 
		l.Username as MemberName,
		2, -- Pending Membership
		0, -- Not a member yet!
		grq.DateCreated as Granted,
		sl.Rank as Rank
	FROM CSS..GroupRequest grq
	JOIN CSS..Alias a ON a.Id = grq.Id
	JOIN CSS..Login l on l.Id = a.LoginId
	JOIN CSSStats..StatsLeaderboard sl on sl.LoginID = l.Id
	WHERE grq.GroupId = @SquadID
END
GO
/****** Object:  StoredProcedure [dbo].[SquadTransferControl]    Script Date: 08/07/2015 19:31:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nick Pirocanac (BackTrak)
-- Description:	Club Integration
-- =============================================
CREATE PROCEDURE [dbo].[SquadTransferControl]
	@LoginID INT,
	@SquadID INT
AS
BEGIN

	SET NOCOUNT ON;
	
	DECLARE @Results AS TABLE
	(
		ErrorCode INT, 
		Status INT
	)
	
	-- Move all SLs to ASLs
	UPDATE CSS..Group_Alias_GroupRole
		SET  GroupRoleId = 2 -- Pilot
	FROM CSS..Group_Alias_GroupRole gagr
	JOIN CSS..Alias a on a.Id = gagr.AliasId
	WHERE gagr.GroupId = @SquadID
	AND gagr.GroupRoleId = 1 -- Squad Leader
	
	UPDATE CSS..Group_Alias_GroupRole
		SET  GroupRoleId = 1 -- Squad Leader
	FROM CSS..Group_Alias_GroupRole gagr
	JOIN CSS..Alias a on a.Id = gagr.AliasId
	WHERE a.LoginId = @LoginID
	AND gagr.GroupId = @SquadID
	
	IF @@ROWCOUNT <= 0
	BEGIN
		SELECT 11001 -- No member found to update to Squad Leader
		RETURN 0
	END
	
	SELECT 0
	
END
GO
/****** Object:  StoredProcedure [dbo].[SquadPromoteToASL]    Script Date: 08/07/2015 19:31:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nick Pirocanac (BackTrak)
-- Description:	Club Integration
-- =============================================
CREATE PROCEDURE [dbo].[SquadPromoteToASL]
	@LoginID INT,
	@SquadID INT
AS
BEGIN

	SET NOCOUNT ON;
	
	DECLARE @Results AS TABLE
	(
		ErrorCode INT, 
		Status INT
	)
	
	UPDATE CSS..Group_Alias_GroupRole
		SET  GroupRoleId = 2 -- Assistant Squad Leader
	FROM CSS..Group_Alias_GroupRole gagr
	JOIN CSS..Alias a on a.Id = gagr.AliasId
	WHERE a.LoginId = @LoginID
	AND gagr.GroupId = @SquadID
	
	IF @@ROWCOUNT <= 0
	BEGIN
		SELECT 11001 -- No member found to update to ASL
		RETURN 0
	END
	
	SELECT 0
	
END
GO
/****** Object:  StoredProcedure [dbo].[SquadMakeJoinRequest]    Script Date: 08/07/2015 19:31:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nick Pirocanac (BackTrak)
-- Club Integration.
-- =============================================
CREATE PROCEDURE [dbo].[SquadMakeJoinRequest] 
	-- Add the parameters for the stored procedure here
	@LoginID INT,
	@SquadID INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @PrimaryAliasID INT

   -- Get the user's primary alias.
   SELECT TOP 1 @PrimaryAliasID = a.Id 
   FROM CSS..Alias a
   WHERE a.LoginId = @LoginID
   ORDER BY a.IsDefault DESC
   
   IF ISNULL(@PrimaryAliasID, 0) <= 0 BEGIN
		SELECT 11001
		RETURN 0;
   END
   
   -- Check that the user doesn't already have an alias waiting to join this squad.
   IF EXISTS 
   (
	SELECT TOP 1 l.Id
	FROM CSS..Login l
	JOIN CSS..Alias a on a.LoginId = l.Id
	JOIN CSS..GroupRequest gr on gr.AliasId = a.Id
	WHERE l.Id = @LoginID AND gr.GroupId = @SquadID
   )
   BEGIN
		SELECT 11002
		RETURN 0;
   END
   
   -- Check that the user isn't already a member of the squad
   IF EXISTS 
   (
	SELECT TOP 1 l.Id
	FROM CSS..Login l
	JOIN CSS..Alias a on a.LoginId = l.Id
	JOIN CSS..Group_Alias_GroupRole gagr on gagr.AliasId = a.Id AND gagr.GroupId = @SquadID
	WHERE l.Id = @LoginID
   )
   BEGIN
		SELECT 11003
		RETURN 0;
   END
   
   DECLARE @JoinGroupRequestType INT = 1
   
   -- Insert into Group Request
   INSERT INTO CSS..GroupRequest ([RequestTypeId], [AliasId], [GroupId], [DateCreated])
   VALUES(@JoinGroupRequestType, @PrimaryAliasID, @SquadID, GETDATE())
  
   SELECT @@ERROR
   
END
GO
/****** Object:  View [dbo].[SquadStats]    Script Date: 08/07/2015 19:31:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create VIEW [dbo].[SquadStats]
AS

	SELECT 
		CAST(FLOOR(AVG(ISNULL(sl.Rank, 0))) AS INT) + 1 as Ranking,
		g.Id as SquadID,
		g.Name as Name, 
		CAST(FLOOR(AVG(ISNULL(sl.Xp, 0))) AS SMALLINT) as Score, 
		CAST(FLOOR(AVG(ISNULL(sl.Wins, 0))) AS SMALLINT) as Wins,
		CAST(FLOOR(AVG(ISNULL(sl.Losses, 0))) AS SMALLINT) as Losses,
		'' as [Log],
		-1 as CivID 
	FROM CSS..Login l
	JOIN CSS..Alias a on a.LoginId = l.Id
	JOIN CSS..Group_Alias_GroupRole gagr on gagr.AliasId = a.Id
	JOIN CSS..[Group] g on g.Id = gagr.GroupId
	LEFT JOIN CSSStats..StatsLeaderboard sl on sl.LoginID = l.Id
	WHERE  g.IsSquad = 1
	GROUP BY g.Id, g.Name
GO
/****** Object:  View [dbo].[Squads]    Script Date: 08/07/2015 19:31:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Squads]
AS
SELECT DISTINCT
	g.Name + ' is owned by ' + l.Username as OwnershipLog,
	g.Id as SquadID
	FROM CSS..Login l
	JOIN CSS..Alias a on a.LoginId = l.Id
	JOIN CSS..Group_Alias_GroupRole gagr on gagr.AliasId = a.Id and gagr.GroupRoleId = 1 -- Squad Leader
	JOIN CSS..GroupRole gr on gr.Id = gagr.GroupRoleId 
	JOIN CSS..[Group] g on g.Id = gagr.GroupId
	JOIN CSSStats..StatsLeaderboard sl on sl.LoginID = l.Id
GO
/****** Object:  StoredProcedure [dbo].[SquadRejectJoinRequest]    Script Date: 08/07/2015 19:31:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nick Pirocanac (BackTrak)
-- Description:	Club Integration
-- =============================================
CREATE PROCEDURE [dbo].[SquadRejectJoinRequest] 
	-- Add the parameters for the stored procedure here
	@LoginID INT, 
	@SquadID INT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE CSS..GroupRequest
		SET RequestTypeId = 2 -- Rejected
	FROM CSS..GroupRequest gr
	JOIN CSS..Alias a on a.Id = gr.AliasId
	JOIN CSS..Login l on l.Id = a.LoginId
	WHERE l.Id = @LoginID
	AND gr.GroupId = @SquadID
	AND gr.RequestTypeId = 1 -- JoinRequest
	
	IF @@ROWCOUNT <= 0
	BEGIN
		SELECT 11001 -- Nothing found to reject.
		RETURN 0
	END
   
   SELECT 0
   
END
GO
/****** Object:  StoredProcedure [dbo].[SquadQuit]    Script Date: 08/07/2015 19:31:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nick Pirocanac (BackTrak)
-- Description:	Club Integration
-- =============================================
CREATE PROCEDURE [dbo].[SquadQuit]
	@LoginID INT,
	@SquadID INT
AS
BEGIN

	SET NOCOUNT ON;
	
	DECLARE @Results AS TABLE
	(
		ErrorCode INT, 
		Status INT
	)
	
	INSERT INTO @Results EXEC SquadBootMember @LoginID, @SquadID

	SELECT * FROM @Results
	
END
GO
/****** Object:  View [dbo].[SquadStatsByWins]    Script Date: 08/07/2015 19:31:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[SquadStatsByWins]
AS
SELECT *, RANK() OVER ( ORDER BY Wins DESC, SquadID) AS Ordinal
FROM SquadStats
GO
/****** Object:  View [dbo].[SquadStatsByScore]    Script Date: 08/07/2015 19:31:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[SquadStatsByScore]
AS
SELECT *, RANK() OVER ( ORDER BY Score DESC, SquadID) AS Ordinal
FROM SquadStats
GO
/****** Object:  View [dbo].[SquadStatsByRanking]    Script Date: 08/07/2015 19:31:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[SquadStatsByRanking]
AS
SELECT *, RANK() OVER ( ORDER BY Ranking DESC, SquadID) AS Ordinal
FROM SquadStats
GO
/****** Object:  View [dbo].[SquadStatsByName]    Script Date: 08/07/2015 19:31:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[SquadStatsByName]
AS
SELECT *, RANK() OVER ( ORDER BY Name, SquadID) AS Ordinal
FROM SquadStats
GO
/****** Object:  View [dbo].[SquadStatsByLosses]    Script Date: 08/07/2015 19:31:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[SquadStatsByLosses]
AS
SELECT *, RANK() OVER ( ORDER BY Losses DESC, SquadID) AS Ordinal
FROM SquadStats
GO
/****** Object:  View [dbo].[Ranks]    Script Date: 08/07/2015 19:31:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Ranks]
AS
SELECT 
	Level as [Rank], 
	-1 as CivID,
	MaxXP as Requirement,
	Caption as Name
FROM  dbo.[Level]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Level"
            Begin Extent = 
               Top = 7
               Left = 48
               Bottom = 148
               Right = 232
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Ranks'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Ranks'
GO
/****** Object:  View [dbo].[CharStats]    Script Date: 08/07/2015 19:31:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CharStats]
AS
SELECT 
	LoginID as CharacterID,
	LoginUsername as CharacterName,
	-1 as CivID,
	Cast(Floor([Rank]) + 1 as INT) as [Rank],
	Xp as Score,
	Cast(Floor(HoursPlayed * 60) as INT) as MinutesPlayed,
	StationKills as BaseKills,
	StationCaptures as BaseCaptures,
	Kills,
	Cast(Kills / (Ejects + 1) * 1000 as SMALLINT) as Rating,
	Wins + Losses + Draws as GamesPlayed,
	Ejects as Deaths,
	Wins,
	Losses,
	CommandWins as WinsCmd,
	DateModified as LastPlayed
FROM  dbo.[StatsLeaderboard]
GO
/****** Object:  View [dbo].[CharacterInfo]    Script Date: 08/07/2015 19:31:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CharacterInfo]
AS
SELECT 
	LoginID as CharacterID,
	LoginUsername as CharacterName,
	[Description]
FROM  dbo.[StatsLeaderboard]
GO
/****** Object:  View [dbo].[CharStatsWithOrdinal]    Script Date: 08/07/2015 19:31:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CharStatsWithOrdinal]
AS
SELECT 
	LoginID as CharacterID,
	LoginUsername as CharacterName,
	-1 as CivID,
	Cast(Floor([Rank]) + 1 as INT) as [Rank],
	Xp as Score,
	Cast(Floor(HoursPlayed * 60) as INT) as MinutesPlayed,
	StationKills as BaseKills,
	StationCaptures as BaseCaptures,
	Kills,
	0 as Rating,
	Wins + Losses + Draws as GamesPlayed,
	Ejects as Deaths,
	Wins,
	Losses,
	CommandWins,
	RANK() OVER ( ORDER BY Xp DESC, LoginID) AS Ordinal
FROM  dbo.[StatsLeaderboard]
GO
/****** Object:  View [dbo].[CharStatsByWins]    Script Date: 08/07/2015 19:31:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CharStatsByWins]
AS
SELECT *, RANK() OVER ( ORDER BY Wins DESC, CharacterID) AS Ordinal
FROM CharStats
GO
/****** Object:  View [dbo].[CharStatsByScore]    Script Date: 08/07/2015 19:31:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CharStatsByScore]
AS
SELECT *, RANK() OVER ( ORDER BY Score DESC, CharacterID) AS Ordinal
FROM CharStats
GO
/****** Object:  View [dbo].[CharStatsByRating]    Script Date: 08/07/2015 19:31:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CharStatsByRating]
AS
SELECT *, RANK() OVER ( ORDER BY Rating DESC, CharacterID) AS Ordinal
FROM CharStats
GO
/****** Object:  View [dbo].[CharStatsByMinutesPlayed]    Script Date: 08/07/2015 19:31:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CharStatsByMinutesPlayed]
AS
SELECT *, RANK() OVER ( ORDER BY MinutesPlayed DESC, CharacterID) AS Ordinal
FROM CharStats
GO
/****** Object:  View [dbo].[CharStatsByLosses]    Script Date: 08/07/2015 19:31:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CharStatsByLosses]
AS
SELECT *, RANK() OVER ( ORDER BY Losses DESC, CharacterID) AS Ordinal
FROM CharStats
GO
/****** Object:  View [dbo].[CharStatsByKills]    Script Date: 08/07/2015 19:31:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CharStatsByKills]
AS
SELECT *, RANK() OVER ( ORDER BY Kills DESC, CharacterID) AS Ordinal
FROM CharStats
GO
/****** Object:  View [dbo].[CharStatsByGamesPlayed]    Script Date: 08/07/2015 19:31:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CharStatsByGamesPlayed]
AS
SELECT *, RANK() OVER ( ORDER BY GamesPlayed DESC, CharacterID) AS Ordinal
FROM CharStats
GO
/****** Object:  View [dbo].[CharStatsByDeaths]    Script Date: 08/07/2015 19:31:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CharStatsByDeaths]
AS
SELECT *, RANK() OVER ( ORDER BY Deaths DESC, CharacterID) AS Ordinal
FROM CharStats
GO
/****** Object:  View [dbo].[CharStatsByCommandWins]    Script Date: 08/07/2015 19:31:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CharStatsByCommandWins]
AS
SELECT *, RANK() OVER ( ORDER BY WinsCmd DESC, CharacterID) AS Ordinal
FROM CharStats
GO
/****** Object:  View [dbo].[CharStatsByCharacterName]    Script Date: 08/07/2015 19:31:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CharStatsByCharacterName]
AS
SELECT *, RANK() OVER ( ORDER BY CharacterName, CharacterID) AS Ordinal
FROM CharStats
GO
/****** Object:  View [dbo].[CharStatsByBaseKills]    Script Date: 08/07/2015 19:31:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CharStatsByBaseKills]
AS
SELECT *, RANK() OVER ( ORDER BY BaseKills DESC, CharacterID) AS Ordinal
FROM CharStats
GO
/****** Object:  View [dbo].[CharStatsByBaseCaptures]    Script Date: 08/07/2015 19:31:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CharStatsByBaseCaptures]
AS
SELECT *, RANK() OVER ( ORDER BY BaseCaptures DESC, CharacterID) AS Ordinal
FROM CharStats
GO
/****** Object:  StoredProcedure [dbo].[SquadGetNearNames]    Script Date: 08/07/2015 19:31:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nick Pirocanac (BackTrak)
-- Club integration
-- =============================================
CREATE PROCEDURE [dbo].[SquadGetNearNames]  
	@Column int,
	@Name NVARCHAR(32)
AS
BEGIN

    DECLARE @ItemOrdinal as INT
    DECLARE @FirstOrdinal INT
    
    IF @Column = 1 -- Ranking
    BEGIN
		SELECT @ItemOrdinal = Ordinal 
		FROM dbo.SquadStatsByRanking
		WHERE Name LIKE '%' + RTRIM(LTRIM(@Name)) + '%'
	    
		SET  @FirstOrdinal = dbo.fn_CalculateNearCharStatsOrdinal(@ItemOrdinal, RTRIM(LTRIM(@Name)))
	    
		SELECT TOP 82 @FirstOrdinal, SquadID, Name, Score, Wins, Losses, Log, CivID 
				FROM SquadStatsByName ss
				WHERE ss.Ordinal >= @FirstOrdinal 
				ORDER BY ss.Ordinal
	END
	
	ELSE IF @Column = 3 -- Score
    BEGIN
		SELECT @ItemOrdinal = Ordinal 
		FROM dbo.SquadStatsByScore
		WHERE Name LIKE '%' + RTRIM(LTRIM(@Name)) + '%'
	    
		SET  @FirstOrdinal = dbo.fn_CalculateNearCharStatsOrdinal(@ItemOrdinal, RTRIM(LTRIM(@Name)))
	    
		SELECT TOP 82 @FirstOrdinal, SquadID, Name, Score, Wins, Losses, Log, CivID 
				FROM SquadStatsByScore ss
				WHERE ss.Ordinal >= @FirstOrdinal 
				ORDER BY ss.Ordinal
	END
	ELSE IF @Column = 4 -- Wins
    BEGIN
		SELECT @ItemOrdinal = Ordinal 
		FROM dbo.SquadStatsByWins
		WHERE Name LIKE '%' + RTRIM(LTRIM(@Name)) + '%'
	    
		SET  @FirstOrdinal = dbo.fn_CalculateNearCharStatsOrdinal(@ItemOrdinal, RTRIM(LTRIM(@Name)))
	    
		SELECT TOP 82 @FirstOrdinal, SquadID, Name, Score, Wins, Losses, Log, CivID 
				FROM SquadStatsByWins ss
				WHERE ss.Ordinal >= @FirstOrdinal 
				ORDER BY ss.Ordinal
	END
	ELSE IF @Column = 5 -- Losses
    BEGIN
		SELECT @ItemOrdinal = Ordinal 
		FROM dbo.SquadStatsByLosses
		WHERE Name LIKE '%' + RTRIM(LTRIM(@Name)) + '%'
	    
		SET  @FirstOrdinal = dbo.fn_CalculateNearCharStatsOrdinal(@ItemOrdinal, RTRIM(LTRIM(@Name)))
	    
		SELECT TOP 82 @FirstOrdinal, SquadID, Name, Score, Wins, Losses, Log, CivID 
				FROM SquadStatsByLosses ss
				WHERE ss.Ordinal >= @FirstOrdinal 
				ORDER BY ss.Ordinal
	END
	ELSE -- IF @Column = 2 -- Name
    BEGIN
		SELECT @ItemOrdinal = Ordinal 
		FROM dbo.SquadStatsByName
		WHERE Name LIKE '%' + RTRIM(LTRIM(@Name)) + '%'
	    
		SET  @FirstOrdinal = dbo.fn_CalculateNearCharStatsOrdinal(@ItemOrdinal, RTRIM(LTRIM(@Name)))
	    
		SELECT TOP 82 @FirstOrdinal, SquadID, Name, Score, Wins, Losses, Log, CivID 
				FROM SquadStatsByName ss
				WHERE ss.Ordinal >= @FirstOrdinal 
				ORDER BY ss.Ordinal
	END

END

--[SquadGetNearNames] 2, 'mos'

--SELECT 
--		CAST(FLOOR(AVG(ISNULL(sl.Rank, 0))) AS INT) + 1 as Ranking,
--		g.Id as SquadID,
--		g.Name as Name, 
--		CAST(FLOOR(AVG(ISNULL(sl.Xp, 0))) AS SMALLINT) as Score, 
--		CAST(FLOOR(AVG(ISNULL(sl.Wins, 0))) AS SMALLINT) as Wins,
--		CAST(FLOOR(AVG(ISNULL(sl.Losses, 0))) AS SMALLINT) as Losses,
--		'' as [Log],
--		-1 as CivID 
--	FROM CSS..Login l
--	JOIN CSS..Alias a on a.LoginId = l.Id
--	JOIN CSS..Group_Alias_GroupRole gagr on gagr.AliasId = a.Id
--	JOIN CSS..[Group] g on g.Id = gagr.GroupId
--	LEFT JOIN CSSStats..StatsLeaderboard sl on sl.LoginID = l.Id
--	WHERE  g.IsSquad = 1
--	AND g.Name LIKE '%' + @Name + '%'
--	GROUP BY g.Id, g.Name
--	ORDER BY 
--		CASE WHEN @Column = 1 THEN AVG(sl.Rank) ELSE NULL END,
--		CASE WHEN @Column = 2 THEN g.Id ELSE NULL END,
--		CASE WHEN @Column = 3 THEN g.Name ELSE NULL END,
--		CASE WHEN @Column = 4 THEN AVG(sl.Xp) ELSE NULL END,
--		CASE WHEN @Column = 5 THEN AVG(sl.Wins) ELSE NULL END,
--		CASE WHEN @Column = 6 THEN AVG(sl.Losses) ELSE NULL END
GO
/****** Object:  StoredProcedure [dbo].[GetNearWinsCmd]    Script Date: 08/07/2015 19:31:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nick Pirocanac (BackTrak)
-- Create date: 8/2/2015
-- Description:	Gets the players with scores near the target player.
--
-- 83 appears to be the limit the allegiance client will display. 
-- It will append an empty record for the 84th row if no user was found 
-- for a query, so leave a gap for that.
-- =============================================
CREATE PROCEDURE [dbo].[GetNearWinsCmd] 
	@CharacterID int,
	@CivID int,
	@CharacterName varchar(100)
AS
BEGIN

	SET NOCOUNT ON;

    DECLARE @PlayerOrdinal as INT
    
    SELECT @PlayerOrdinal = Ordinal 
    FROM dbo.CharStatsByCommandWins
    WHERE CharacterID = @CharacterID OR CharacterName = @CharacterName
    
    DECLARE @FirstOrdinal INT
    
    SET  @FirstOrdinal = dbo.fn_CalculateNearCharStatsOrdinal(@PlayerOrdinal, @CharacterName)
    
	SELECT TOP 82 @FirstOrdinal, * 
			FROM CharStatsByCommandWins cs
			WHERE cs.Ordinal >= @FirstOrdinal 
			ORDER BY cs.Ordinal
END
GO
/****** Object:  StoredProcedure [dbo].[GetNearWins]    Script Date: 08/07/2015 19:31:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nick Pirocanac (BackTrak)
-- Create date: 8/2/2015
-- Description:	Gets the players with scores near the target player.
--
-- 83 appears to be the limit the allegiance client will display. 
-- It will append an empty record for the 84th row if no user was found 
-- for a query, so leave a gap for that.
-- =============================================
CREATE PROCEDURE [dbo].[GetNearWins] 
	@CharacterID int,
	@CivID int,
	@CharacterName varchar(100)
AS
BEGIN

	SET NOCOUNT ON;

    DECLARE @PlayerOrdinal as INT
    
    SELECT @PlayerOrdinal = Ordinal 
    FROM dbo.CharStatsByWins
    WHERE CharacterID = @CharacterID OR CharacterName = @CharacterName
    
    DECLARE @FirstOrdinal INT
    
    SET  @FirstOrdinal = dbo.fn_CalculateNearCharStatsOrdinal(@PlayerOrdinal, @CharacterName)
    
	SELECT TOP 82 @FirstOrdinal, * 
			FROM CharStatsByWins cs
			WHERE cs.Ordinal >= @FirstOrdinal 
			ORDER BY cs.Ordinal
END
GO
/****** Object:  StoredProcedure [dbo].[GetNearScore]    Script Date: 08/07/2015 19:31:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nick Pirocanac (BackTrak)
-- Create date: 8/2/2015
-- Description:	Gets the players with scores near the target player.
--
-- 83 appears to be the limit the allegiance client will display. 
-- It will append an empty record for the 84th row if no user was found 
-- for a query, so leave a gap for that.
-- =============================================
CREATE PROCEDURE [dbo].[GetNearScore] 
	@CharacterID int,
	@CivID int,
	@CharacterName varchar(100)
AS
BEGIN

	SET NOCOUNT ON;

    DECLARE @PlayerOrdinal as INT
    
    SELECT @PlayerOrdinal = Ordinal 
    FROM CharStatsByScore
    WHERE CharacterID = @CharacterID OR CharacterName = @CharacterName
    
    DECLARE @FirstOrdinal INT
    
    SET  @FirstOrdinal = dbo.fn_CalculateNearCharStatsOrdinal(@PlayerOrdinal, @CharacterName)

	SELECT TOP 82 @FirstOrdinal, * 
			FROM CharStatsByScore cs
			WHERE cs.Ordinal >= @FirstOrdinal 
			ORDER BY cs.Ordinal
END
GO
/****** Object:  StoredProcedure [dbo].[GetNearRating]    Script Date: 08/07/2015 19:31:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nick Pirocanac (BackTrak)
-- Create date: 8/2/2015
-- Description:	Gets the players with scores near the target player.
--
-- 83 appears to be the limit the allegiance client will display. 
-- It will append an empty record for the 84th row if no user was found 
-- for a query, so leave a gap for that.
-- =============================================
CREATE PROCEDURE [dbo].[GetNearRating] 
	@CharacterID int,
	@CivID int,
	@CharacterName varchar(100)
AS
BEGIN

	SET NOCOUNT ON;

    DECLARE @PlayerOrdinal as INT
    
    SELECT @PlayerOrdinal = Ordinal 
    FROM dbo.CharStatsByRating
    WHERE CharacterID = @CharacterID OR CharacterName = @CharacterName
    
    DECLARE @FirstOrdinal INT
    
    SET  @FirstOrdinal = dbo.fn_CalculateNearCharStatsOrdinal(@PlayerOrdinal, @CharacterName)
    
	SELECT TOP 82 @FirstOrdinal, * 
			FROM CharStatsByRating cs
			WHERE cs.Ordinal >= @FirstOrdinal 
			ORDER BY cs.Ordinal
END
GO
/****** Object:  StoredProcedure [dbo].[GetNearNames]    Script Date: 08/07/2015 19:31:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nick Pirocanac (BackTrak)
-- Create date: 8/2/2015
-- Description:	Gets the players with scores near the target player.
--
-- 83 appears to be the limit the allegiance client will display. 
-- It will append an empty record for the 84th row if no user was found 
-- for a query, so leave a gap for that.
-- =============================================
CREATE PROCEDURE [dbo].[GetNearNames] 
	@CharacterID int,
	@CivID int,
	@CharacterName varchar(100)
AS
BEGIN

	SET NOCOUNT ON;

    DECLARE @PlayerOrdinal as INT
    
    SELECT @PlayerOrdinal = Ordinal 
    FROM dbo.CharStatsByCharacterName
    WHERE CharacterID = @CharacterID OR CharacterName = @CharacterName
    
    DECLARE @FirstOrdinal INT
    
    SET  @FirstOrdinal = dbo.fn_CalculateNearCharStatsOrdinal(@PlayerOrdinal, @CharacterName)
    
	SELECT TOP 82 @FirstOrdinal, * 
			FROM CharStatsByCharacterName cs
			WHERE cs.Ordinal >= @FirstOrdinal 
			ORDER BY cs.Ordinal
END
GO
/****** Object:  StoredProcedure [dbo].[GetNearMinutesPlayed]    Script Date: 08/07/2015 19:31:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nick Pirocanac (BackTrak)
-- Create date: 8/2/2015
-- Description:	Gets the players with scores near the target player.
--
-- 83 appears to be the limit the allegiance client will display. 
-- It will append an empty record for the 84th row if no user was found 
-- for a query, so leave a gap for that.
-- =============================================
CREATE PROCEDURE [dbo].[GetNearMinutesPlayed] 
	@CharacterID int,
	@CivID int,
	@CharacterName varchar(100)
AS
BEGIN

	SET NOCOUNT ON;

    DECLARE @PlayerOrdinal as INT
    
    SELECT @PlayerOrdinal = Ordinal 
    FROM dbo.CharStatsByMinutesPlayed
    WHERE CharacterID = @CharacterID OR CharacterName = @CharacterName
    
    DECLARE @FirstOrdinal INT
    
    SET  @FirstOrdinal = dbo.fn_CalculateNearCharStatsOrdinal(@PlayerOrdinal, @CharacterName)
    
	SELECT TOP 82 @FirstOrdinal, * 
			FROM CharStatsByMinutesPlayed cs
			WHERE cs.Ordinal >= @FirstOrdinal 
			ORDER BY cs.Ordinal
END
GO
/****** Object:  StoredProcedure [dbo].[GetNearLosses]    Script Date: 08/07/2015 19:31:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nick Pirocanac (BackTrak)
-- Create date: 8/2/2015
-- Description:	Gets the players with scores near the target player.
--
-- 83 appears to be the limit the allegiance client will display. 
-- It will append an empty record for the 84th row if no user was found 
-- for a query, so leave a gap for that.
-- =============================================
CREATE PROCEDURE [dbo].[GetNearLosses] 
	@CharacterID int,
	@CivID int,
	@CharacterName varchar(100)
AS
BEGIN

	SET NOCOUNT ON;

    DECLARE @PlayerOrdinal as INT
    
    SELECT @PlayerOrdinal = Ordinal 
    FROM dbo.CharStatsByLosses
    WHERE CharacterID = @CharacterID OR CharacterName = @CharacterName
    
    DECLARE @FirstOrdinal INT
    
    SET  @FirstOrdinal = dbo.fn_CalculateNearCharStatsOrdinal(@PlayerOrdinal, @CharacterName)
    
	SELECT TOP 82 @FirstOrdinal, * 
			FROM CharStatsByLosses cs
			WHERE cs.Ordinal >= @FirstOrdinal 
			ORDER BY cs.Ordinal
END
GO
/****** Object:  StoredProcedure [dbo].[GetNearKills]    Script Date: 08/07/2015 19:31:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nick Pirocanac (BackTrak)
-- Create date: 8/2/2015
-- Description:	Gets the players with scores near the target player.
--
-- 83 appears to be the limit the allegiance client will display. 
-- It will append an empty record for the 84th row if no user was found 
-- for a query, so leave a gap for that.
-- =============================================
CREATE PROCEDURE [dbo].[GetNearKills] 
	@CharacterID int,
	@CivID int,
	@CharacterName varchar(100)
AS
BEGIN

	SET NOCOUNT ON;

    DECLARE @PlayerOrdinal as INT
    
    SELECT @PlayerOrdinal = Ordinal 
    FROM dbo.CharStatsByKills
    WHERE CharacterID = @CharacterID OR CharacterName = @CharacterName
    
    DECLARE @FirstOrdinal INT
    
    SET  @FirstOrdinal = dbo.fn_CalculateNearCharStatsOrdinal(@PlayerOrdinal, @CharacterName)
    
	SELECT TOP 82 @FirstOrdinal, * 
			FROM CharStatsByKills cs
			WHERE cs.Ordinal >= @FirstOrdinal 
			ORDER BY cs.Ordinal
END
GO
/****** Object:  StoredProcedure [dbo].[GetNearGamesPlayed]    Script Date: 08/07/2015 19:31:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nick Pirocanac (BackTrak)
-- Create date: 8/2/2015
-- Description:	Gets the players with scores near the target player.
--
-- 83 appears to be the limit the allegiance client will display. 
-- It will append an empty record for the 84th row if no user was found 
-- for a query, so leave a gap for that.
-- =============================================
CREATE PROCEDURE [dbo].[GetNearGamesPlayed] 
	@CharacterID int,
	@CivID int,
	@CharacterName varchar(100)
AS
BEGIN

	SET NOCOUNT ON;

    DECLARE @PlayerOrdinal as INT
    
    SELECT @PlayerOrdinal = Ordinal 
    FROM dbo.CharStatsByGamesPlayed
    WHERE CharacterID = @CharacterID OR CharacterName = @CharacterName
    
    --PRINT @PlayerOrdinal
    
    DECLARE @FirstOrdinal INT
    
    SET  @FirstOrdinal = dbo.fn_CalculateNearCharStatsOrdinal(@PlayerOrdinal, @CharacterName)
    
     --PRINT @FirstOrdinal
     
    -- 	DECLARE @TotalRowCount as INT
    
    --SELECT @TotalRowCount = COUNT(*) 
    --FROM CharStats
    
    --PRINT @TotalRowCount
     
	SELECT TOP 82 @FirstOrdinal, * 
			FROM CharStatsByGamesPlayed cs
			WHERE cs.Ordinal >= @FirstOrdinal 
			ORDER BY cs.Ordinal
END
GO
/****** Object:  StoredProcedure [dbo].[GetNearDeaths]    Script Date: 08/07/2015 19:31:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nick Pirocanac (BackTrak)
-- Create date: 8/2/2015
-- Description:	Gets the players with scores near the target player.
--
-- 83 appears to be the limit the allegiance client will display. 
-- It will append an empty record for the 84th row if no user was found 
-- for a query, so leave a gap for that.
-- =============================================
CREATE PROCEDURE [dbo].[GetNearDeaths] 
	@CharacterID int,
	@CivID int,
	@CharacterName varchar(100)
AS
BEGIN

	SET NOCOUNT ON;

    DECLARE @PlayerOrdinal as INT
    
    SELECT @PlayerOrdinal = Ordinal 
    FROM dbo.CharStatsByDeaths
    WHERE CharacterID = @CharacterID OR CharacterName = @CharacterName
    
    DECLARE @FirstOrdinal INT
    
    SET  @FirstOrdinal = dbo.fn_CalculateNearCharStatsOrdinal(@PlayerOrdinal, @CharacterName)
    
	SELECT TOP 82 @FirstOrdinal, * 
			FROM CharStatsByDeaths cs
			WHERE cs.Ordinal >= @FirstOrdinal 
			ORDER BY cs.Ordinal
END
GO
/****** Object:  StoredProcedure [dbo].[GetNearBaseKills]    Script Date: 08/07/2015 19:31:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nick Pirocanac (BackTrak)
-- Create date: 8/2/2015
-- Description:	Gets the players with scores near the target player.
--
-- 83 appears to be the limit the allegiance client will display. 
-- It will append an empty record for the 84th row if no user was found 
-- for a query, so leave a gap for that.
-- =============================================
CREATE PROCEDURE [dbo].[GetNearBaseKills] 
	@CharacterID int,
	@CivID int,
	@CharacterName varchar(100)
AS
BEGIN

	SET NOCOUNT ON;

    DECLARE @PlayerOrdinal as INT
    
    SELECT @PlayerOrdinal = Ordinal 
    FROM dbo.CharStatsByBaseKills
    WHERE CharacterID = @CharacterID OR CharacterName = @CharacterName
    
    DECLARE @FirstOrdinal INT
    
    SET  @FirstOrdinal = dbo.fn_CalculateNearCharStatsOrdinal(@PlayerOrdinal, @CharacterName)
    
	SELECT TOP 82 @FirstOrdinal, * 
			FROM CharStatsByBaseKills cs
			WHERE cs.Ordinal >= @FirstOrdinal 
			ORDER BY cs.Ordinal
END
GO
/****** Object:  StoredProcedure [dbo].[GetNearBaseCaptures]    Script Date: 08/07/2015 19:31:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nick Pirocanac (BackTrak)
-- Create date: 8/2/2015
-- Description:	Gets the players with scores near the target player.
--
-- 83 appears to be the limit the allegiance client will display. 
-- It will append an empty record for the 84th row if no user was found 
-- for a query, so leave a gap for that.
-- =============================================
CREATE PROCEDURE [dbo].[GetNearBaseCaptures] 
	@CharacterID int,
	@CivID int,
	@CharacterName varchar(100)
AS
BEGIN

	SET NOCOUNT ON;

    DECLARE @PlayerOrdinal as INT
    
    SELECT @PlayerOrdinal = Ordinal 
    FROM dbo.CharStatsByBaseCaptures
    WHERE CharacterID = @CharacterID OR CharacterName = @CharacterName
    
    DECLARE @FirstOrdinal INT
    
    SET  @FirstOrdinal = dbo.fn_CalculateNearCharStatsOrdinal(@PlayerOrdinal, @CharacterName)
    
	SELECT TOP 82 @FirstOrdinal, * 
			FROM CharStatsByBaseCaptures cs
			WHERE cs.Ordinal >= @FirstOrdinal 
			ORDER BY cs.Ordinal
END
GO


USE [CSSStats]
GO

/****** Object:  UserDefinedFunction [dbo].[fn_CalculateNearCharStatsOrdinal]    Script Date: 08/26/2015 22:33:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Nick Pirocanac (BackTrak)
-- 
-- Calculates the ordinal location to show the target item 
-- and the leading and trailing items within 41 on each side.
-- =============================================
CREATE FUNCTION [dbo].[fn_CalculateNearCharStatsOrdinal] 
(
	@CharacterOrdinal int,
	@SearchString nvarchar(100)
)
RETURNS int 
AS
BEGIN
	DECLARE @FirstOrdinal INT
	
	DECLARE @TotalRowCount as INT
    
    SELECT @TotalRowCount = COUNT(*) 
    FROM CharStats
    
    IF @CharacterOrdinal IS NULL AND LEN(ISNULL(@SearchString, '')) = 0
    BEGIN
		SET @FirstOrdinal = 1
    END
    
    ELSE IF @TotalRowCount - ISNULL(@CharacterOrdinal, 1) < 40
    BEGIN
		SET @FirstOrdinal = @TotalRowCount - 82;
		IF @FirstOrdinal < 1
		BEGIN
			SET @FirstOrdinal = 1
		END
    END
    
    ELSE IF ISNULL(@CharacterOrdinal, 1) < 40
    BEGIN
		SET @FirstOrdinal = 1
    END
    
    ELSE
    BEGIN
		SET @FirstOrdinal = ISNULL(@CharacterOrdinal, 1) - 40
		IF @FirstOrdinal < 1
		BEGIN
			SET @FirstOrdinal = 1
		END
    END

	RETURN @FirstOrdinal

END

GO


