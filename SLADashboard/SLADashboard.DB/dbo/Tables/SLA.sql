CREATE TABLE [dbo].[SLA] (
    [ID]          INT            IDENTITY (1, 1) NOT NULL,
    [ProfileID]   INT            NOT NULL,
    [Name]        NVARCHAR (MAX) NULL,
    [Description] NVARCHAR (MAX) NULL,
    [Target]      FLOAT (53)     NULL,
    [IsDeleted]   BIT            NULL,
    [DeletedUser] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_SLA] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SLA_Profile] FOREIGN KEY ([ProfileID]) REFERENCES [dbo].[Profile] ([ID])
);

