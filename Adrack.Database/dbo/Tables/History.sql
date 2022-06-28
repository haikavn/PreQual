CREATE TABLE [dbo].[History] (
    [Id]       BIGINT        IDENTITY (1, 1) NOT NULL,
    [Date]     DATETIME      NULL,
    [Module]   VARCHAR (50)  NULL,
    [Action]   INT           NULL,
    [Entity]   VARCHAR (50)  NULL,
    [EntityID] BIGINT        NULL,
    [Data1]    VARCHAR (MAX) NULL,
    [Data2]    VARCHAR (MAX) NULL,
    [Note]     VARCHAR (MAX) NULL,
    [UserID]   BIGINT        NULL,
    CONSTRAINT [PK_History] PRIMARY KEY CLUSTERED ([Id] ASC)
);







