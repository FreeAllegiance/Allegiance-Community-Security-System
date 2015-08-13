/* REMOVE OLD STUFF */
DELETE FROM [CSS].[dbo].[Session]
      WHERE 1=1
GO
DELETE FROM [CSS].[dbo].[UsedKey]
      WHERE 1=1
GO
DELETE FROM [CSS].[dbo].[GroupMessage_Alias]
      WHERE 1=1
GO
DELETE FROM [CSS].[dbo].[GroupMessage]
      WHERE 1=1
GO
DELETE FROM [CSS].[dbo].[Group]
      WHERE 1=1
GO
DELETE FROM [CSS].[dbo].[Alias]
      WHERE Callsign != 'Orion'
GO
DELETE FROM [CSS].[dbo].[Login]
      WHERE Username != 'Orion'
GO
/* CREATE NEW IDENTITIES */
INSERT INTO [CSS].[dbo].[Identity]
           ([DateLastLogin])
     VALUES
           ('2009-09-05 22:32:57.827')
GO
INSERT INTO [CSS].[dbo].[Identity]
           ([DateLastLogin])
     VALUES
           ('2009-09-05 22:32:57.827')
GO
INSERT INTO [CSS].[dbo].[Identity]
           ([DateLastLogin])
     VALUES
           ('2009-09-05 22:32:57.827')
GO
INSERT INTO [CSS].[dbo].[Identity]
           ([DateLastLogin])
     VALUES
           ('2009-09-05 22:32:57.827')
GO
/* CREATE NEW LOGINS */
INSERT INTO [CSS].[dbo].[Login]
           ([Username]
           ,[Password]
           ,[Email]
           ,[DateCreated]
           ,[IdentityId])
     VALUES
           ('One'
           ,'a4ayc/80/OGda4BO/1o/V0etpOqiLx1JwB5S3beHW0s='
           ,'1@1.com'
           ,'2009-09-05 22:32:57.827'
           ,(SELECT MAX(Id)-3 FROM [CSS].[dbo].[Identity]))
GO
INSERT INTO [CSS].[dbo].[Login]
           ([Username]
           ,[Password]
           ,[Email]
           ,[DateCreated]
           ,[IdentityId])
     VALUES
           ('Two'
           ,'1HNeOiZeFu7gP1lxi5tdAwGcB9i2xR+Q2jpmbuwTqzU='
           ,'2@2.com'
           ,'2009-09-05 22:32:57.827'
           ,(SELECT MAX(Id)-2 FROM [CSS].[dbo].[Identity]))
GO
INSERT INTO [CSS].[dbo].[Login]
           ([Username]
           ,[Password]
           ,[Email]
           ,[DateCreated]
           ,[IdentityId])
     VALUES
           ('Three'
           ,'TgdAhWK+24tgzgXB3s/jrRa3IjCWfeAfZAt+Rym0n84='
           ,'3@3.com'
           ,'2009-09-05 22:32:57.827'
           ,(SELECT MAX(Id)-1 FROM [CSS].[dbo].[Identity]))
GO
INSERT INTO [CSS].[dbo].[Login]
           ([Username]
           ,[Password]
           ,[Email]
           ,[DateCreated]
           ,[IdentityId])
     VALUES
           ('Four'
           ,'SyJ3d9TdH8Ycb4hPSGQdArTRIdP9Moywi1Ux/Kzav4o='
           ,'4@4.com'
           ,'2009-09-05 22:32:57.827'
           ,(SELECT MAX(Id) FROM [CSS].[dbo].[Identity]))
GO
/* CREATE NEW ALIASES */
INSERT INTO [CSS].[dbo].[Alias]
           ([LoginId]
           ,[Callsign]
           ,[IsDefault]
           ,[DateCreated])
     VALUES
           ((SELECT MAX(Id)-3 FROM [CSS].[dbo].[Login])
           ,'One'
           ,1
           ,'2009-09-05 22:32:57.827')
GO
INSERT INTO [CSS].[dbo].[Alias]
           ([LoginId]
           ,[Callsign]
           ,[IsDefault]
           ,[DateCreated])
     VALUES
           ((SELECT MAX(Id)-2 FROM [CSS].[dbo].[Login])
           ,'Two'
           ,1
           ,'2009-09-05 22:32:57.827')
GO
INSERT INTO [CSS].[dbo].[Alias]
           ([LoginId]
           ,[Callsign]
           ,[IsDefault]
           ,[DateCreated])
     VALUES
           ((SELECT MAX(Id)-1 FROM [CSS].[dbo].[Login])
           ,'Three'
           ,1
           ,'2009-09-05 22:32:57.827')
GO
INSERT INTO [CSS].[dbo].[Alias]
           ([LoginId]
           ,[Callsign]
           ,[IsDefault]
           ,[DateCreated])
     VALUES
           ((SELECT MAX(Id) FROM [CSS].[dbo].[Login])
           ,'Four'
           ,1
           ,'2009-09-05 22:32:57.827')
GO
/* CREATE THE GROUPS */
INSERT INTO [CSS].[dbo].[Group]
           ([Name]
           ,[Tag]
           ,[IsSquad]
           ,[DateCreated])
     VALUES
           ('GROUP A'
           ,'GA'
           ,1
           ,'2009-09-05 22:32:57.827')
