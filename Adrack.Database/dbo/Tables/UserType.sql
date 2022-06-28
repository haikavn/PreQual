CREATE TABLE [dbo].[UserType] (
    [Id]           BIGINT       IDENTITY (1, 1) NOT NULL,
    [Name]         VARCHAR (50) NOT NULL,
    [Published]    BIT          NOT NULL,
    [Deleted]      BIT          NOT NULL,
    [DisplayOrder] INT          NOT NULL,
    CONSTRAINT [PK_UserType] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);










GO
CREATE UNIQUE NONCLUSTERED INDEX [UNIX_UserType_Name]
    ON [dbo].[UserType]([Name] ASC) WITH (FILLFACTOR = 90);

