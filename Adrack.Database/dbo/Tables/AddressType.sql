CREATE TABLE [dbo].[AddressType] (
    [Id]           BIGINT       IDENTITY (1, 1) NOT NULL,
    [Name]         VARCHAR (50) NOT NULL,
    [Published]    BIT          NOT NULL,
    [Deleted]      BIT          NOT NULL,
    [DisplayOrder] INT          NOT NULL,
    CONSTRAINT [PK_AddressType] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);





