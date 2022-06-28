CREATE TABLE [dbo].[LogX] (
    [Id]              BIGINT          IDENTITY (1, 1) NOT NULL,
    [UserId]          BIGINT          NULL,
    [LogLevelId]      INT             NOT NULL,
    [Message]         NVARCHAR (2000) NOT NULL,
    [Exception]       NVARCHAR (MAX)  NULL,
    [IpAddress]       VARCHAR (15)    NULL,
    [PageUrl]         NVARCHAR (1000) NULL,
    [ReferrerUrl]     NVARCHAR (1000) NULL,
    [CreatedOn]       DATETIME        NOT NULL,
    [CreateArchiveDT] DATETIME        CONSTRAINT [DF_LogX_CreateArchiveDT] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_LogX] PRIMARY KEY CLUSTERED ([Id] ASC)
);

