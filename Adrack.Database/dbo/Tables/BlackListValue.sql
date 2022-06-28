CREATE TABLE [dbo].[BlackListValue] (
    [Id]              BIGINT        IDENTITY (1, 1) NOT NULL,
    [BlackListTypeId] BIGINT        NULL,
    [Value]           VARCHAR (150) NULL,
    [Condition]       SMALLINT      NULL,
    CONSTRAINT [PK_BlackListValue] PRIMARY KEY CLUSTERED ([Id] ASC)
);






GO



GO


