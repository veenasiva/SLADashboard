USE [SLADashboard]
GO
/****** Object:  Table [dbo].[Client]    Script Date: 11/10/2018 11:06:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Client]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Client](
		[ID] [int] IDENTITY(1,1) NOT NULL,
		[Description] [nvarchar](max) NOT NULL,
	 CONSTRAINT [PK_dbo.Client] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Profile]    Script Date: 11/10/2018 11:06:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Profile]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Profile](
		[ID] [int] IDENTITY(1,1) NOT NULL,
		[ClientID] [int] NOT NULL,
		[Name] [nvarchar](max) NULL,
		[Description] [nvarchar](max) NULL,
		[SLAIDPrefix] [nvarchar](max) NULL,
	 CONSTRAINT [PK_dbo.Profile] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	ALTER TABLE [dbo].[Profile]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Profile_dbo.Client_ClientID] FOREIGN KEY([ClientID])
	REFERENCES [dbo].[Client] ([ID])
	ON DELETE CASCADE

	ALTER TABLE [dbo].[Profile] CHECK CONSTRAINT [FK_dbo.Profile_dbo.Client_ClientID]

END

GO
/****** Object:  Table [dbo].[Settings]    Script Date: 11/10/2018 11:06:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Settings]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Settings](
		[Name] [nvarchar](128) NOT NULL,
		[Value] [nvarchar](max) NULL,
		[Comments] [nvarchar](max) NULL,
	 CONSTRAINT [PK_dbo.Settings] PRIMARY KEY CLUSTERED 
	(
		[Name] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

GO
/****** Object:  Table [dbo].[SLA]    Script Date: 11/10/2018 11:06:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SLA]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[SLA](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProfileID] [int] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Target] [float] NULL,
	[IsDeleted] [bit] NULL,
	[DeletedUser] [nvarchar](max) NULL,
	 CONSTRAINT [PK_SLA] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	ALTER TABLE [dbo].[SLA]  WITH CHECK ADD  CONSTRAINT [FK_SLA_Profile] FOREIGN KEY([ProfileID])
	REFERENCES [dbo].[Profile] ([ID])

	ALTER TABLE [dbo].[SLA] CHECK CONSTRAINT [FK_SLA_Profile]
END

/****** Object:  Table [dbo].[SLAValues]    Script Date: 11/10/2018 11:06:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SLAValues]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[SLAValues](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SLAID] [int] NOT NULL,
	[ReportingDate] [datetime] NULL,
	[QuantityProcessed] [int] NULL,
	[QuantityOutsideofSLA] [int] NULL,
	[UpdatedBy] [nvarchar](150) NULL,
	[UpdatedDate] [datetime] NULL,
	 CONSTRAINT [PK_SLAValues] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END

GO
/****** Object:  Table [dbo].[SystemConfig]    Script Date: 11/10/2018 11:06:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Systemconfig]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[SystemConfig](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](max) NULL,
	[Mode] [int] NOT NULL,
	 CONSTRAINT [PK_dbo.SystemConfig] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

GO

-------Inititalis Data in Settings table-----------------------------
IF @@SERVERNAME = 'DECTST-SQL2014' 
BEGIN
	MERGE INTO  [dbo].[Settings] as target
	USING (VALUES 
		  (N'AdminGroup', N'RTSAdministrator', NULL)
		, (N'OperatorGroup', N'RTSOperator', NULL)
		, (N'ReportPath', N'\\DECTST-FS1\Clients\SLADashboard\Reports', 'Default path to save reports')
	) AS Source ([Name], [Value], [Comments])
		ON target.[Name] = Source.[Name]
	WHEN MATCHED AND ISNULL(target.[Value], '') != ISNULL(source.[Value], '') OR ISNULL(target.[Comments], '') != ISNULL(source.[Comments], '') THEN 
	Update SET [Value] = source.[Value], [Comments] = source.[Comments]
	WHEN NOT MATCHED THEN 
	INSERT ([Name], [Value], [Comments])
	VALUES (source.[Name], source. [Value], source.[Comments]);
END