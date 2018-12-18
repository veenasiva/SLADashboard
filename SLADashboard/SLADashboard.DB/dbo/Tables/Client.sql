CREATE TABLE [dbo].[Client] (
    [ID]          INT            IDENTITY (1, 1) NOT NULL,
    [Description] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_dbo.Client] PRIMARY KEY CLUSTERED ([ID] ASC)
);

