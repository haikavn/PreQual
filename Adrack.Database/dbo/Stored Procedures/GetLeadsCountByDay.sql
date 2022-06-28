-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetLeadsCountByDay]
	-- Add the parameters for the stored procedure here
	@buyerchannelid bigint,
	@datetime DateTime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	--SELECT sum(Quantity) from LeadMainReportDay where BuyerChannelId = @buyerchannelid and Created = @datetime
	SELECT ISNULL(sum(Quantity),0) from LeadMainReportDay where BuyerChannelId = @buyerchannelid and Created = @datetime


END