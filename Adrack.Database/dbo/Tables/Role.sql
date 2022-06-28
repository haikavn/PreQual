CREATE TABLE [dbo].[Role] (
    [Id]          BIGINT        IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (100) NOT NULL,
    [Key]         VARCHAR (250) NOT NULL,
    [Active]      BIT           NOT NULL,
    [Deleted]     BIT           NOT NULL,
    [BuiltIn]     BIT           NOT NULL,
    [Description] VARCHAR (200) NULL,
    [UserTypeId]  BIGINT        NULL,
    CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);




GO

