-- =============================================
-- Author:		<Author,,Arman Zakaryan>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE CheckBuyerCredit
	-- Add the parameters for the stored procedure here
	@BuyerId bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

IF ( (SELECT Count(Id)
FROM BuyerBalance
WHERE BuyerId = @BuyerId AND ABS(Balance) <=  Credit) > 0 )
BEGIN
SELECT 1
END
ELSE
SELECT 0
	
END