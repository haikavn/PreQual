
-- =============================================
-- Developer: Zarzand Papikyan
-- Description:	Report Totals Buyer
-- ---------------------------------------------
-- Execute: EXECUTE [dbo].[ReportTotalsBuyer]
-- =============================================
CREATE PROCEDURE [dbo].[ReportTotalsBuyer]
	 @start DATETIME
	,@end DATETIME
	,@parentid BIGINT
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
	----------------------------------------
	DECLARE @SQLError_Severity INT
	DECLARE @SQLError_State    INT
	DECLARE @SQLError_Message  NVARCHAR(2000)
	----------------------------------------
	DECLARE @now DATETIME = getdate();--convert(datetimeoffset, getdate()) AT TIME ZONE (select top 1 Value from setting where [Key]='TimeZone');
	DECLARE @start2 DATETIME = @start;
	DECLARE @end2 DATETIME = convert(NVARCHAR(10), cast(@end AS DATE), 10) + ' 23:59:59';
	
	-- Insert statements for procedure here
	DECLARE @starttime TIME = '00:00:00';
	DECLARE @endtime TIME = '23:59:59';
	
	--declare @start as DateTime = convert(nvarchar(10), cast(@now as Date), 10) + ' 00:00:00';
	--declare @end as DateTime = convert(nvarchar(10), cast(@now as Date), 10) + ' 23:59:59';
	
	DECLARE @receivedleads_today INT = 0;
	DECLARE @totalleads_today INT = 0;
	DECLARE @soldleads_today INT = 0;
	DECLARE @rejectedleads_today INT = 0;
	DECLARE @debit_today MONEY = 0;
	DECLARE @profit_today MONEY = 0;
