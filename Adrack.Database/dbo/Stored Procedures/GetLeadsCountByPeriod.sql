-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetLeadsCountByPeriod]
	-- Add the parameters for the stored procedure here
	@buyerchannelid bigint,
	@from DateTime,
	@to DateTime,
	@status smallint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	--SELECT ISNULL(sum(Quantity), 0) from LeadMainReportDayHour where BuyerChannelId = @buyerchannelid and Created between cast(@from as date) and cast(@to as date) and [Hour] >= DATEPART(hour, @from) and [Hour] <= DATEPART(hour, @to) and Status != 7 and Status != 5 and (@status = -1 or (@status != -1 and Status = @status));
	SELECT ISNULL(sum(Quantity), 0) from LeadMainReportDayHour where BuyerChannelId = @buyerchannelid and (DATEADD(hour, LeadMainReportDayHour.Hour,CAST(LeadMainReportDayHour.Created AS datetime)) ) between cast(@from as datetime) and cast(@to as datetime) and Status != 7 and Status != 5 and (@status = -1 or (@status != -1 and Status = @status));
END