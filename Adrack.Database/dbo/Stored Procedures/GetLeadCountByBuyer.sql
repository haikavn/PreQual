-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetLeadCountByBuyer] 
	-- Add the parameters for the stored procedure here
	@created DateTime,
	@BuyerId bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

SELECT ISNULL(sum(Quantity), 0)
  FROM LeadMainReportDay where cast(Created as date) = cast(@created as date) and BuyerId = @BuyerId
END