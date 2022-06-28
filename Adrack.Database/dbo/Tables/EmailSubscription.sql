CREATE TABLE [dbo].[EmailSubscription] (
    [Id]        BIGINT        IDENTITY (1, 1) NOT NULL,
    [GuId]      VARCHAR (36)  NULL,
    [Email]     VARCHAR (100) NOT NULL,
    [Active]    BIT           NOT NULL,
    [CreatedOn] DATETIME      CONSTRAINT [DF_EmailSubscription_CreatedOn] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_EmailSubscription] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);







