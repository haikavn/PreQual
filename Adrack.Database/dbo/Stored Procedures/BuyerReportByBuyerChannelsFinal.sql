CREATE PROCEDURE [dbo].[BuyerReportByBuyerChannelsFinal]
    @Start         DATETIME,
	@End           DATETIME,
	@Buyers        VARCHAR(MAX),
	@BuyerChannels VARCHAR(MAX),
	@AffiliateChannels VARCHAR(MAX),
	@Campaigns     VARCHAR(MAX)
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

    DECLARE @buyerChanelReport TABLE (BuyerId bigint not null, BuyerName varchar(50), BuyerChannelId bigint not null, BuyerChannelName varchar(50), 
						              TotalLeads int not null, SoldLeads int not null, RejectedLeads int not null, Cost decimal(18,2) not null,
									  AffiliatePrice decimal(18,2)  not null, AveragePrice decimal(18,2) not null, OrderNum int null, 
									  Loanedleads bigint not null, Redirected int not null, LastSoldDate datetime
									  )

    INSERT @buyerChanelReport EXEC dbo.BuyerReportByBuyerChannels @Start, @End,  @Buyers, @BuyerChannels, @AffiliateChannels, @Campaigns

	SELECT DISTINCT 
	b.BuyerId, b.BuyerName, b.BuyerChannelId, b.BuyerChannelName,
	SUM(b.TotalLeads) AS TotalLeads, SUM(b.SoldLeads) AS SoldLeads, SUM(b.loanedleads) AS LoanedLead, SUM(b.Cost) AS Cost, b.OrderNum AS [Rank],
	SUM(b.Cost - b.AffiliatePrice) AS Profit,  SUM(b.AveragePrice) AS AveragePrice, 
	CONVERT(decimal(18,2), CASE WHEN SUM(b.TotalLeads) > 0 THEN 
			ROUND( Convert( decimal, SUM(b.SoldLeads)) / Convert( decimal,  SUM(b.TotalLeads) ) * 100, 2) ELSE 0 END) AS AcceptRate,
	Sum(ld.Quantity) AS Cap

	FROM @buyerChanelReport b
	LEFT JOIN BuyerChannelSchedule ld ON b.BuyerChannelId = ld.BuyerChannelId AND ld.DayValue = DATEPART(dw,@Start) + 1 AND ld.Quantity >= 0
	GROUP BY b.BuyerChannelId, b.BuyerId, b.BuyerChannelName, b.BuyerName,b.OrderNum
			
	UNION ALL

	SELECT 0, null, 0, null, SUM(tb.TotalLeads), SUM(tb.SoldLeads), SUM(tb.loanedleads),
		   SUM(tb.Cost), 0, SUM(tb.Cost - tb.AffiliatePrice), SUM(tb.AveragePrice),
		   CONVERT(decimal(18,2), CASE WHEN SUM(tb.TotalLeads) > 0 THEN 
					ROUND( Convert( decimal, SUM(tb.SoldLeads)) / Convert( decimal, SUM(tb.TotalLeads)) * 100, 2) ELSE 0 END),
		   SUM(ld.Quantity)

	FROM @buyerChanelReport tb
	LEFT JOIN BuyerChannelSchedule ld ON tb.BuyerChannelId = ld.BuyerChannelId AND ld.DayValue = DATEPART(dw,@Start) + 1 AND ld.Quantity >= 0 

	END TRY
		BEGIN CATCH
			SELECT @SQLError_Severity = ERROR_SEVERITY(), 
					@SQLError_State    = ERROR_STATE(), 
					@SQLError_Message  = ERROR_MESSAGE()

			RAISERROR(@SQLError_Message, @SQLError_Severity, @SQLError_State)
		END CATCH
	END

	
