CREATE TABLE [dbo].[CustomBlackListValue] (
    [Id]            BIGINT        IDENTITY (1, 1) NOT NULL,
    [ChannelId]     BIGINT        NULL,
    [ChannelType]   SMALLINT      NULL,
    [Value]         VARCHAR (MAX) NULL,
    [TemplateFieldId] BIGINT NULL, 
    CONSTRAINT [PK_CustomBlackListValue] PRIMARY KEY CLUSTERED ([Id] ASC)
);

