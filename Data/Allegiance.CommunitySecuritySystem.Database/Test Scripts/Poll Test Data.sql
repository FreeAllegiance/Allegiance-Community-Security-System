DELETE FROM  [CSS].[dbo].[PollVote] WHERE 1=1
GO

DELETE FROM  [CSS].[dbo].[PollOption] WHERE 1=1
GO

DELETE FROM  [CSS].[dbo].[Poll] WHERE 1=1
GO

 
 INSERT INTO [CSS].[dbo].[Poll] ([Question],[DateCreated],[DateExpires])
     VALUES ('Does TB Rock?','2009-9-1 04:13:54','2009-11-1 04:13:54')
GO
 
 INSERT INTO [CSS].[dbo].[PollOption] ([PollId],[Option])
     VALUES ((SELECT MAX(Id) FROM [CSS].[dbo].[Poll]),'Yes!')
GO

INSERT INTO [CSS].[dbo].[PollOption] ([PollId],[Option])
     VALUES ((SELECT MAX(Id) FROM [CSS].[dbo].[Poll]),'Hell Yes!')
GO 

INSERT INTO [CSS].[dbo].[Poll] ([Question],[DateCreated],[DateExpires])
     VALUES ('This is a question?','2009-9-2 07:13:54','2009-11-2 12:13:54')
GO
 
 INSERT INTO [CSS].[dbo].[PollOption] ([PollId],[Option])
     VALUES ((SELECT MAX(Id) FROM [CSS].[dbo].[Poll]),'Could be...')
GO

INSERT INTO [CSS].[dbo].[PollOption] ([PollId],[Option])
     VALUES ((SELECT MAX(Id) FROM [CSS].[dbo].[Poll]),'I think so.')
GO


