CREATE TABLE [dbo].[Permission] (
    [Id]          BIGINT        IDENTITY (1, 1) NOT NULL,
    [ParentId]    BIGINT        CONSTRAINT [DF_Permission_ParentId] DEFAULT ((0)) NOT NULL,
    [Name]        VARCHAR (100) NOT NULL,
    [Key]         VARCHAR (250) NOT NULL,
    [EntityName]  VARCHAR (150) NOT NULL,
    [Active]      BIT           NOT NULL,
    [Deleted]     BIT           NOT NULL,
    [Description] VARCHAR (200) NULL,
    [Order]       INT           CONSTRAINT [DF_Permission_Order] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_Permission] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);






GO
CREATE UNIQUE NONCLUSTERED INDEX [UNIX_Permission_Name_Key]
    ON [dbo].[Permission]([Name] ASC, [Key] ASC) WITH (FILLFACTOR = 90);


