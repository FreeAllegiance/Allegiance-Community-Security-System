/****** Object:  Table [dbo].[Error]    Script Date: 09/03/2009 18:22:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Error]') AND type in (N'U'))
DROP TABLE [dbo].[Error]
GO
/****** Object:  Table [dbo].[Error]    Script Date: 09/03/2009 18:22:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Error]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Error](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ExceptionType] [nvarchar](100) NOT NULL,
	[Message] [nvarchar](255) NOT NULL,
	[StackTrace] [ntext] NOT NULL,
	[InnerMessage] [nvarchar](255) NULL,
	[DateOccurred] [datetime] NOT NULL,
 CONSTRAINT [PK_Error] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Error_DateOccurred]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Error] ADD  CONSTRAINT [DF_Error_DateOccurred]  DEFAULT (getdate()) FOR [DateOccurred]
END

GO
/****** Object:  Table [dbo].[Log]    Script Date: 09/03/2009 18:25:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Log]') AND type in (N'U'))
DROP TABLE [dbo].[Log]
GO
/****** Object:  Table [dbo].[Log]    Script Date: 09/03/2009 18:25:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Log]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Log](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Type] [tinyint] NOT NULL,
	[Message] [nvarchar](255) NOT NULL,
	[DateOccurred] [datetime] NOT NULL,
 CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Log_DateOccurred]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Log] ADD  CONSTRAINT [DF_Log_DateOccurred]  DEFAULT (getdate()) FOR [DateOccurred]
END

GO
