TRUNCATE TABLE AutoUpdateFile_Lobby
TRUNCATE TABLE Lobby_Login
GO

DELETE FROM Lobby
GO

SET IDENTITY_INSERT Lobby ON

INSERT INTO Lobby (Id, [Name], Host, BasePath, IsRestrictive, IsEnabled)
VALUES (0, 'Default', 'unavailable', 'C:\Development\Visual Studio 2008\Allegiance.CSS\trunk\Server\Allegiance.CommunitySecuritySystem.AutoUpdate\Files\0\', 0, 0)


INSERT INTO Lobby (Id, [Name], Host, BasePath, IsRestrictive, IsEnabled)
VALUES (1, 'Production', 'lobby.alleg.net', 'C:\Development\Visual Studio 2008\Allegiance.CSS\trunk\Server\Allegiance.CommunitySecuritySystem.AutoUpdate\Files\1\', 0, 1)

SET IDENTITY_INSERT Lobby OFF