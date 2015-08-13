USE [CSSStats]
GO
/****** Object:  StoredProcedure [dbo].[ASGSServiceUpdateASRankings]    Script Date: 10/21/2012 09:55:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Tigereye, ported from sgt_baker's C# implementation
-- Create date: December 8, 2008
-- Description:	Updates the AllegSkill rankings with the stats collected in the specified GameID
-- =============================================
--IF OBJECT_ID('dbo.ASGSServiceUpdateASRankings', 'U') IS NOT NULL DROP PROCEDURE [dbo].[ASGSServiceUpdateASRankings];
ALTER PROCEDURE [dbo].[ASGSServiceUpdateASRankings] @GameID INT, @DebugMode INT = 0
AS
BEGIN
	SET NOCOUNT ON;
	
	-- DON'T UPDATE RANKINGS FOR A GAME THAT HAS ALREADY BEEN PROCESSED
	IF (((SELECT COUNT(GameID) FROM AS_GameStats WHERE GameID = @GameID) > 0) AND
		@DebugMode = 0)
	BEGIN
		RAISERROR('This game has already been processed.',16,1)
		RETURN
	END
	
	-- SET UP SOME CONSTANTS
	DECLARE @MAX_TEAMS INT
		SET @MAX_TEAMS = 2					-- Maximum 2 teams
	DECLARE @MIN_GAME_LENGTH_SECONDS INT
		SET @MIN_GAME_LENGTH_SECONDS = 300	-- 300seconds = 5mins
	DECLARE @MIN_PLAYERS_FOR_HALFGAME INT
		SET @MIN_PLAYERS_FOR_HALFGAME = 10	-- 10 players must have played > 1/2 the game 

	-- AllegSkill CONSTANTS
	DECLARE @INITIAL_MU FLOAT
		SET @INITIAL_MU = 25.0
	DECLARE @DYNAMICS_DIVISOR FLOAT
		SET @DYNAMICS_DIVISOR = 300.0
	DECLARE @PERFORMANCE_DIVISOR FLOAT
		SET @PERFORMANCE_DIVISOR = 6.0
	DECLARE @VARIANCE_DIVISOR FLOAT
		SET @VARIANCE_DIVISOR = 6.0
	DECLARE @DRAW_PERCENT FLOAT
		SET @DRAW_PERCENT = 1.101385

	DECLARE @DYNAMICS FLOAT
		SET @DYNAMICS = (@INITIAL_MU / @DYNAMICS_DIVISOR)
	DECLARE @PERFORMANCE FLOAT
		SET @PERFORMANCE = (@INITIAL_MU / @PERFORMANCE_DIVISOR)
	DECLARE @VARIANCE FLOAT
		SET @VARIANCE = (@INITIAL_MU / @VARIANCE_DIVISOR)

    DECLARE @GameDuration INT
    SET @GameDuration = (SELECT DATEDIFF(second,GameStartTime,GameEndTime) FROM Game WHERE GameIdentID = @GameID)

	-- GAME COUNTS BY DEFAULT
	DECLARE @GameCounted INT
		SET @GameCounted = 1

	-- TO BE OUTPUT VIA THE WEBSERVICE
	DECLARE @GameReason varchar(100)
		SET @GameReason = 'Game did not count.'

	-- SEE IF STATS WERE ENABLED IN GAME SETTINGS
	DECLARE @GameStatsCounted INT
		SET @GameStatsCounted = (SELECT GameStatsCount FROM Game WHERE GameIdentID = @GameID)

	-- THIS SHOULDN'T HAPPEN
	IF @GameDuration < 1 
	BEGIN
		SET @GameReason = 'Game did not count because duration was zero or negative'
		SET @GameCounted = 0
	END
	
	-- CHECK TO SEE IF GAME SHOULD COUNT

	-- GAMES WITH 'Stats Count' TURNED OFF DO NOT COUNT. 
	SET @GameStatsCounted = 1	-- Override this for now
	IF @GameStatsCounted <> 1
	BEGIN
		SET @GameReason = 'Game did not count because the Stats Count game setting was disabled.'
		SET @GameCounted = 0
	END

	-- 3+ TEAMERS DO NOT COUNT
	IF ((SELECT MAX(GameTeamNumber) FROM GameTeam WHERE GameID = @GameID) > 1)
	BEGIN
		SET @GameReason = 'Game did not count because more than two teams participated.'
		SET @GameCounted = 0
	END

	-- GAMES WITH FEWER THAN 10 PLAYERS FOR HALF THE GAME DO NOT COUNT
	IF ((SELECT COUNT(gtm.GameTeamMemberLoginID) FROM GameTeamMember gtm INNER JOIN GameTeam gt ON gt.GameTeamIdentID = gtm.GameTeamID WHERE gt.GameID = @GameID AND gtm.GameTeamMemberDuration > (@GameDuration / 2)) < @MIN_PLAYERS_FOR_HALFGAME)
	BEGIN
		SET @GameReason = 'Game did not count because less than ' + CONVERT(VARCHAR(4), @MIN_PLAYERS_FOR_HALFGAME) + ' players played for at least half of the game.'
		SET @GameCounted = 0
	END
	
	-- GAMES LESS THAN 5 MINUTES DO NOT COUNT
	IF (@GameDuration < @MIN_GAME_LENGTH_SECONDS)
	BEGIN
		SET @GameReason = 'Game did not count because it was not ' + CONVERT(VARCHAR(4), (@MIN_GAME_LENGTH_SECONDS / 60)) + 'mins long.'
		SET @GameCounted = 0
	END
	
	-- FIND OUT IF GAME WAS A DRAW
	DECLARE @GameIsDraw INT
		SET @GameIsDraw = 0
		
	-- BT 12/23/2010 - Convert from bit to int for summation.
	IF (SELECT SUM(CONVERT(INT, (CASE WHEN GameTeamWinner = 0 THEN 0 ELSE 1 END))) FROM GameTeam WHERE GameID = @GameID) = 0
		SET @GameIsDraw = 1
	
	IF @DebugMode = 1
	BEGIN
		IF @GameIsDraw = 1 PRINT 'GAME ' + CONVERT(VARCHAR(10), @GameID) + ' WAS A DRAW!' ELSE PRINT 'Game ' + CONVERT(VARCHAR(10), @GameID) + ' was won.'
	END
	
	-- DRAWS DO NOT COUNT
	IF @GameIsDraw = 1
	BEGIN
		SET @GameReason = 'Game did not count because it was a draw.'
		SET @GameCounted = 0
	END
	
	-- IF THE GAME COUNTED, CALCULATE STATS
	IF @GameCounted = 1
	BEGIN
		IF @DebugMode = 1 PRINT 'Game Counted. Processing stats...'
		
		-- GRAB THE WINNING/LOSING TEAM IDs AND INDICES.
		DECLARE @LosingTeamID INT
			SET @LosingTeamID = (SELECT TOP 1 GameTeamIdentID FROM GameTeam WHERE GameID = @GameID AND GameTeamWinner = 0)
		DECLARE @WinningTeamID INT
			SET @WinningTeamID = (SELECT TOP 1 GameTeamIdentID FROM GameTeam WHERE GameID = @GameID AND GameTeamIdentID <> @LosingTeamID)

		DECLARE @LosingTeamIndex INT
			SET @LosingTeamIndex = (SELECT GameTeamNumber FROM GameTeam WHERE GameTeamIdentID = @LosingTeamID)
		DECLARE @WinningTeamIndex INT
			SET @WinningTeamIndex = (SELECT GameTeamNumber FROM GameTeam WHERE GameTeamIdentID = @WinningTeamID)
		
		IF @DebugMode = 1
		BEGIN
			PRINT 'Winning Team Index: ' + CONVERT(VARCHAR(2), @WinningTeamIndex)
			PRINT 'Losing Team Index: ' + CONVERT(VARCHAR(2), @LosingTeamIndex)
			PRINT ''
		END

		-- CREATE A TEMPORARY TABLE TO WORK THROUGH THE STATS
		DECLARE @PlayerCount INT;
		DECLARE @Players TABLE (
			GTM_ID INT,
			LoginID INT,
			Callsign VARCHAR(50),
			Team INT,
			SecondsPlayed INT,
			FractionPlayed FLOAT,
			Mu FLOAT,
			Sigma FLOAT,
			DeltaMu FLOAT,
			DeltaSigma FLOAT,
			CommandMu FLOAT,
			CommandSigma FLOAT,
			StackRating FLOAT,
			Defector INT,
			GamePlayerKills INT,
			GameDroneKills INT,
			GameEjects INT,
			GameStationKills INT,
			GameStationCaptures INT,
			Pass INT)
		
		-- GET ALL THE CALLSIGNS FOR THIS GAME AND INSERT THEM INTO OUR TEMPORARY TABLE
		INSERT INTO @Players
			 SELECT DISTINCT 
					gtm.GameTeamMemberIdentID AS 'GTM_ID',
					gtm.GameTeamMemberLoginID AS 'LoginID',
					gtm.GameTeamMemberCallsign AS 'Callsign', 
					gt.GameTeamIdentID AS 'Team',
					gtm.GameTeamMemberDuration AS 'SecondsPlayed',
					(CONVERT(FLOAT, gtm.GameTeamMemberDuration) / CONVERT(FLOAT, @GameDuration)) AS 'FractionPlayed',
					0.0 AS 'Mu',
					0.0 AS 'Sigma',
					0.0 AS 'DeltaMu',
					0.0 AS 'DeltaSigma',
					0.0 AS 'CommandMu',
					0.0 AS 'CommandSigma',
					0.0 AS 'StackRating',
					0 AS 'GamePlayerKills',
					0 AS 'GameDroneKills',
					0 AS 'GameEjects',
					0 AS 'GameStationKills',
					0 AS 'GameStationCaptures',
					0 AS 'Defector',
					-1 AS 'Pass'
			   FROM GameTeamMember gtm
		 INNER JOIN GameTeam gt ON gt.GameTeamIdentID = gtm.GameTeamID
			  WHERE gt.GameID = @GameID
		
		IF @DebugMode = 1
		BEGIN
			SET @PlayerCount = (SELECT COUNT(*) FROM @Players);
			PRINT 'All Callsigns In Game (' + CONVERT(VARCHAR(5), @PlayerCount) + '):'
			SELECT * FROM @Players
		END
		
		IF (@DebugMode = 1)
		BEGIN
			IF ((SELECT COUNT(p.LoginID) FROM @Players p LEFT JOIN StatsLeaderboard sl ON (p.LoginID = sl.LoginID) WHERE sl.LoginID IS NULL) > 0)
			BEGIN
				PRINT 'Members that dont exist:'
				SELECT p.LoginID
				FROM @Players p LEFT JOIN StatsLeaderboard sl ON (p.LoginID = sl.LoginID)
				WHERE sl.LoginID IS NULL
			END
		END
		ELSE
		BEGIN
			-- DELETE PLAY ENTRIES FOR CALLSIGNS THAT DON'T ACTUALLY EXIST
			DELETE @Players
				WHERE LoginID IN (SELECT p.LoginID FROM @Players p LEFT JOIN StatsLeaderboard sl ON (p.LoginID = sl.LoginID) WHERE sl.LoginID IS NULL)
		END
		
		-- GRAB COMMANDER LoginIDs
		DECLARE @WinningCommLoginID INT
			SET @WinningCommLoginID = (SELECT GameTeamCommanderLoginID FROM GameTeam WHERE GameTeamIdentID = @WinningTeamID)
		DECLARE @LosingCommLoginID INT
			SET @LosingCommLoginID = (SELECT GameTeamCommanderLoginID FROM GameTeam WHERE GameTeamIdentID = @LosingTeamID)

		IF @DebugMode = 1
		BEGIN
			PRINT 'Winning Comm: ' + CONVERT(VARCHAR(10), @WinningCommLoginID);
			PRINT 'Losing Comm: ' + CONVERT(VARCHAR(10), @LosingCommLoginID);
			PRINT '';
		END
		
		-- ABORT IF COMMANDERS DO NOT EXIST
		IF (((SELECT COUNT(LoginID) FROM StatsLeaderboard WHERE LoginID IN (@WinningCommLoginID, @LosingCommLoginID)) < 2) OR (SELECT COUNT(LoginID) FROM @Players WHERE LoginID IN (@WinningCommLoginID, @LosingCommLoginID)) < 2)
		BEGIN
			RAISERROR('One or more of the commanders of this game do not exist. Game ignored.',16,1)
			RETURN
		END
		
		-- FLAG DEFECTORS, AND AGGREGATE WHORESTATS
		DECLARE @DefectCheckID INT
		DECLARE @DefectCheckLoginID INT
		DECLARE @DefectCheckTeam INT
		DECLARE @CumulativeDuration INT
		WHILE (SELECT COUNT(*) FROM @PLAYERS WHERE Pass = -1) > 0
		BEGIN
			SET @DefectCheckID = (SELECT TOP 1 GTM_ID FROM @Players WHERE Pass = -1)
			SET @DefectCheckLoginID = (SELECT TOP 1 LoginID FROM @Players WHERE GTM_ID = @DefectCheckID)
			SET @DefectCheckTeam = (SELECT Team FROM @Players WHERE GTM_ID = @DefectCheckID)
			SET @CumulativeDuration = (SELECT SUM(SecondsPlayed) FROM @Players WHERE LoginID = @DefectCheckLoginID AND Team = @DefectCheckTeam)

			-- IF THIS PLAYER DEFECTED, RECORD IT
			IF (((SELECT COUNT(DISTINCT Team) FROM @Players WHERE LoginID = @DefectCheckLoginID) > 1) AND ((SELECT Defector FROM @Players WHERE GTM_ID = @DefectCheckID) = 0))
			BEGIN
				IF @DebugMode = 1 PRINT '		FLAGGING DEFECTOR: ' + CONVERT(VARCHAR(6), @DefectCheckLoginID)
				UPDATE @Players
				SET Defector = 1,
				SecondsPlayed = @CumulativeDuration
				WHERE LoginID = @DefectCheckLoginID
			END
			
			-- COUNT THIS PLAYER AS CHECKED
			UPDATE @Players
			SET Pass = 0,
			FractionPlayed = (CONVERT(FLOAT, @CumulativeDuration) / CONVERT(FLOAT, @GameDuration)),
			GamePlayerKills = (SELECT Count(GameEventTargetLoginID) FROM GameEvent WHERE GameID = @GameID AND Team = @DefectCheckTeam AND EventID = 302 AND GameEventTargetLoginID = @DefectCheckLoginID AND GameEventPerformerName NOT LIKE '.%'),
			GameDroneKills = (SELECT Count(GameEventPerformerLoginID) FROM GameEvent WHERE GameID = @GameID AND Team = @DefectCheckTeam AND EventID = 302 AND GameEventTargetLoginID = @DefectCheckLoginID AND GameEventPerformerName LIKE '.%'),
			GameEjects = (SELECT Count(GameEventPerformerLoginID) FROM GameEvent WHERE GameID = @GameID AND Team = @DefectCheckTeam AND EventID = 302 AND GameEventPerformerLoginID = @DefectCheckLoginID),
			GameStationKills = (SELECT Count(GameEventPerformerLoginID) FROM GameEvent WHERE GameID = @GameID AND Team = @DefectCheckTeam AND EventID = 202 AND GameEventPerformerLoginID = @DefectCheckLoginID),
			GameStationCaptures = (SELECT Count(GameEventPerformerLoginID) FROM GameEvent WHERE GameID = @GameID AND Team = @DefectCheckTeam AND EventID = 203 AND GameEventPerformerLoginID = @DefectCheckLoginID)
			WHERE GTM_ID = @DefectCheckID
			
			-- REMOVE HIDERS FROM THIS SAME TEAM
			DELETE FROM @Players
			WHERE LoginID = @DefectCheckLoginID
					AND GTM_ID != @DefectCheckID
					AND Team = @DefectCheckTeam
					AND LoginID != @WinningCommLoginID
					AND LoginID != @LosingCommLoginID
		END
		-- REMOVE PLAYERS WHO PLAYED 30s OR LESS
		DELETE @Players
		WHERE SecondsPlayed < 30
			AND LoginID != @WinningCommLoginID
			AND LoginID != @LosingCommLoginID
		
		-- CREATE NEW AllegSkill ENTRIES FOR FIRST-TIME PLAYERS
		-- Don't need this, the rows are already in StatsLeaderboard
		--INSERT INTO AS_AllegSkill (LoginID)
		--	SELECT p.LoginID
		--	FROM @Players p LEFT JOIN AS_AllegSkill as_as ON (p.LoginID = as_as.LoginID)
		--	WHERE as_as.LoginID IS NULL
		
		-- UPDATE @Players WITH THEIR CURRENT MU/SIGMA VALUES
		UPDATE @Players
		SET Mu = a.Mu,
		Sigma = a.Sigma,
		CommandMu = a.CommandMu,
		CommandSigma = a.CommandSigma
		FROM @Players p, StatsLeaderboard a
		WHERE a.LoginID = p.LoginID
		
		-- ENSURE DURATIONS MAKE SENSE
		UPDATE @Players
		SET SecondsPlayed = @GameDuration
		WHERE SecondsPlayed > @GameDuration;
		
		UPDATE @Players
		SET SecondsPlayed = 0
		WHERE SecondsPlayed < 0
		
		-- CALCULATE THE TEAM MU AND SIGMA
		DECLARE @DynamicsSquared FLOAT
			SET @DynamicsSquared = @DYNAMICS * @DYNAMICS
		DECLARE @PerformanceSquared FLOAT
			SET @PerformanceSquared = @PERFORMANCE * @PERFORMANCE

		DECLARE @WinningTeamMu FLOAT
			SET @WinningTeamMu = (SELECT SUM(Mu * FractionPlayed) FROM @Players WHERE Team = @WinningTeamID)

		DECLARE @TempSigmaCalc FLOAT
			SET @TempSigmaCalc = ((SELECT SUM(((Sigma * Sigma) * FractionPlayed) + @DynamicsSquared + @PerformanceSquared)
									 FROM @Players
									 WHERE Team = @WinningTeamID) - @DynamicsSquared - @PerformanceSquared);
		
		-- Make sure we don't do a SQRT of a negative number
		IF @TempSigmaCalc < 0.0
		BEGIN
			RAISERROR('The WinningTeamSigma calculation at line 323 was about to calculate the SQRT of a negative number. Game ignored.',16,1)
			RETURN			
		END
		
		DECLARE @WinningTeamSigma FLOAT
			SET @WinningTeamSigma = (SQRT(@TempSigmaCalc))

		DECLARE @LosingTeamMu FLOAT
			SET @LosingTeamMu = (SELECT SUM(Mu * FractionPlayed) FROM @Players WHERE Team = @LosingTeamID)

		SET @TempSigmaCalc = (SELECT SUM(((Sigma * Sigma) * FractionPlayed) + @DynamicsSquared + @PerformanceSquared)
									 FROM @Players
									 WHERE Team = @LosingTeamID) - @DynamicsSquared - @PerformanceSquared;
		
		-- Make sure we don't do a SQRT of a negative number
		IF @TempSigmaCalc < 0.0
		BEGIN
			RAISERROR('The LosingTeamSigma calculation at line 338 was about to calculate the SQRT of a negative number. Game ignored.',16,1)
			RETURN			
		END
		
		DECLARE @LosingTeamSigma FLOAT
			SET @LosingTeamSigma = (SQRT(@TempSigmaCalc))
		
		IF @DebugMode = 1
		BEGIN
			PRINT 'Winning Team Mu: ' + CONVERT(VARCHAR(30), CAST(@WinningTeamMu AS DECIMAL(38, 20)))
			PRINT 'Winning Team Sigma: ' + CONVERT(VARCHAR(30), CAST(@WinningTeamSigma AS DECIMAL(38, 20)))
			PRINT 'Losing Team Mu: ' + CONVERT(VARCHAR(30), CAST(@LosingTeamMu AS DECIMAL(38, 20)))
			PRINT 'Losing Team Sigma: ' + CONVERT(VARCHAR(30), CAST(@LosingTeamSigma AS DECIMAL(38, 20)))
			PRINT ''
			SET @PlayerCount = (SELECT COUNT(*) FROM @Players);
			PRINT 'Populated players(' + CONVERT(VARCHAR(5), @PlayerCount) + '):'
			SELECT * FROM @Players;
		END
		
		-- CALCULATE EPSILON AND C
		-- Line 196 and 191
		DECLARE @NewEpsilon FLOAT
			SET @NewEpsilon = SQRT(2.0) * dbo.AS_GetInvErf(2.0 * (((@DRAW_PERCENT / 100.0) + 1.0) / 2.0) - 1.0) * SQRT(2.0) * @VARIANCE

		DECLARE @CSquared FLOAT
			SET @CSquared = (2.0 * POWER(@VARIANCE, 2) + POWER(@WinningTeamSigma, 2) + POWER(@LosingTeamSigma, 2))
		DECLARE @C FLOAT
			SET @C = SQRT(@CSquared)
		
		IF @DebugMode = 1
		BEGIN
			PRINT 'Epsilon: ' + CONVERT(VARCHAR(25), CAST(@NewEpsilon AS DECIMAL(38, 20)))
			PRINT 'CSquared: ' + CONVERT(VARCHAR(25), CAST(@CSquared AS DECIMAL(38, 20))) + '. C: ' + CONVERT(VARCHAR(25), CAST(@C AS DECIMAL(38, 20)))
			PRINT ''
		END
		
		-- CALCULATE INTERMEDIATE VALUES NEEDED FOR NEW MU AND SIGMA
		DECLARE @MuDifferenceOverC FLOAT
			SET @MuDifferenceOverC = ((@WinningTeamMu - @LosingTeamMu) / @C)
		DECLARE @T FLOAT
			SET @T = @MuDifferenceOverC
		DECLARE @E FLOAT
			SET @E = @NewEpsilon / @C
		
		DECLARE @CDF1 FLOAT
			SET @CDF1 = dbo.AS_GetCDF(0.0, 1.0, (@E - @T))
		DECLARE @CDF2 FLOAT
			SET @CDF2 = dbo.AS_GetCDF(0.0, 1.0, ((-1.0 * @E) - @T))

		DECLARE @TMinusE FLOAT
			SET @TMinusE = (@T - @E);
		DECLARE @Denominator FLOAT
			SET @Denominator = (dbo.AS_GetCDF(0.0, 1.0, (@E - @T)) - dbo.AS_GetCDF(0.0, 1.0, ((-1.0 * @E) - @T)))

		IF @DebugMode = 1
		BEGIN
			PRINT 'MuDiffOverC: ' + CONVERT(VARCHAR(30), CAST(@MuDifferenceOverC AS DECIMAL(38, 20)))
			PRINT 'T: ' + CONVERT(VARCHAR(30), CAST(@T AS DECIMAL(38, 20)))
			PRINT 'E: ' + CONVERT(VARCHAR(30), CAST(@E AS DECIMAL(38, 20)))
			PRINT 'TminusE: ' + CONVERT(VARCHAR(30), CAST(@TMinusE AS DECIMAL(38, 20)))
			PRINT 'Denominator: ' + CONVERT(VARCHAR(30), CAST(@Denominator AS DECIMAL(38, 20)))
			PRINT 'CDF1: ' + CONVERT(VARCHAR(30), CAST(@CDF1 AS decimal(38,20)))
			PRINT 'CDF2: ' + CONVERT(VARCHAR(30), CAST(@CDF2 AS decimal(38,20)))
			PRINT ''
		END
		
		DECLARE @NewWinningTeamMu FLOAT
		DECLARE @NewWinningTeamSigma FLOAT
		DECLARE @NewLosingTeamMu FLOAT
		DECLARE @NewLosingTeamSigma FLOAT
		
		-- CALCULATE THE NEW MU/SIGMAS FOR THE TEAMS
		IF (@GameIsDraw = 0)	-- If game is not draw
		BEGIN
			DECLARE @VWin FLOAT
				SET @VWin = (dbo.AS_GetPDF(0.0, 1.0, @TMinusE) / dbo.AS_GetCDF(0.0, 1.0, @TMinusE))
			DECLARE @WWin FLOAT
				SET @WWin = (@VWin * (@VWin + @TMinusE))

			SET @NewWinningTeamMu = (@WinningTeamMu + POWER(@WinningTeamSigma, 2) / @C * @VWin)
			
			SET @TempSigmaCalc = (POWER(@WinningTeamSigma, 2) * (1.0 - POWER(@WinningTeamSigma, 2) / @CSquared * @WWin) + @DynamicsSquared)
			IF @TempSigmaCalc < 0.0
			BEGIN
				RAISERROR('The WinningTeamSigma calculation at line 411 was about to calculate the SQRT of a negative number. Game ignored.',16,1)
				RETURN			
			END
			SET @NewWinningTeamSigma = (SQRT(@TempSigmaCalc))
			SET @NewLosingTeamMu = (@LosingTeamMu - POWER(@LosingTeamSigma, 2) / @C * @VWin)
			
			SET @TempSigmaCalc = (POWER(@LosingTeamSigma, 2) * (1.0 - POWER(@LosingTeamSigma, 2) / @CSquared * @WWin) + @DynamicsSquared)
			IF @TempSigmaCalc < 0.0
			BEGIN
				RAISERROR('The LosingTeamSigma calculation at line 420 was about to calculate the SQRT of a negative number. Game ignored.',16,1)
				RETURN			
			END
			SET @NewLosingTeamSigma = (SQRT(@TempSigmaCalc))
		
			IF @DebugMode = 1
			BEGIN
				PRINT 'VWin: ' + CONVERT(VARCHAR(15), @VWin)
				PRINT 'WWin: ' + CONVERT(VARCHAR(15), @WWin)
				PRINT ''
			END
		END
		ELSE
		BEGIN	-- Game is draw
			DECLARE @VDraw FLOAT
				SET @VDraw = ((dbo.AS_GetPDF(0.0, 1.0, (-1.0 * @E) - @T) - dbo.AS_GetPDF(0.0, 1.0, @E - @T)) / @Denominator)
			DECLARE @WDraw FLOAT
				SET @WDraw = (POWER(@VDraw, 2) + ((((@E - @T) * dbo.AS_GetPDF(0.0, 1.0, (@E - @T))) + (@E + @T) * dbo.AS_GetPDF(0.0, 1.0, (@E + @T)))) / @Denominator)

			IF @DebugMode = 1
			BEGIN
				PRINT 'VDraw: ' + CONVERT(VARCHAR(25), CAST(@VDraw AS DECIMAL(38, 20)))
				PRINT 'WDraw: ' + CONVERT(VARCHAR(25), CAST(@WDraw AS DECIMAL(38, 20)))
				PRINT 'E: ' + CONVERT(VARCHAR(25), CAST(@E AS DECIMAL(38, 20)))
				PRINT 'T: ' + CONVERT(VARCHAR(25), CAST(@T AS DECIMAL(38, 20)))
				PRINT 'Denominator: ' + CONVERT(VARCHAR(25), CAST(@Denominator AS DECIMAL(38, 20)))
				PRINT ''
			END

			SET @NewWinningTeamMu = (@WinningTeamMu + POWER(@WinningTeamSigma, 2) / @C * @VDraw)
			
			SET @TempSigmaCalc = (POWER(@WinningTeamSigma, 2) * (1.0 - POWER(@WinningTeamSigma, 2) / @CSquared * @WDraw) + @DynamicsSquared)
			IF @TempSigmaCalc < 0.0
			BEGIN
				RAISERROR('The WinningTeamSigma calculation at line 454 was about to calculate the SQRT of a negative number. Game ignored.',16,1)
				RETURN			
			END
			SET @NewWinningTeamSigma = (SQRT(@TempSigmaCalc))
			SET @NewLosingTeamMu = (@LosingTeamMu - POWER(@LosingTeamSigma, 2) / @C * @VDraw)
			
			SET @TempSigmaCalc = (POWER(@LosingTeamSigma, 2) * (1.0 - POWER(@LosingTeamSigma, 2) / @CSquared * @WDraw) + @DynamicsSquared)
			IF @TempSigmaCalc < 0.0
			BEGIN
				RAISERROR('The LosingTeamSigma calculation at line 463 was about to calculate the SQRT of a negative number. Game ignored.',16,1)
				RETURN			
			END
			SET @NewLosingTeamSigma = (SQRT(@TempSigmaCalc))
		END
		
		-- CALCULATE THE TOTAL VARIANCE
		DECLARE @TotalVariance FLOAT
			SET @TotalVariance = (SELECT SUM(((POWER(Sigma, 2) * FractionPlayed) + @DynamicsSquared + @PerformanceSquared)) FROM @Players)
		
		IF @DebugMode = 1
		BEGIN
			PRINT 'New WinningTeamMu: ' + CONVERT(VARCHAR(10), @NewWinningTeamMu)
			PRINT 'New WinningTeamSigma: ' + CONVERT(VARCHAR(10), @NewWinningTeamSigma)
			PRINT 'New LosingTeamMu: ' + CONVERT(VARCHAR(10), @NewLosingTeamMu)
			PRINT 'New LosingTeamSigma: ' + CONVERT(VARCHAR(10), @NewLosingTeamSigma)
			PRINT 'Total Variance: ' + CONVERT(VARCHAR(10), @TotalVariance)
			PRINT ''
		END

		-- Intermediate values for the V/W Win/Loser calcs
		DECLARE @WinningTeamTotalSigmaPlusDyn FLOAT
			SET @WinningTeamTotalSigmaPlusDyn = (((SELECT SUM((POWER(Sigma, 2) * FractionPlayed) + @DynamicsSquared + @PerformanceSquared) FROM @Players WHERE Team = @WinningTeamID) - @DynamicsSquared - @PerformanceSquared) + @DynamicsSquared)
		DECLARE @LosingTeamTotalSigmaPlusDyn FLOAT
			SET @LosingTeamTotalSigmaPlusDyn = (((SELECT SUM((POWER(Sigma, 2) * FractionPlayed) + @DynamicsSquared + @PerformanceSquared) FROM @Players WHERE Team = @LosingTeamID) - @DynamicsSquared - @PerformanceSquared) + @DynamicsSquared)
		
		-- CALCULATE V/W WINNER/LOSER VALUES
		DECLARE @VWinner FLOAT
			SET @VWinner = ((@NewWinningTeamMu - @WinningTeamMu) * SQRT(@TotalVariance) / @WinningTeamTotalSigmaPlusDyn)
		DECLARE @VLoser FLOAT
			SET @VLoser = ((@NewLosingTeamMu - @LosingTeamMu) * SQRT(@TotalVariance) / @LosingTeamTotalSigmaPlusDyn)
		DECLARE @WWinner FLOAT
			SET @WWinner = ((1.0 - (POWER(@NewWinningTeamSigma, 2) / POWER(@WinningTeamSigma, 2))) * @TotalVariance / @WinningTeamTotalSigmaPlusDyn)
		DECLARE @WLoser FLOAT
			SET @WLoser = ((1.0 - (POWER(@NewLosingTeamSigma, 2) / POWER(@LosingTeamSigma, 2))) * @TotalVariance / @LosingTeamTotalSigmaPlusDyn)

		IF @DebugMode = 1
		BEGIN
			PRINT 'VWinner: ' + CONVERT(VARCHAR(20), @VWinner)
			PRINT 'VLoser: ' + CONVERT(VARCHAR(20), @VLoser)
			PRINT 'WWinner: ' + CONVERT(VARCHAR(20), @WWinner)
			PRINT 'WLoser: ' + CONVERT(VARCHAR(20), @WLoser)
			PRINT ''
		END

		-- CALCULATE StackRatings
		DECLARE @WinnerStack FLOAT
			SET @WinnerStack = ((@WinningTeamMu - (3.0 * @WinningTeamSigma)) - (@LosingTeamMu - (3.0 * @LosingTeamSigma)))
		DECLARE @LoserStack FLOAT
			SET @LoserStack = ((@LosingTeamMu - (3.0 * @LosingTeamSigma)) - (@WinningTeamMu - (3.0 * @WinningTeamSigma)))
		
		-- UPDATE STACK RATINGS
		UPDATE @Players
		SET StackRating = @WinnerStack
		WHERE Team = @WinningTeamID

		UPDATE @Players
		SET StackRating = @LoserStack
		WHERE Team = @LosingTeamID

		IF @DebugMode = 1
		BEGIN
			PRINT 'WinnerStack: ' + CONVERT(VARCHAR(10), @WinnerStack)
			PRINT 'LoserStack: ' + CONVERT(VARCHAR(10), @LoserStack)
			PRINT ''
		END
		
		-- CALCULATE WINNER MU AND SIGMA VALUES FOR ALL PLAYERS ON WINNING TEAM
		UPDATE @Players
		SET DeltaMu = (((POWER(Sigma, 2) + @DynamicsSquared) / SQRT(@TotalVariance)) * @VWinner * FractionPlayed),
		DeltaSigma = (((Sigma * SQRT(1.0 - ((POWER(Sigma, 2) + @DynamicsSquared) / @TotalVariance) * @WWinner))) - Sigma) * FractionPlayed
		WHERE Team = @WinningTeamID

		-- CALCULATE LOSER MU AND SIGMA VALUES FOR ALL PLAYERS ON LOSING TEAM
		UPDATE @Players
		SET DeltaMu = (((POWER(Sigma, 2) + @DynamicsSquared) / SQRT(@TotalVariance)) * @VLoser * FractionPlayed),
		DeltaSigma = (((Sigma * SQRT(1.0 - ((POWER(Sigma, 2) + @DynamicsSquared) / @TotalVariance) * @WLoser))) - Sigma) * FractionPlayed
		WHERE Team = @LosingTeamID
		
		-- APPLY MU AND SIGMA UPDATES
		UPDATE @Players
		SET Mu = Mu + DeltaMu,
		Sigma = Sigma + DeltaSigma
		
		-- CALCULATE COMMANDER MU/SIGMA
		-- Grab their current mu/sigmas
		DECLARE @WinningCommMu FLOAT
			SET @WinningCommMu = (SELECT TOP 1 CommandMu FROM @Players WHERE LoginID = @WinningCommLoginID)
		DECLARE @WinningCommSigma FLOAT
			SET @WinningCommSigma = (SELECT TOP 1 CommandSigma FROM @Players WHERE LoginID = @WinningCommLoginID)
		DECLARE @LosingCommMu FLOAT
			SET @LosingCommMu = (SELECT TOP 1 CommandMu FROM @Players WHERE LoginID = @LosingCommLoginID)
		DECLARE @LosingCommSigma FLOAT
			SET @LosingCommSigma = (SELECT TOP 1 CommandSigma FROM @Players WHERE LoginID = @LosingCommLoginID)

		IF @DebugMode = 1
		BEGIN
			PRINT 'Winning Comm Mu: ' + CONVERT(VARCHAR(10), @WinningCommMu) + '. Sigma: ' + CONVERT(VARCHAR(10), @WinningCommSigma);
			PRINT 'Losing Comm Mu: ' + CONVERT(VARCHAR(10), @LosingCommMu) + '. Sigma: ' + CONVERT(VARCHAR(10), @LosingCommSigma);
		END
		
		-- Calculate comm C values
		DECLARE @CommCSquared FLOAT
			SET @CommCSquared = (2 * POWER(@VARIANCE, 2) + POWER(@WinningCommSigma, 2) + POWER(@LosingCommSigma, 2));
		DECLARE @CommC FLOAT
			SET @CommC = SQRT(@CommCSquared);
		
		IF (@DebugMode = 1)
		BEGIN
			PRINT 'Epsilon: ' + CONVERT(VARCHAR(10), @NewEpsilon);
			PRINT 'CommCSquared: ' + CONVERT(VARCHAR(10), @CommCSquared) + '. CommC: ' + CONVERT(VARCHAR(10), @CommC);
			PRINT ''
		END
		
		-- CALCULATE INTERMEDIATE VALUES NEEDED FOR NEW COMM MU AND SIGMA
		DECLARE @CommMuDifferenceOverC FLOAT
			SET @CommMuDifferenceOverC = ((@WinningCommMu - @LosingCommMu) / @CommC)
		DECLARE @CommT FLOAT
			SET @CommT = @CommMuDifferenceOverC
		DECLARE @CommE FLOAT
			SET @CommE = @NewEpsilon / @CommC

		DECLARE @CommTMinusE FLOAT
			SET @CommTMinusE = (@CommT - @CommE);
		DECLARE @CommDenominator FLOAT
			SET @CommDenominator = (dbo.AS_GetCDF(0.0, 1.0, (@CommE - @CommT)) - dbo.AS_GetCDF(0.0, 1.0, ((-1.0 * @CommE) - @CommT)))

		DECLARE @NewWinningCommMu FLOAT
		DECLARE @NewWinningCommSigma FLOAT
		DECLARE @NewLosingCommMu FLOAT
		DECLARE @NewLosingCommSigma FLOAT
		
		-- CALCULATE THE NEW MU/SIGMAS FOR THE COMMS
		IF (@GameIsDraw = 0)	-- If game is not draw
		BEGIN
			DECLARE @CommVWin FLOAT
				SET @CommVWin = (dbo.AS_GetPDF(0.0, 1.0, @CommTMinusE) / dbo.AS_GetCDF(0.0, 1.0, @CommTMinusE))
			DECLARE @CommWWin FLOAT
				SET @CommWWin = (@CommVWin * (@CommVWin + @CommTMinusE))

			SET @NewWinningCommMu = (@WinningCommMu + POWER(@WinningCommSigma, 2) / @CommC * @CommVWin)
			SET @NewWinningCommSigma = (SQRT(POWER(@WinningCommSigma, 2) * (1.0 - POWER(@WinningCommSigma, 2) / @CommCSquared * @CommWWin) + @DynamicsSquared))
			SET @NewLosingCommMu = (@LosingCommMu - POWER(@LosingCommSigma, 2) / @CommC * @CommVWin)
			SET @NewLosingCommSigma = (SQRT(POWER(@LosingCommSigma, 2) * (1.0 - POWER(@LosingCommSigma, 2) / @CommCSquared * @CommWWin) + @DynamicsSquared))
		
			IF @DebugMode = 1
			BEGIN
				PRINT 'CommVWin: ' + CONVERT(VARCHAR(10), @CommVWin)
				PRINT 'CommWWin: ' + CONVERT(VARCHAR(10), @CommWWin)
				PRINT ''
			END
		END
		ELSE
		BEGIN	-- Game is draw
			DECLARE @CommVDraw FLOAT
				SET @CommVDraw = ((dbo.AS_GetPDF(0.0, 1.0, (-1.0 * @CommE) - @CommT) - dbo.AS_GetPDF(0.0, 1.0, @CommE - @CommT)) / @CommDenominator)
			DECLARE @CommWDraw FLOAT
				SET @CommWDraw = (POWER(@CommVDraw, 2) + ((@CommE - @CommT) * dbo.AS_GetPDF(0.0, 1.0, (@CommE - @CommT)) + (@CommE + @CommT) * dbo.AS_GetPDF(0.0, 1.0, (@CommE + @CommT))) / @CommDenominator)
			
			IF @DebugMode = 1
			BEGIN
				PRINT 'CommVDraw: ' + CONVERT(VARCHAR(10), @CommVDraw)
				PRINT 'CommWDraw: ' + CONVERT(VARCHAR(10), @CommWDraw)
				PRINT ''
			END
			
			SET @NewWinningCommMu = (@WinningCommMu + POWER(@WinningCommSigma, 2) / @CommC * @CommVDraw)
			SET @NewWinningCommSigma = (SQRT(POWER(@WinningCommSigma, 2) * (1.0 - POWER(@WinningCommSigma, 2) / @CommCSquared * @CommWDraw) + @DynamicsSquared))
			SET @NewLosingCommMu = (@LosingCommMu - POWER(@LosingCommSigma, 2) / @CommC * @CommVDraw)
			SET @NewLosingCommSigma = (SQRT(POWER(@LosingCommSigma, 2) * (1.0 - POWER(@LosingCommSigma, 2) / @CommCSquared * @CommWDraw) + @DynamicsSquared))
		END
		
		IF @DebugMode = 1
		BEGIN
			PRINT 'New WinningCommMu: ' + CONVERT(VARCHAR(10), @NewWinningCommMu)
			PRINT 'New WinningCommSigma: ' + CONVERT(VARCHAR(10), @NewWinningCommSigma)
			PRINT 'New LosingCommMu: ' + CONVERT(VARCHAR(10), @NewLosingCommMu)
			PRINT 'New LosingCommSigma: ' + CONVERT(VARCHAR(10), @NewLosingCommSigma)
			PRINT ''
		END
		
		-- APPLY THE UPDATES TO OUR TEMPORARY TABLE
		UPDATE @Players
		SET CommandMu = @NewWinningCommMu,
		CommandSigma = @NewWinningCommSigma
		WHERE LoginID = @WinningCommLoginID
		
		UPDATE @Players
		SET CommandMu = @NewLosingCommMu,
		CommandSigma = @NewLosingCommSigma
		WHERE LoginID = @LosingCommLoginID
		-- COMMANDER CALCS END
		
		-- MAKE SURE MU AND SIGMA VALUES ARE SANE
		UPDATE @Players
		SET Mu = 50
		WHERE Mu > 50

		UPDATE @Players
		SET Mu = 0
		WHERE Mu < 0

		UPDATE @Players
		SET Sigma = 0
		WHERE Sigma < 0
		
		-- CALCULATE MATCH QUALITY
		DECLARE @VarianceSquared FLOAT
			SET @VarianceSquared = POWER(@Variance, 2)
		
		DECLARE @MatchQuality FLOAT
			SET @MatchQuality = (EXP(-1.0 * (POWER(@WinningTeamMu - @LosingTeamMu, 2) / (2.0 * @CSquared))) * SQRT((2.0 * @VarianceSquared) / @CSquared))
		
		IF @DebugMode = 1 PRINT 'MatchQuality: ' + CONVERT(VARCHAR(15), @MatchQuality)
		
		-- CALCULATE CONSERVATIVE RANKS
		DECLARE @WinningTeamConservativeRank FLOAT
			SET @WinningTeamConservativeRank = (@WinningTeamMu - (3.0 * @WinningTeamSigma))
		DECLARE @LosingTeamConservativeRank FLOAT
			SET @LosingTeamConservativeRank = (@LosingTeamMu - (3.0 * @LosingTeamSigma))
		
		DECLARE @Team1ConservativeRank FLOAT
		DECLARE @Team2ConservativeRank FLOAT

		IF @WinningTeamIndex = 0
		BEGIN
			SET @Team1ConservativeRank = @WinningTeamConservativeRank
			SET @Team2ConservativeRank = @LosingTeamConservativeRank
		END
		ELSE
		BEGIN
			SET @Team1ConservativeRank = @LosingTeamConservativeRank
			SET @Team2ConservativeRank = @WinningTeamConservativeRank
		END
		
		IF @DebugMode = 1
		BEGIN
			PRINT 'Team1ConservativeRank: ' + CONVERT(VARCHAR(10), @Team1ConservativeRank)
			PRINT 'Team2ConservativeRank: ' + CONVERT(VARCHAR(10), @Team2ConservativeRank)
			PRINT ''
		END

		-- HANDLE THE FINAL RESULTS (Print or save)
		IF @DebugMode = 0
		BEGIN
			-- SAVE GAME STATS
			INSERT INTO AS_GameStats (GameID, MatchQuality, Team1ConservativeRank, Team2ConservativeRank)
			VALUES (@GameID, @MatchQuality, @Team1ConservativeRank, @Team2ConservativeRank)
			
			-- SAVE THIS GAME'S UPDATES TO PLAYERSTATS
			INSERT INTO AS_GamePlayerAS (GameID, LoginID, NewMu, NewSigma, NewCommandMu, NewCommandSigma, StackRatingChange, Defected, KillCount, EjectCount, DroneKillCount, StationKillCount, StationCaptureCount)
				SELECT @GameID AS 'GameID',
					LoginID AS 'LoginID',
					Mu AS 'NewMu',
					Sigma AS 'NewSigma',
					CommandMu AS 'NewCommandMu',
					CommandSigma AS 'NewCommandSigma',
					StackRating AS 'StackRatingChange',
					Defector AS 'Defected',
					GamePlayerKills AS 'KillCount',
					GameEjects AS 'EjectCount',
					GameDroneKills AS 'DroneKillCount',
					GameStationKills AS 'StationKillCount',
					GameStationCaptures AS 'StationCaptureCount'
				FROM @Players
			
			-- UPDATE PLAYERS' ACTUAL STATS WITH CHANGES FROM THIS GAME
			DECLARE @UpdatingID INT;
			DECLARE @UpdatingLoginID INT;
			DECLARE @PlayedForWinningTeam INT;
			DECLARE @WinningCommander INT;
			DECLARE @LosingCommander INT;
			WHILE (SELECT COUNT(*) FROM @Players WHERE Pass < 1) > 0
			BEGIN
				-- Grab a row from our temporary table
				SET @UpdatingID = (SELECT TOP 1 GTM_ID FROM @Players WHERE PASS < 1);
				SET @UpdatingLoginID = (SELECT LoginID FROM @Players WHERE GTM_ID = @UpdatingID)
				
				-- Did they win?
				IF (SELECT Team FROM @Players WHERE GTM_ID = @UpdatingID) = @WinningTeamID
					SET @PlayedForWinningTeam = 1
				ELSE
					SET @PlayedForWinningTeam = 0
				
				-- Were they the winning comm?
				IF @UpdatingLoginID = @WinningCommLoginID
					SET @WinningCommander = 1
				ELSE
					SET @WinningCommander = 0
				
				-- Were they the losing comm?
				IF @UpdatingLoginID = @LosingCommLoginID
					SET @LosingCommander = 1
				ELSE
					SET @LosingCommander = 0
				
				-- Update this player's stats
				UPDATE StatsLeaderboard 
				SET Mu = p.Mu,
				Sigma = p.Sigma,
				Rank = dbo.AS_GetRank(p.Mu, p.Sigma),
				CommandMu = p.CommandMu,
				CommandSigma = p.CommandSigma,
				CommandRank = dbo.AS_GetRank(p.CommandMu, p.CommandSigma),
				StackRating = a.StackRating + (p.StackRating * p.FractionPlayed),
				Wins = a.Wins + @PlayedForWinningTeam,
				Losses = a.Losses + (@PlayedForWinningTeam ^ 1),
				Draws = a.Draws + @GameIsDraw,
				CommandWins = a.CommandWins + @WinningCommander,
				CommandLosses = a.CommandLosses + @LosingCommander,
				CommandDraws = a.CommandDraws + (@GameIsDraw & (@WinningCommander | @LosingCommander)),
				Defects = a.Defects + p.Defector,
				Kills = a.Kills + p.GamePlayerKills,
				Ejects = a.Ejects + p.GameEjects,
				DroneKills = a.DroneKills + p.GameDroneKills,
				StationKills = a.StationKills + p.GameStationKills,
				StationCaptures = a.StationCaptures + p.GameStationCaptures,
				HoursPlayed = a.HoursPlayed + (CONVERT(float, p.SecondsPlayed) / 3600),
				DateModified = GETDATE()
				FROM StatsLeaderboard a, @Players p
				WHERE (a.LoginID = @UpdatingLoginID AND p.GTM_ID = @UpdatingID)
				
				-- Flag this player as updated
				UPDATE @Players
				SET Pass = 1
				WHERE GTM_ID = @UpdatingID
			END
		END
		ELSE
		BEGIN
			PRINT 'Printing final values...'
			SELECT * FROM @Players
		END
	END
	ELSE
	BEGIN
		-- Game didn't count!
		IF @DebugMode = 1 PRINT @GameReason
	END
END
