
-- =============================================
-- Developer: Arman Zakaryan
-- Revised By: Zarzand Papikyan
-- Description:	Get Leads
-- ---------------------------------------------
-- Execute: EXECUTE [dbo].[usp_get_leads]
-- =============================================

CREATE PROCEDURE [dbo].[usp_get_leads]
	@DateFrom DATETIME = NULL,
	@DateTo   DATETIME = NULL

AS
BEGIN
-- =================================================================================================
-- SQL Script Setup
-- =================================================================================================
	SET NOCOUNT ON

-- =================================================================================================
-- SQL Common
-- =================================================================================================
	DECLARE @SQLCommand         NVARCHAR(4000)
	----------------------------------------
	DECLARE @SQLError_Severity INT
	DECLARE @SQLError_State    INT
	DECLARE @SQLError_Message  NVARCHAR(2000)
	----------------------------------------

-- =================================================================================================
-- Drop SQL Temp Tables
-- =================================================================================================


-- =================================================================================================
-- SQL Temp Tables
-- =================================================================================================


-- =================================================================================================
-- Validation
-- =================================================================================================
	IF (@DateFrom IS NULL)
		BEGIN
			SET @DateFrom = CONVERT(DATE, DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()), 0))
		END

	IF(@DateTo IS NULL)
		BEGIN
			SET @DateTo = CONVERT(DATE, DATEADD(MILLISECOND, -3, DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()) + 1, 0)))
		END

-- =================================================================================================
-- Process Script
-- =================================================================================================
 	BEGIN TRY
		--------- Build The Report --------
		SELECT a.[Id],
			   a.[Created],
			   a.[CampaignId],
			   a.[AffiliateId],
			   a.[Status],
			   a.[AffiliateChannelId],
			   a.[CampaignType],
			   a.[LeadNumber],
			   a.[Warning],
			   a.[ProcessingTime],
			   a.[DublicateLeadId],
			   a.[ReceivedData],
			   a.[BuyerChannelId],
			   b.[Id],
			   b.[LeadId],
			   b.[Ip],
			   b.[Minprice],
			   b.[Firstname],
			   b.[Lastname],
			   b.[Address],
			   b.[City],
			   b.[State],
			   b.[Zip],
			   b.[HomePhone],
			   b.[CellPhone],
			   b.[Email],
			   b.[PayFrequency],
			   b.[Directdeposit],
			   b.[AccountType],
			   b.[IncomeType],
			   b.[NetMonthlyIncome],
			   b.[Emptime],
			   b.[AddressMonth],
			   b.[Dob],
			   b.[Age],
			   b.[RequestedAmount],
			   b.[Ssn],
			   b.[String1],
			   b.[String2],
			   b.[String3],
			   b.[String4],
			   b.[String5],
			   b.[Created],
			   b.[AffiliateId],
			   b.[CampaignType],
			   c.[Id],
			   c.[BuyerId],
			   c.[BuyerChannelId],
			   c.[Response],
			   c.[ResponseTime],
			   c.[LeadId],
			   c.[AffiliateChannelId],
			   c.[AffiliateId],
			   c.[CampaignId],
			   c.[ResponseError],
			   c.[Status],
			   c.[AffiliatePrice],
			   c.[BuyerPrice],
			   c.[CampaignType],
			   c.[State],
			   c.[Created]
		FROM [dbo].[LeadMain] a WITH(NOLOCK)
			INNER JOIN [dbo].[LeadContent] b WITH(NOLOCK)
				ON a.Id = b.[LeadId]
			LEFT JOIN [dbo].[LeadMainResponse] c WITH(NOLOCK)
				ON a.[Id] = c.[LeadId]
					AND c.[Status] = 1
		WHERE a.[Created] BETWEEN @DateFrom AND @DateTo
	END TRY

	BEGIN CATCH
		SELECT @SQLError_Severity = ERROR_SEVERITY(), 
		       @SQLError_State    = ERROR_STATE(), 
			   @SQLError_Message  = ERROR_MESSAGE()
		RAISERROR(@SQLError_Message, @SQLError_Severity, @SQLError_State)
	END CATCH
END