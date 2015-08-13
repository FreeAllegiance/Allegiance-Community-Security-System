/****** Object:  Table [dbo].[InfractionType]    Script Date: 09/02/2009 17:23:20 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InfractionType]') AND type in (N'U'))
DROP TABLE [dbo].[InfractionType]
GO
/****** Object:  Table [dbo].[InfractionType]    Script Date: 09/02/2009 17:23:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InfractionType]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InfractionType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
	[FirstOffense] [int] NOT NULL,
	[Class] [tinyint] NOT NULL,
 CONSTRAINT [PK_InfractionType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/****** Object:  Table [dbo].[Ban]    Script Date: 09/02/2009 17:25:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ban]') AND type in (N'U'))
DROP TABLE [dbo].[Ban]
GO
/****** Object:  Table [dbo].[Ban]    Script Date: 09/02/2009 17:25:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ban]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ban](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdentityId] [int] NOT NULL,
	[BannedById] [int] NOT NULL,
	[Reason] [nvarchar](100) NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateExpires] [datetime] NULL,
	[InEffect] [bit] NOT NULL,
	[InfractionTypeId] [int] NULL,
 CONSTRAINT [PK_Ban] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ban_Identity]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ban]'))
ALTER TABLE [dbo].[Ban]  WITH CHECK ADD  CONSTRAINT [FK_Ban_Identity] FOREIGN KEY([IdentityId])
REFERENCES [dbo].[Identity] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ban_Identity]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ban]'))
ALTER TABLE [dbo].[Ban] CHECK CONSTRAINT [FK_Ban_Identity]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ban_InfractionType]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ban]'))
ALTER TABLE [dbo].[Ban]  WITH CHECK ADD  CONSTRAINT [FK_Ban_InfractionType] FOREIGN KEY([InfractionTypeId])
REFERENCES [dbo].[InfractionType] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ban_InfractionType]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ban]'))
ALTER TABLE [dbo].[Ban] CHECK CONSTRAINT [FK_Ban_InfractionType]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ban_Login]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ban]'))
ALTER TABLE [dbo].[Ban]  WITH CHECK ADD  CONSTRAINT [FK_Ban_Login] FOREIGN KEY([BannedById])
REFERENCES [dbo].[Login] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ban_Login]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ban]'))
ALTER TABLE [dbo].[Ban] CHECK CONSTRAINT [FK_Ban_Login]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ban_DateCreated]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ban] ADD  CONSTRAINT [DF_Ban_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ban_InEffect]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ban] ADD  CONSTRAINT [DF_Ban_InEffect]  DEFAULT ((1)) FOR [InEffect]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ban_InfractionCode]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ban] ADD  CONSTRAINT [DF_Ban_InfractionCode]  DEFAULT ((0)) FOR [InfractionTypeId]
END

GO
