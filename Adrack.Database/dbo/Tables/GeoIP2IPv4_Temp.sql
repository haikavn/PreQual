CREATE TABLE [dbo].[GeoIP2IPv4_Temp] (
    [network_start_integer]          VARCHAR (50) NULL,
    [network_last_integer]           VARCHAR (50) NULL,
    [geoname_id]                     VARCHAR (50) NULL,
    [registered_country_geoname_id]  VARCHAR (50) NULL,
    [represented_country_geoname_id] VARCHAR (50) NULL,
    [is_anonymous_proxy]             VARCHAR (50) NULL,
    [is_satellite_provider]          VARCHAR (50) NULL,
    [postal_code]                    VARCHAR (50) NULL,
    [latitude]                       VARCHAR (50) NULL,
    [longitude]                      VARCHAR (50) NULL,
    [accuracy_radius]                VARCHAR (50) NULL
);

