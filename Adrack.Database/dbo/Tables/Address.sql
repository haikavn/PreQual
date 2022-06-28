CREATE TABLE [dbo].[Address] (
    [Id]              BIGINT         IDENTITY (1, 1) NOT NULL,
    [AddressTypeId]   BIGINT         NOT NULL,
    [UserId]          BIGINT         NOT NULL,
    [CountryId]       BIGINT         NULL,
    [StateProvinceId] BIGINT         NULL,
    [FirstName]       NVARCHAR (50)  NULL,
    [LastName]        NVARCHAR (50)  NULL,
    [AddressLine1]    NVARCHAR (150) NULL,
    [AddressLine2]    NVARCHAR (150) NULL,
    [City]            NVARCHAR (100) NULL,
    [ZipPostalCode]   VARCHAR (20)   NULL,
    [Telephone]       BIGINT         NULL,
    [Default]         BIT            NULL,
    CONSTRAINT [PK_Address] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_Address_AddressType] FOREIGN KEY ([AddressTypeId]) REFERENCES [dbo].[AddressType] ([Id]),
    CONSTRAINT [FK_Address_Country] FOREIGN KEY ([CountryId]) REFERENCES [dbo].[Country] ([Id]),
    CONSTRAINT [FK_Address_StateProvince] FOREIGN KEY ([StateProvinceId]) REFERENCES [dbo].[StateProvince] ([Id])
);






















GO
CREATE NONCLUSTERED INDEX [NIX_Address_StateProvinceId]
    ON [dbo].[Address]([StateProvinceId] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [NIX_Address_CountryId]
    ON [dbo].[Address]([CountryId] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [NIX_Address_AddressTypeId]
    ON [dbo].[Address]([AddressTypeId] ASC) WITH (FILLFACTOR = 90);

