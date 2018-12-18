CREATE TABLE [dbo].[Settings] (
    [Name]     NVARCHAR (128) NOT NULL,
    [Value]    NVARCHAR (MAX) NULL,
    [Comments] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.Settings] PRIMARY KEY CLUSTERED ([Name] ASC)
);

