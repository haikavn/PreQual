CREATE TABLE [dbo].[GeoLocationIP] (
    [Id]            BIGINT          IDENTITY (1, 1) NOT NULL,
    [GeoId]         BIGINT          NULL,
    [IpFrom]        BIGINT          NOT NULL,
    [IpTo]          BIGINT          NOT NULL,
    [Latitude]      DECIMAL (18, 4) NULL,
    [Longitude]     DECIMAL (18, 4) NULL,
    [ZipPostalCode] VARCHAR (15)    NULL,
    CONSTRAINT [PK_GeoLocationIP] PRIMARY KEY CLUSTERED ([Id] ASC)
);






GO
CREATE NONCLUSTERED INDEX [NIX_GeoLocationIP_IpFrom_IpTo_INCLUDE]
    ON [dbo].[GeoLocationIP]([IpFrom] ASC, [IpTo] ASC)
    INCLUDE([GeoId], [Latitude], [Longitude], [ZipPostalCode]);


GO



GO
CREATE NONCLUSTERED INDEX [NIX_GeoLocationIP_GeoId_IpFrom_IpTo_INCLUDE]
    ON [dbo].[GeoLocationIP]([GeoId] ASC, [IpFrom] ASC, [IpTo] ASC)
    INCLUDE([Latitude], [Longitude], [ZipPostalCode]);

