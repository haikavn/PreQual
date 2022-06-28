-- =============================================
-- Author:		<Author: Arman Zakaryan>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SetDefaultPaymentMethod]
	@Id bigint,
	@AffiliateId bigint
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE PaymentMethod
	SET IsPrimary = 0
	WHERE AffiliateId = @AffiliateId;
	
	UPDATE PaymentMethod
	SET IsPrimary = 1
	WHERE Id = @Id;
	SELECT 1;
END