CREATE TABLE [dbo].[GeoLocationCity_Temp] (
    [geoname_id]             VARCHAR (50)  NULL,
    [locale_code]            VARCHAR (50)  NULL,
    [continent_code]         VARCHAR (50)  NULL,
    [continent_name]         VARCHAR (50)  NULL,
    [country_iso_code]       VARCHAR (50)  NULL,
    [country_name]           VARCHAR (50)  NULL,
    [subdivision_1_iso_code] VARCHAR (50)  NULL,
    [subdivision_1_name]     VARCHAR (200) NULL,
    [subdivision_2_iso_code] VARCHAR (50)  NULL,
    [subdivision_2_name]     VARCHAR (50)  NULL,
    [city_name]              VARCHAR (200) NULL,
    [metro_code]             VARCHAR (50)  NULL,
    [time_zone]              VARCHAR (50)  NULL
);

