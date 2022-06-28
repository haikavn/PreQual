CREATE TABLE [dbo].[GlobalAttribute] (
    [Id]       BIGINT          IDENTITY (1, 1) NOT NULL,
    [EntityId] BIGINT          NOT NULL,
    [KeyGroup] VARCHAR (250)   NOT NULL,
    [Key]      VARCHAR (250)   NOT NULL,
    [Value]    NVARCHAR (1000) NOT NULL,
    CONSTRAINT [PK_GlobalAttribute] PRIMARY KEY CLUSTERED ([Id] ASC)
);





