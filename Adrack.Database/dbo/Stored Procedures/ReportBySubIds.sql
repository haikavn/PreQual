-- =============================================
-- Company: Adrack.com
-- ---------------------------------------------
-- Developer: Zarzand Papikyan
-- Description:	Buyer Report By Buyer Channels
-- ---------------------------------------------
-- Execute: EXECUTE [dbo].[BuyerReportByBuyerChannels]
-- =============================================

create PROCEDURE [dbo].[ReportBySubIds]
    @start  DATETIME,
	@end    DATETIME,
	@SubId	varchar(200)
AS
BEGIN
	select SubId, [Status], sum(Quantity) as Quantity from LeadMainReportDaySubIds where Created between @start and @end and SubId = @SubId group by SubId, [Status]; 
END