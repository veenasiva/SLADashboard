CREATE TABLE [dbo].[SystemConfig] (
    [ID]       INT            IDENTITY (1, 1) NOT NULL,
    [UserName] NVARCHAR (MAX) NULL,
    [Mode]     INT            NOT NULL,
    CONSTRAINT [PK_dbo.SystemConfig] PRIMARY KEY CLUSTERED ([ID] ASC)
);

