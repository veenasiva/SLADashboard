CREATE TABLE [dbo].[Profile] (
    [ID]          INT            IDENTITY (1, 1) NOT NULL,
    [ClientID]    INT            NOT NULL,
    [Name]        NVARCHAR (MAX) NULL,
    [Description] NVARCHAR (MAX) NULL,
    [SLAIDPrefix] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.Profile] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_dbo.Profile_dbo.Client_ClientID] FOREIGN KEY ([ClientID]) REFERENCES [dbo].[Client] ([ID]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ClientID]
    ON [dbo].[Profile]([ClientID] ASC);

