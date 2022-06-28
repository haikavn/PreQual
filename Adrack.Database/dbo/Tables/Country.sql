CREATE TABLE [dbo].[Country] (
    [Id]               BIGINT         IDENTITY (1, 1) NOT NULL,
    [Name]             NVARCHAR (100) NOT NULL,
    [TwoLetteroCode]   VARCHAR (2)    NOT NULL,
    [ThreeLetteroCode] VARCHAR (3)    NOT NULL,
    [NumericCode]      INT            NOT NULL,
    [Published]        BIT            NOT NULL,
    [DisplayOrder]     INT            NOT NULL,
    CONSTRAINT [PK_Country] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);













