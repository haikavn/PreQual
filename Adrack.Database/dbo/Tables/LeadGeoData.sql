CREATE TABLE [dbo].[LeadGeoData] (
    [Id]          BIGINT       IDENTITY (1, 1) NOT NULL,
    [LeadId]      BIGINT       NULL,
    [CountryCode] VARCHAR (8)  NULL,
    [CountryName] VARCHAR (50) NULL,
    [RegionName]  VARCHAR (50) NULL,
    [CityName]    VARCHAR (50) NULL,
    [Latitude]    VARCHAR (50) NULL,
    [Longitude]   VARCHAR (50) NULL,
    [ZipCode]     VARCHAR (50) NULL,
    [TimeZone]    VARCHAR (50) NULL,
    [AreaCode]    VARCHAR (50) NULL,
    CONSTRAINT [PK_LeadGeoData] PRIMARY KEY CLUSTERED ([Id] ASC)
);

