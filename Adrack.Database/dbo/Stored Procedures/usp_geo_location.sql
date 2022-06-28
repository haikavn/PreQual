-- =============================================
-- Developer: Zarzand Papikyan
-- Description:	GEO Location
-- ---------------------------------------------
-- Execute: EXECUTE [dbo].[usp_geo_location] @IpAddress = '24.205.53.102' -- 24.205.53.102, 24.205.52.119, 69.169.43.8, 37.157.215.99, 82.199.220.18, 82.199.201.107, 78.109.76.78
-- =============================================

CREATE PROCEDURE [dbo].[usp_geo_location]
	@IpAddress    NVARCHAR(45)
AS
BEGIN
-- =================================================================================================
-- SQL Script Setup
-- =================================================================================================
	SET NOCOUNT ON

-- =================================================================================================
-- SQL Common
-- =================================================================================================
	DECLARE @SQLCommand        NVARCHAR(4000)
	DECLARE @SQLErrorMessage   NVARCHAR(4000)
	-----------------------------------------
	DECLARE @SQLError_Severity INT
	DECLARE @SQLError_State    INT
	DECLARE @SQLError_Message  NVARCHAR(2000)
	-----------------------------------------
	DECLARE @IpAddressNumber   BIGINT

-- =================================================================================================
-- Validation 
-- =================================================================================================
	---- Check If IP v4 ----
	IF (PARSENAME(@IpAddress, 1) NOT LIKE '%[^0-9]%')
		BEGIN
			SET @IpAddressNumber = (16777216 * PARSENAME(@IpAddress, 4) + 
			                        65536    * PARSENAME(@IpAddress, 3) + 
									256      * PARSENAME(@IpAddress, 2) + 
									1        * PARSENAME(@IpAddress, 1))
		END
	ELSE
		BEGIN
			SET @IpAddressNumber = (1099511627776 * PARSENAME(@IpAddress, 6) + 
			                        4294967296    * PARSENAME(@IpAddress, 5) + 
									16777216      * PARSENAME(@IpAddress, 4) + 
									65536         * PARSENAME(@IpAddress, 3) + 
									256           * PARSENAME(@IpAddress, 2) + 
									1             * PARSENAME(@IpAddress, 1))
		END	

-- =================================================================================================
-- Process Script
-- =================================================================================================
 	BEGIN TRY
		SELECT @IpAddress AS [IpAddress],
		       a.[GeoId],
			   a.[IpFrom],
			   a.[IpTo],
			   a.[Latitude],
			   a.[Longitude],
			   a.[ZipPostalCode],
			   --b.[GeoId],
			   b.[CountryCode],
			   b.[CountryName],
			   b.[SubdivisionCode],
			   b.[SubdivisionName],
			   b.[CityName],
			   b.[TimeZone]
		FROM [dbo].[GeoLocationIP] a WITH(NOLOCK) 
		  INNER JOIN [dbo].[GeoLocation] b WITH(NOLOCK)
			ON a.[GeoId] = b.[GeoId]
		WHERE a.[IpFrom] <= @IpAddressNumber AND
			  a.[IpTo]   >= @IpAddressNumber
	END TRY
 
	BEGIN CATCH
		SELECT @SQLError_Severity = ERROR_SEVERITY(), 
		       @SQLError_State    = ERROR_STATE(), 
			   @SQLError_Message  = ERROR_MESSAGE()

		RAISERROR(@SQLError_Message, @SQLError_Severity, @SQLError_State)
	END CATCH
END