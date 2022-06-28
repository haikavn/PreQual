
-- =============================================
-- Developer: Zarzand Papikyan
-- Description:	Report Totals By Date
-- ---------------------------------------------
-- Execute: EXECUTE [dbo].[ReportTotalsByDate]
-- =============================================
CREATE PROCEDURE [dbo].[ReportTotalsByDate]
	 @start date
	,@end date
	,@CampaignId BIGINT
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
	DECLARE @receivedleads INT = 0;
	DECLARE @postedleads INT = 0;
	DECLARE @totalleads INT = 0;
	DECLARE @soldleads INT = 0;
	DECLARE @loanedleads INT = 0;
	DECLARE @bprice MONEY = 0;
	DECLARE @aprice MONEY = 0;
	DECLARE @profit MONEY = 0;

-- =================================================================================================
-- Process Script
-- =================================================================================================
 	BEGIN TRY
		
		SELECT @receivedleads = ISNULL(sum(Quantity), 0)
		FROM LeadMainReportDayReceived
		WHERE CampaignType = 0
			AND Created BETWEEN @start
				AND @end
			AND (
				@CampaignId = 0
				OR (
					@CampaignId > 0
					AND CampaignId = @CampaignId
					)
				)
			AND (
				@parentid = 0
				OR (
					@parentid < 0
					AND AffiliateId = abs(@parentid)
					)
				);

		
		SELECT @postedleads = ISNULL(sum(Quantity), 0)
		FROM LeadMainReportDay
		WHERE CampaignType = 0
			AND Created BETWEEN @start
				AND @end
			AND (
				@CampaignId = 0
				OR (
					@CampaignId > 0
					AND CampaignId = @CampaignId
					)
				)
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

		--select @totalleads = ISNULL(count(LeadId), 0) from LeadMainReport where CampaignType = 0 and Created between @start and @end and (@CampaignId = 0 or (@CampaignId > 0 and CampaignId = @CampaignId)) and (@parentid = 0 or (@parentid > 0 and BuyerId = @parentid) or (@parentid < 0 and AffiliateId = abs(@parentid))) ;
		SELECT @totalleads = ISNULL(sum(Quantity), 0)
		FROM LeadMainReportDay
		WHERE CampaignType = 0
			AND Created BETWEEN @start
				AND @end
			AND (
				@CampaignId = 0
				OR (
					@CampaignId > 0
					AND CampaignId = @CampaignId
					)
				)
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

		--select @soldleads = ISNULL(count(LeadId), 0), @bprice = ISNULL(sum(BuyerPrice), 0), @aprice = ISNULL(sum(AffiliatePrice), 0), @profit = ISNULL(sum(BuyerPrice - AffiliatePrice), 0) from LeadMainReport where CampaignType = 0 and Created between @start and @end and (@CampaignId = 0 or (@CampaignId > 0 and CampaignId = @CampaignId)) and status = 1 and (@parentid = 0 or (@parentid > 0 and BuyerId = @parentid) or (@parentid < 0 and AffiliateId = abs(@parentid)));
		SELECT @soldleads = ISNULL(sum(Quantity), 0)
			,@bprice = ISNULL(sum(BuyerPrice), 0)
			,@aprice = ISNULL(sum(AffiliatePrice), 0)
			,@profit = ISNULL(sum(BuyerPrice - AffiliatePrice), 0)
		FROM LeadMainReportDay
		WHERE CampaignType = 0
			AND Created BETWEEN @start
				AND @end
			AND (
				@CampaignId = 0
				OR (
					@CampaignId > 0
					AND CampaignId = @CampaignId
					)
				)
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

		SELECT @loanedleads = ISNULL(count(Id), 0)
		FROM LeadNote
		WHERE NoteTitleId = 1
			AND LeadId IN (
				SELECT LeadId
				FROM LeadMainReport
				WHERE CampaignType = 0
					AND Created BETWEEN @start
						AND @end
					AND (
						@CampaignId = 0
						OR (
							@CampaignId > 0
							AND CampaignId = @CampaignId
							)
						)
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
						)
				)

		--select @credit_today = ISNULL(sum(aprice), 0) from r_main where Created between @start and @now  and status = 1;
		SELECT @receivedleads AS received
			,@postedleads AS posted
			,@totalleads AS total
			,@soldleads AS sold
			,@loanedleads AS loaned
			,@aprice AS aprice
			,@bprice AS bprice
			,@profit AS profit;

	END TRY
 
	BEGIN CATCH
		SELECT @SQLError_Severity = ERROR_SEVERITY(), 
		       @SQLError_State    = ERROR_STATE(), 
			   @SQLError_Message  = ERROR_MESSAGE()

		RAISERROR(@SQLError_Message, @SQLError_Severity, @SQLError_State)
	END CATCH
END