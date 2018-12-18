CREATE TABLE [dbo].[SLAValues] (
    [ID]                   INT        IDENTITY (1, 1) NOT NULL,
    [SLAID]                INT        NULL,
    [ReportingDate]        DATETIME   NULL,
    [QuantityProcessed]    INT NULL,
    [QuantityOutsideofSLA] INT NULL,
    CONSTRAINT [PK_SLAValues] PRIMARY KEY CLUSTERED ([ID] ASC)
);

