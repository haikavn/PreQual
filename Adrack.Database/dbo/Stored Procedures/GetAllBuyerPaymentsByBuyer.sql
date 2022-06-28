

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetAllBuyerPaymentsByBuyer]
	@BuyerId bigint,
	@DateFrom datetime,
	@DateTo datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT BuyerPayment.[Id], BuyerPayment.[BuyerId], BuyerPayment.[PaymentDate], BuyerPayment.[Amount], BuyerPayment.[Note], BuyerPayment.[Created], BuyerPayment.[UserId], BuyerPayment.[PaymentMethod], Buyer.[Name] AS BuyerName, [User].Username AS UserName
	FROM dbo.BuyerPayment
	INNER JOIN dbo.Buyer on BuyerPayment.BuyerId=Buyer.Id
	LEFT JOIN dbo.[User] on BuyerPayment.UserId = [User].Id
	WHERE BuyerPayment.BuyerId = @BuyerId AND ((@DateFrom IS NULL AND @DateTo IS NULL) OR BuyerPayment.Created between @DateFrom AND @DateTo)
	ORDER BY BuyerPayment.Id DESC
END