-- =================================================================================================
-- Process Script
-- =================================================================================================
 	BEGIN TRY
		SELECT @receivedleads_today = ISNULL(count(DISTINCT LeadId), 0)
		FROM LeadMainReport
		WHERE CampaignType = 0
			AND Created BETWEEN @start
				AND @end
			AND (
				@parentid = 0
				OR (
					@parentid > 0
					AND BuyerId = @parentid
					)
				OR (
					@parentid < 0
					AND AffiliateId = abs(@parentid)
					)
				);

		SELECT @totalleads_today = ISNULL(count(LeadId), 0)
		FROM LeadMainReport
		WHERE CampaignType = 0
			AND Created BETWEEN @start
				AND @end
			AND (
				@parentid = 0
				OR (
					@parentid > 0
					AND BuyerId = @parentid
					)
				OR (
					@parentid < 0
					AND AffiliateId = abs(@parentid)
					)
				);

		SELECT @soldleads_today = ISNULL(count(LeadId), 0)
			,@debit_today = ISNULL(sum(BuyerPrice), 0)
			,@profit_today = ISNULL(sum(BuyerPrice - AffiliatePrice), 0)
		FROM LeadMainReport
		WHERE CampaignType = 0
			AND Created BETWEEN @start
				AND @end
			AND Status = 1
			AND (
				@parentid = 0
				OR (
					@parentid > 0
					AND BuyerId = @parentid
					)
				OR (
					@parentid < 0
					AND AffiliateId = abs(@parentid)
					)
				);

		SELECT @rejectedleads_today = ISNULL(count(LeadId), 0)
		FROM LeadMainReport
		WHERE CampaignType = 0
			AND Created BETWEEN @start
				AND @end
			AND Status = 3
			AND (
				@parentid = 0
				OR (
					@parentid > 0
					AND BuyerId = @parentid
					)
				OR (
					@parentid < 0
					AND AffiliateId = abs(@parentid)
					)
				);

		--select @credit_today = ISNULL(sum(aprice), 0) from r_main where Created between @start and @now  and status = 1;
		----- yesterday -----
		SET @start = DATEADD(day, - 1, @start2);
		SET @end = DATEADD(day, - 1, @end2);

		PRINT @start;
		PRINT @end;

		DECLARE @receivedleads_yesterday INT = 0;
		DECLARE @totalleads_yesterday INT = 0;
		DECLARE @soldleads_yesterday INT = 0;
		DECLARE @rejectedleads_yesterday INT = 0;
		DECLARE @debit_yesterday MONEY = 0;
		DECLARE @profit_yesterday MONEY = 0;

		SELECT @receivedleads_yesterday = ISNULL(count(DISTINCT LeadId), 0)
		FROM LeadMainReport
		WHERE CampaignType = 0
			AND Created BETWEEN @start
				AND @end
			AND (
				@parentid = 0
				OR (
					@parentid > 0
					AND BuyerId = @parentid
					)
				OR (
					@parentid < 0
					AND AffiliateId = abs(@parentid)
					)
				);

		SELECT @totalleads_yesterday = ISNULL(count(LeadId), 0)
		FROM LeadMainReport
		WHERE CampaignType = 0
			AND Created BETWEEN @start
				AND @end
			AND (
				@parentid = 0
				OR (
					@parentid > 0
					AND BuyerId = @parentid
					)
				OR (
					@parentid < 0
					AND AffiliateId = abs(@parentid)
					)
				);

		SELECT @soldleads_yesterday = ISNULL(count(LeadId), 0)
			,@debit_yesterday = ISNULL(sum(BuyerPrice), 0)
			,@profit_yesterday = ISNULL(sum(BuyerPrice - AffiliatePrice), 0)
		FROM LeadMainReport
		WHERE CampaignType = 0
			AND Created BETWEEN @start
				AND @end
			AND Status = 1
			AND (
				@parentid = 0
				OR (
					@parentid > 0
					AND BuyerId = @parentid
					)
				OR (
					@parentid < 0
					AND AffiliateId = abs(@parentid)
					)
				);

		SELECT @rejectedleads_yesterday = ISNULL(count(LeadId), 0)
		FROM LeadMainReport
		WHERE CampaignType = 0
			AND Created BETWEEN @start
				AND @end
			AND Status = 3
			AND (
				@parentid = 0
				OR (
					@parentid > 0
					AND BuyerId = @parentid
					)
				OR (
					@parentid < 0
					AND AffiliateId = abs(@parentid)
					)
				);

		--select @credit_yesterday = ISNULL(sum(aprice), 0) from r_main where Created between @start and @end  and status = 1;
		----- 7 days -----
		SET @start = DATEADD(day, - 7, @start2);
		SET @end = DATEADD(day, - 7, @end2);

		DECLARE @receivedleads_7days INT = 0;
		DECLARE @totalleads_7days INT = 0;
		DECLARE @soldleads_7days INT = 0;
		DECLARE @rejectedleads_7days INT = 0;
		DECLARE @debit_7days MONEY = 0;
		DECLARE @profit_7days MONEY = 0;

		SELECT @receivedleads_7days = ISNULL(count(DISTINCT LeadId), 0)
		FROM LeadMainReport
		WHERE CampaignType = 0
			AND Created BETWEEN @start
				AND @end
			AND (
				@parentid = 0
				OR (
					@parentid > 0
					AND BuyerId = @parentid
					)
				OR (
					@parentid < 0
					AND AffiliateId = abs(@parentid)
					)
				);

		SELECT @totalleads_7days = ISNULL(count(LeadId), 0)
		FROM LeadMainReport
		WHERE CampaignType = 0
			AND Created BETWEEN @start
				AND @end
			AND (
				@parentid = 0
				OR (
					@parentid > 0
					AND BuyerId = @parentid
					)
				OR (
					@parentid < 0
					AND AffiliateId = abs(@parentid)
					)
				);

		SELECT @soldleads_7days = ISNULL(count(LeadId), 0)
			,@debit_7days = ISNULL(sum(BuyerPrice), 0)
			,@profit_7days = ISNULL(sum(BuyerPrice - AffiliatePrice), 0)
		FROM LeadMainReport
		WHERE CampaignType = 0
			AND Created BETWEEN @start
				AND @end
			AND Status = 1
			AND (
				@parentid = 0
				OR (
					@parentid > 0
					AND BuyerId = @parentid
					)
				OR (
					@parentid < 0
					AND AffiliateId = abs(@parentid)
					)
				);

		SELECT @rejectedleads_7days = ISNULL(count(LeadId), 0)
		FROM LeadMainReport
		WHERE CampaignType = 0
			AND Created BETWEEN @start
				AND @end
			AND Status = 3
			AND (
				@parentid = 0
				OR (
					@parentid > 0
					AND BuyerId = @parentid
					)
				OR (
					@parentid < 0
					AND AffiliateId = abs(@parentid)
					)
				);

		--select @credit_7days = ISNULL(sum(aprice), 0) from r_main where Created between @start and @end  and status = 1;	
		---- Cur month ----
		SET @start = DATEADD(month, DATEDIFF(month, 0, @start2), 0);
		SET @end = DATEADD(SECOND, - 1, DATEADD(MONTH, 1, DATEADD(MONTH, DATEDIFF(MONTH, 0, @end2), 0)))

		DECLARE @receivedleads_curmonth INT = 0;
		DECLARE @totalleads_curmonth INT = 0;
		DECLARE @soldleads_curmonth INT = 0;
		DECLARE @rejectedleads_curmonth INT = 0;
		DECLARE @debit_curmonth MONEY = 0;
		DECLARE @profit_curmonth MONEY = 0;

		SELECT @receivedleads_curmonth = ISNULL(count(DISTINCT LeadId), 0)
		FROM LeadMainReport
		WHERE CampaignType = 0
			AND Created BETWEEN @start
				AND @end
			AND (
				@parentid = 0
				OR (
					@parentid > 0
					AND BuyerId = @parentid
					)
				OR (
					@parentid < 0
					AND AffiliateId = abs(@parentid)
					)
				);

		SELECT @totalleads_curmonth = ISNULL(count(LeadId), 0)
		FROM LeadMainReport
		WHERE CampaignType = 0
			AND Created BETWEEN @start
				AND @end
			AND (
				@parentid = 0
				OR (
					@parentid > 0
					AND BuyerId = @parentid
					)
				OR (
					@parentid < 0
					AND AffiliateId = abs(@parentid)
					)
				);

		SELECT @soldleads_curmonth = ISNULL(count(LeadId), 0)
			,@debit_curmonth = ISNULL(sum(BuyerPrice), 0)
			,@profit_curmonth = ISNULL(sum(BuyerPrice - AffiliatePrice), 0)
		FROM LeadMainReport
		WHERE CampaignType = 0
			AND Created BETWEEN @start
				AND @end
			AND Status = 1
			AND (
				@parentid = 0
				OR (
					@parentid > 0
					AND BuyerId = @parentid
					)
				OR (
					@parentid < 0
					AND AffiliateId = abs(@parentid)
					)
				);

		SELECT @rejectedleads_curmonth = ISNULL(count(LeadId), 0)
		FROM LeadMainReport
		WHERE CampaignType = 0
			AND Created BETWEEN @start
				AND @end
			AND Status = 3
			AND (
				@parentid = 0
				OR (
					@parentid > 0
					AND BuyerId = @parentid
					)
				OR (
					@parentid < 0
					AND AffiliateId = abs(@parentid)
					)
				);

		---- Last month ----
		SET @start = DATEADD(MONTH, DATEDIFF(MONTH, 0, @start2) - 1, 0);
		SET @end = DATEADD(MONTH, DATEDIFF(MONTH, - 1, @end2) - 1, - 1);

		DECLARE @receivedleads_lastmonth INT = 0;
		DECLARE @totalleads_lastmonth INT = 0;
		DECLARE @soldleads_lastmonth INT = 0;
		DECLARE @rejectedleads_lastmonth INT = 0;
		DECLARE @debit_lastmonth MONEY = 0;
		DECLARE @profit_lastmonth MONEY = 0;

		SELECT @receivedleads_lastmonth = ISNULL(count(DISTINCT LeadId), 0)
		FROM LeadMainReport
		WHERE CampaignType = 0
			AND Created BETWEEN @start
				AND @end
			AND (
				@parentid = 0
				OR (
					@parentid > 0
					AND BuyerId = @parentid
					)
				OR (
					@parentid < 0
					AND AffiliateId = abs(@parentid)
					)
				);

		SELECT @totalleads_lastmonth = ISNULL(count(LeadId), 0)
		FROM LeadMainReport
		WHERE CampaignType = 0
			AND Created BETWEEN @start
				AND @end
			AND (
				@parentid = 0
				OR (
					@parentid > 0
					AND BuyerId = @parentid
					)
				OR (
					@parentid < 0
					AND AffiliateId = abs(@parentid)
					)
				);

		SELECT @soldleads_lastmonth = ISNULL(count(LeadId), 0)
			,@debit_lastmonth = ISNULL(sum(BuyerPrice), 0)
			,@profit_lastmonth = ISNULL(sum(BuyerPrice - AffiliatePrice), 0)
		FROM LeadMainReport
		WHERE CampaignType = 0
			AND Created BETWEEN @start
				AND @end
			AND Status = 1
			AND (
				@parentid = 0
				OR (
					@parentid > 0
					AND BuyerId = @parentid
					)
				OR (
					@parentid < 0
					AND AffiliateId = abs(@parentid)
					)
				);

		SELECT @rejectedleads_lastmonth = ISNULL(count(LeadId), 0)
		FROM LeadMainReport
		WHERE CampaignType = 0
			AND Created BETWEEN @start
				AND @end
			AND Status = 3
			AND (
				@parentid = 0
				OR (
					@parentid > 0
					AND BuyerId = @parentid
					)
				OR (
					@parentid < 0
					AND AffiliateId = abs(@parentid)
					)
				);

		---- Last 6 months ----
		SET @start = DATEADD(MONTH, - 6, @start2);
		SET @end = @end2;

		DECLARE @receivedleads_last6months INT = 0;
		DECLARE @totalleads_last6months INT = 0;
		DECLARE @soldleads_last6months INT = 0;
		DECLARE @rejectedleads_last6months INT = 0;
		DECLARE @debit_last6months MONEY = 0;
		DECLARE @profit_last6months MONEY = 0;

		SELECT @receivedleads_last6months = ISNULL(count(DISTINCT LeadId), 0)
		FROM LeadMainReport
		WHERE CampaignType = 0
			AND Created BETWEEN @start
				AND @end
			AND (
				@parentid = 0
				OR (
					@parentid > 0
					AND BuyerId = @parentid
					)
				OR (
					@parentid < 0
					AND AffiliateId = abs(@parentid)
					)
				);

		SELECT @totalleads_last6months = ISNULL(count(LeadId), 0)
		FROM LeadMainReport
		WHERE CampaignType = 0
			AND Created BETWEEN @start
				AND @end
			AND (
				@parentid = 0
				OR (
					@parentid > 0
					AND BuyerId = @parentid
					)
				OR (
					@parentid < 0
					AND AffiliateId = abs(@parentid)
					)
				);

		SELECT @soldleads_last6months = ISNULL(count(LeadId), 0)
			,@debit_last6months = ISNULL(sum(BuyerPrice), 0)
			,@profit_last6months = ISNULL(sum(BuyerPrice - AffiliatePrice), 0)
		FROM LeadMainReport
		WHERE CampaignType = 0
			AND Created BETWEEN @start
				AND @end
			AND Status = 1
			AND (
				@parentid = 0
				OR (
					@parentid > 0
					AND BuyerId = @parentid
					)
				OR (
					@parentid < 0
					AND AffiliateId = abs(@parentid)
					)
				);

		SELECT @rejectedleads_last6months = ISNULL(count(LeadId), 0)
		FROM LeadMainReport
		WHERE CampaignType = 0
			AND Created BETWEEN @start
				AND @end
			AND Status = 3
			AND (
				@parentid = 0
				OR (
					@parentid > 0
					AND BuyerId = @parentid
					)
				OR (
					@parentid < 0
					AND AffiliateId = abs(@parentid)
					)
				);

		---- Last year ----
		SET @start = DATEADD(yy, DATEDIFF(yy, 0, @start2) - 1, 0);
		SET @end = DATEADD(dd, - 1, DATEADD(yy, DATEDIFF(yy, 0, @end2), 0));

		DECLARE @receivedleads_lastyear INT = 0;
		DECLARE @totalleads_lastyear INT = 0;
		DECLARE @soldleads_lastyear INT = 0;
		DECLARE @rejectedleads_lastyear INT = 0;
		DECLARE @debit_lastyear MONEY = 0;
		DECLARE @profit_lastyear MONEY = 0;

		SELECT @receivedleads_lastyear = ISNULL(count(DISTINCT LeadId), 0)
		FROM LeadMainReport
		WHERE CampaignType = 0
			AND Created BETWEEN @start
				AND @end
			AND (
				@parentid = 0
				OR (
					@parentid > 0
					AND BuyerId = @parentid
					)
				OR (
					@parentid < 0
					AND AffiliateId = abs(@parentid)
					)
				);

		SELECT @totalleads_lastyear = ISNULL(count(LeadId), 0)
		FROM LeadMainReport
		WHERE CampaignType = 0
			AND Created BETWEEN @start
				AND @end
			AND (
				@parentid = 0
				OR (
					@parentid > 0
					AND BuyerId = @parentid
					)
				OR (
					@parentid < 0
					AND AffiliateId = abs(@parentid)
					)
				);

		SELECT @soldleads_lastyear = ISNULL(count(LeadId), 0)
			,@debit_lastyear = ISNULL(sum(BuyerPrice), 0)
			,@profit_lastyear = ISNULL(sum(BuyerPrice - AffiliatePrice), 0)
		FROM LeadMainReport
		WHERE CampaignType = 0
			AND Created BETWEEN @start
				AND @end
			AND Status = 1
			AND (
				@parentid = 0
				OR (
					@parentid > 0
					AND BuyerId = @parentid
					)
				OR (
					@parentid < 0
					AND AffiliateId = abs(@parentid)
					)
				);

		SELECT @rejectedleads_lastyear = ISNULL(count(LeadId), 0)
		FROM LeadMainReport
		WHERE CampaignType = 0
			AND Created BETWEEN @start
				AND @end
			AND Status = 3
			AND (
				@parentid = 0
				OR (
					@parentid > 0
					AND BuyerId = @parentid
					)
				OR (
					@parentid < 0
					AND AffiliateId = abs(@parentid)
					)
				);

		SELECT t.*
		FROM (
			SELECT 1 AS num
				,'Today' AS name
				,@debit_today AS debit
				,@receivedleads_today AS received
				,@soldleads_today AS sold
				,@rejectedleads_today AS rejected
		
			UNION
		
			SELECT 2 AS num
				,'Yesterday' AS name
				,@debit_yesterday AS debit
				,@receivedleads_yesterday AS received
				,@soldleads_yesterday AS sold
				,@rejectedleads_yesterday AS rejected
		
			UNION
		
			SELECT 3 AS num
				,'Last 7 days' AS name
				,@debit_7days AS debit
				,@receivedleads_7days AS received
				,@soldleads_7days AS sold
				,@rejectedleads_7days AS rejected
		
			UNION
		
			SELECT 4 AS num
				,'Current month' AS name
				,@debit_curmonth AS debit
				,@receivedleads_curmonth AS received
				,@soldleads_curmonth AS sold
				,@rejectedleads_curmonth AS rejected
		
			UNION
		
			SELECT 5 AS num
				,'Last month' AS name
				,@debit_lastmonth AS debit
				,@receivedleads_lastmonth AS received
				,@soldleads_lastmonth AS sold
				,@rejectedleads_lastmonth AS rejected
		
			UNION
		
			SELECT 6 AS num
				,'Last 6 months' AS name
				,@debit_last6months AS debit
				,@receivedleads_last6months AS received
				,@soldleads_last6months AS sold
				,@rejectedleads_last6months AS rejected
		
			UNION
		
			SELECT 7 AS num
				,'Last year' AS name
				,@debit_lastyear AS debit
				,@receivedleads_lastyear AS received
				,@soldleads_lastyear AS sold
				,@rejectedleads_lastyear AS rejected
		
			UNION
		
			SELECT 8 AS num
				,'All' AS name
				,@debit_today + @debit_yesterday + @debit_7days + @debit_curmonth + @debit_lastmonth + @debit_last6months + @debit_lastyear + @receivedleads_last6months + @receivedleads_lastyear AS debit
				,@receivedleads_today + @receivedleads_yesterday + @receivedleads_7days + @receivedleads_curmonth + @receivedleads_lastmonth + @rejectedleads_lastyear AS received
				,@soldleads_today + @soldleads_yesterday + @soldleads_7days + @soldleads_curmonth + @soldleads_lastmonth + @soldleads_last6months + @soldleads_last6months + @soldleads_lastyear AS sold
				,@rejectedleads_today + @rejectedleads_yesterday + @rejectedleads_7days + @rejectedleads_curmonth + @rejectedleads_lastmonth + @rejectedleads_last6months + @rejectedleads_lastyear AS rejected
			) t
		ORDER BY t.num
	END TRY
 
	BEGIN CATCH
		SELECT @SQLError_Severity = ERROR_SEVERITY(), 
		       @SQLError_State    = ERROR_STATE(), 
			   @SQLError_Message  = ERROR_MESSAGE()

		RAISERROR(@SQLError_Message, @SQLError_Severity, @SQLError_State)
	END CATCH
END