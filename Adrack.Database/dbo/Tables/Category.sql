CREATE TABLE [dbo].[Category] (
    [Id]           BIGINT        IDENTITY (1, 1) NOT NULL,
    [ParentId]     BIGINT        NOT NULL,
    [Name]         NVARCHAR (50) NOT NULL,
    [Published]    BIT           NOT NULL,
    [Deleted]      BIT           NOT NULL,
    [DisplayOrder] INT           NOT NULL,
    CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);



