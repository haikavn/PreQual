CREATE PROCEDURE [dbo].[ReportAffiliatesByStatesFinal]
(
    @start DateTime,
	@end DateTime,
	@affiliates varchar(MAX),
	@affiliateChannels varchar(MAX)
)
AS
BEGIN

-- =================================================================================================
-- SQL Common
-- =================================================================================================
	DECLARE @SQLError_Severity INT
	DECLARE @SQLError_State    INT
	DECLARE @SQLError_Message  NVARCHAR(2000)

-- =================================================================================================
-- Process Script
-- =================================================================================================
 	BEGIN TRY

    DECLARE @affiliateByStateReport TABLE (RowNum int not null, AffiliateId bigint not null, AffiliateName varchar(50), 
										   [State] varchar(50), SoldLeads int not null, Debet decimal(18,2),
										   Credit decimal(18,2), Redirected int not null, TotalLeads bigint not null)

    INSERT @affiliateByStateReport EXEC dbo.ReportAffiliatesByStates @Start, @End, @affiliates, @affiliateChannels;

	SELECT   a.[State], SUM(a.TotalLeads) AS TotalLeads, SUM(a.SoldLeads) AS SoldLeads, SUM(a.Debet) AS Debet,
			 SUM(a.Credit) AS Credit, SUM(CASE WHEN a.Redirected > a.SoldLeads THEN a.SoldLeads ELSE a.Redirected END) AS Redirected, 
			 SUM(a.Debet - a.Credit) AS Profit,
			 CONVERT(decimal(18,2), CASE WHEN SUM(a.TotalLeads) > 0 THEN 
				   ROUND( Convert( decimal, SUM(a.SoldLeads)) / Convert( decimal, SUM(a.TotalLeads)) * 100, 2) ELSE 0 END) AS AcceptRate,
			 CONVERT(decimal(18,2), CASE WHEN SUM(a.SoldLeads) > 0 THEN 
				   ROUND( Convert( decimal, SUM(CASE WHEN a.Redirected > a.SoldLeads THEN a.SoldLeads ELSE a.Redirected END)) 
				   / Convert( decimal, SUM(a.SoldLeads)) * 100, 2) ELSE 0 END) AS RedirectedRate,
			 CONVERT(decimal(18,2), CASE WHEN SUM(a.TotalLeads) > 0 THEN 
				   ROUND( Convert( decimal, SUM(a.Debet - a.Credit)) / Convert( decimal, SUM(a.TotalLeads)), 2) ELSE 0 END) AS EPL,
			 CONVERT(decimal(18,2), CASE WHEN SUM(a.SoldLeads) > 0 THEN 
				   ROUND( Convert( decimal, SUM(a.Debet - a.Credit)) / Convert( decimal, SUM(a.SoldLeads)), 2) ELSE 0 END) AS EPA

	  FROM @affiliateByStateReport a
	  GROUP BY a.[State]

	  UNION ALL

	  SELECT null, SUM(a.TotalLeads), SUM(a.SoldLeads), SUM(a.Debet),
		     SUM(a.Credit), SUM(CASE WHEN a.Redirected > a.SoldLeads THEN a.SoldLeads ELSE a.Redirected END), SUM(a.Debet - a.Credit),
		     CONVERT(decimal(18,2), CASE WHEN SUM(a.TotalLeads) > 0 THEN 
				  ROUND( Convert( decimal, SUM(a.SoldLeads)) / Convert( decimal, SUM(a.TotalLeads)) * 100, 2) ELSE 0 END),
		     CONVERT(decimal(18,2), CASE WHEN SUM(a.SoldLeads) > 0 THEN 
				  ROUND( Convert( decimal, SUM(CASE WHEN a.Redirected > a.SoldLeads THEN a.SoldLeads ELSE a.Redirected END)) 
				  / Convert( decimal, SUM(a.SoldLeads)) * 100, 2) ELSE 0 END),
		     CONVERT(decimal(18,2), CASE WHEN SUM(a.TotalLeads) > 0 THEN 
				  ROUND( Convert( decimal, SUM(a.Debet - a.Credit)) / Convert( decimal, SUM(a.TotalLeads)), 2) ELSE 0 END),
		     CONVERT(decimal(18,2), CASE WHEN SUM(a.SoldLeads) > 0 THEN 
				  ROUND( Convert( decimal, SUM(a.Debet - a.Credit)) / Convert( decimal, SUM(a.SoldLeads)), 2) ELSE 0 END)
	 FROM @affiliateByStateReport a
END TRY
		BEGIN CATCH
			SELECT @SQLError_Severity = ERROR_SEVERITY(), 
				   @SQLError_State    = ERROR_STATE(), 
				   @SQLError_Message  = ERROR_MESSAGE()

			RAISERROR(@SQLError_Message, @SQLError_Severity, @SQLError_State)
		END CATCH
END