GO
INSERT INTO [CSS].[dbo].[Group]
           ([Name]
           ,[Tag]
           ,[IsSquad]
           ,[DateCreated])
     VALUES
           ('GROUP B'
           ,'GB'
           ,1
           ,'2009-09-05 22:32:57.827')
GO
/* CREATE MESSAGES */
INSERT INTO [CSS].[dbo].[GroupMessage]
           ([Message]
           ,[GroupId]
           ,[DateCreated]
           ,[DateToSend]
           ,[DateExpires])
     VALUES
           ('THIS MESSAGE IS FOR GROUP A WHICH CONSISTS OF "One" AND "Two"'
           ,(SELECT MAX(Id)-1 FROM [CSS].[dbo].[Group])
           ,'2009-09-05 22:32:57.827'
           ,'2009-09-05 22:32:57.827'
           ,'2009-09-05 22:32:57.827')
GO
INSERT INTO [CSS].[dbo].[GroupMessage]
           ([Message]
           ,[GroupId]
           ,[DateCreated]
           ,[DateToSend]
           ,[DateExpires])
     VALUES
           ('THIS MESSAGE IS FOR GROUP A WHICH CONSISTS OF "Three" AND "Four"'
           ,(SELECT MAX(Id) FROM [CSS].[dbo].[Group])
           ,'2009-09-05 22:32:57.827'
           ,'2009-09-05 22:32:57.827'
           ,'2009-09-05 22:32:57.827')
GO
/* PLACE MESSAGES IN GROUPS */
INSERT INTO [CSS].[dbo].[GroupMessage_Alias]
           ([GroupMessageId]
           ,[AliasId]
           ,[DateViewed])
     VALUES
           ((SELECT MAX(Id)-1 FROM [CSS].[dbo].[GroupMessage])
           ,(SELECT MAX(Id)-3 FROM [CSS].[dbo].[Alias])
           ,null)
GO
INSERT INTO [CSS].[dbo].[GroupMessage_Alias]
           ([GroupMessageId]
           ,[AliasId]
           ,[DateViewed])
     VALUES
           ((SELECT MAX(Id)-1 FROM [CSS].[dbo].[GroupMessage])
           ,(SELECT MAX(Id)-2 FROM [CSS].[dbo].[Alias])
           ,null)
GO
INSERT INTO [CSS].[dbo].[GroupMessage_Alias]
           ([GroupMessageId]
           ,[AliasId]
           ,[DateViewed])
     VALUES
           ((SELECT MAX(Id) FROM [CSS].[dbo].[GroupMessage])
           ,(SELECT MAX(Id)-1 FROM [CSS].[dbo].[Alias])
           ,null)
GO
INSERT INTO [CSS].[dbo].[GroupMessage_Alias]
           ([GroupMessageId]
           ,[AliasId]
           ,[DateViewed])
     VALUES
           ((SELECT MAX(Id) FROM [CSS].[dbo].[GroupMessage])
           ,(SELECT MAX(Id) FROM [CSS].[dbo].[Alias])
           ,null)
GO
/* NEW PERSONAL MESSAGES */
INSERT INTO [CSS].[dbo].[PersonalMessage]
           ([Message]
           ,[DateCreated]
           ,[DateToSend]
           ,[DateExpires]
           ,[LoginId]
           ,[DateViewed])
     VALUES
           ('THIS MESSAGE IS FOR "One"'
           ,'2009-08-31 21:04:45.593'
           ,'2009-08-31 21:04:45.593'
           ,'2011-08-31 21:04:45.593'
           ,(SELECT MAX(Id)-3 FROM [CSS].[dbo].[Login])
           ,null)
GO
INSERT INTO [CSS].[dbo].[PersonalMessage]
           ([Message]
           ,[DateCreated]
           ,[DateToSend]
           ,[DateExpires]
           ,[LoginId]
           ,[DateViewed])
     VALUES
           ('THIS MESSAGE IS FOR "Two"'
           ,'2009-08-31 21:04:45.593'
           ,'2009-08-31 21:04:45.593'
           ,'2011-08-31 21:04:45.593'
           ,(SELECT MAX(Id)-2 FROM [CSS].[dbo].[Login])
           ,null)
GO
INSERT INTO [CSS].[dbo].[PersonalMessage]
           ([Message]
           ,[DateCreated]
           ,[DateToSend]
           ,[DateExpires]
           ,[LoginId]
           ,[DateViewed])
     VALUES
           ('THIS MESSAGE IS FOR "Three"'
           ,'2009-08-31 21:04:45.593'
           ,'2009-08-31 21:04:45.593'
           ,'2011-08-31 21:04:45.593'
           ,(SELECT MAX(Id)-1 FROM [CSS].[dbo].[Login])
           ,null)
GO
INSERT INTO [CSS].[dbo].[PersonalMessage]
           ([Message]
           ,[DateCreated]
           ,[DateToSend]
           ,[DateExpires]
           ,[LoginId]
           ,[DateViewed])
     VALUES
           ('THIS MESSAGE IS FOR "Four"'
           ,'2009-08-31 21:04:45.593'
           ,'2009-08-31 21:04:45.593'
           ,'2011-08-31 21:04:45.593'
           ,(SELECT MAX(Id) FROM [CSS].[dbo].[Login])
           ,null)
GO