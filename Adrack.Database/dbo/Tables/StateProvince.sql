CREATE TABLE [dbo].[StateProvince] (
    [Id]           BIGINT         IDENTITY (1, 1) NOT NULL,
    [CountryId]    BIGINT         NOT NULL,
    [Name]         NVARCHAR (100) NOT NULL,
    [Code]         VARCHAR (3)    NULL,
    [Published]    BIT            NOT NULL,
    [DisplayOrder] INT            NOT NULL,
    CONSTRAINT [PK_StateProvince] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_StateProvince_Country] FOREIGN KEY ([CountryId]) REFERENCES [dbo].[Country] ([Id])
);










GO
CREATE NONCLUSTERED INDEX [NIX_StateProvince_CountryId]
    ON [dbo].[StateProvince]([CountryId] ASC) WITH (FILLFACTOR = 90);

