CREATE TABLE [dbo].[LogX] (
    [Id]              BIGINT          NOT NULL,
    [UserId]          BIGINT          NULL,
    [LogLevelId]      INT             NOT NULL,
    [Message]         NVARCHAR (2000) NOT NULL,
    [Exception]       NVARCHAR (MAX)  NULL,
    [IpAddress]       VARCHAR (15)    NULL,
    [PageUrl]         NVARCHAR (1000) NULL,
    [ReferrerUrl]     NVARCHAR (1000) NULL,
    [CreatedOn]       DATETIME        NOT NULL,
    [CreateArchiveDT] DATETIME        NOT NULL
);

