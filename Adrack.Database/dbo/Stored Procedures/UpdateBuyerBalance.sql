-- =============================================
-- Author:		<Author,,Arman Zakaryan>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UpdateBuyerBalance]
	-- Add the parameters for the stored procedure here
	@BuyerId bigint,
	@Sum money
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE BuyerBalance
	SET SoldSum = SoldSum + @Sum, Balance = PaymentSum + Credit - SoldSum - @Sum
	WHERE BuyerId = @BuyerId;
END