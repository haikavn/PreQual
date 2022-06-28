CREATE TABLE [dbo].[GeoLocation] (
    [Id]              BIGINT        IDENTITY (1, 1) NOT NULL,
    [GeoId]           BIGINT        NULL,
    [CountryCode]     VARCHAR (3)   NULL,
    [CountryName]     VARCHAR (50)  NULL,
    [SubdivisionCode] VARCHAR (20)  NULL,
    [SubdivisionName] VARCHAR (50)  NULL,
    [CityName]        VARCHAR (100) NULL,
    [TimeZone]        VARCHAR (40)  NULL,
    CONSTRAINT [PK_GeoLocation] PRIMARY KEY CLUSTERED ([Id] ASC)
);




GO
CREATE NONCLUSTERED INDEX [NIX_GeoLocation_GeoId]
    ON [dbo].[GeoLocation]([GeoId] ASC);

