CREATE TABLE [dbo].[BuyerChannelTemplateMatching] (
    [Id]                     BIGINT        IDENTITY (1, 1) NOT NULL,
    [BuyerChannelTemplateId] BIGINT        NULL,
    [InputValue]             VARCHAR (150) NULL,
    [OutputValue]            VARCHAR (150) NULL,
    CONSTRAINT [PK_BuyerChannelTemplateMatching] PRIMARY KEY CLUSTERED ([Id] ASC)
);

