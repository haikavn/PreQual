CREATE TABLE [dbo].[LeadMainReportDaySubIds] (
    [Id]       BIGINT        IDENTITY (1, 1) NOT NULL,
    [Created]  DATE          NULL,
    [Quantity] INT           NULL,
    [Status]   SMALLINT      NULL,
    [SubId]    VARCHAR (300) NULL,
    CONSTRAINT [PK_LeadMainReportDaySubIds] PRIMARY KEY CLUSTERED ([Id] ASC)
);

