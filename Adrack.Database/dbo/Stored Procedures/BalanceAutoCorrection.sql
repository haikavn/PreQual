
-- =============================================
-- Author:		<Author,Arman Zakaryan>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[BalanceAutoCorrection]
	@AutoCorrect int = 0

AS
BEGIN
	SET NOCOUNT ON;

DECLARE @BUYER_ID bigint;
DECLARE @SoldSumReal money;
DECLARE @SoldSumBalance money;
DECLARE @SoldSumError int;

DECLARE @PaymentAmountReal money;
DECLARE @PaymentAmountBalance money;
DECLARE @PaymentAmountError int;;

DECLARE Buyer_Cursor CURSOR FOR  
SELECT Id
FROM Buyer

OPEN Buyer_Cursor;  
FETCH NEXT FROM Buyer_Cursor into @BUYER_ID;  

WHILE @@FETCH_STATUS = 0  
	BEGIN
		
		SET @SoldSumReal = 0;
		SET @SoldSumBalance = 0;
		SET @SoldSumError = 0;

		SET @PaymentAmountReal = 0;
		SET @PaymentAmountBalance = 0;
		SET @PaymentAmountError = 0;
      
/* Payment */
		SELECT @PaymentAmountReal = SUM([dbo].[BuyerPayment].Amount), @PaymentAmountBalance = [BuyerBalance].PaymentSum, @PaymentAmountError = IIF([BuyerBalance].PaymentSum = SUM([dbo].[BuyerPayment].Amount), 0, 1)
		FROM [dbo].[BuyerBalance] 
		INNER JOIN [dbo].[BuyerPayment] ON [dbo].[BuyerBalance].BuyerId = [dbo].[BuyerPayment].BuyerId
		WHERE [BuyerBalance].BuyerId = @BUYER_ID
		GROUP BY [BuyerBalance].BuyerId, [BuyerBalance].PaymentSum;

		SELECT @BUYER_ID AS BuyerId, @PaymentAmountReal AS PaymentAmountReal, @PaymentAmountBalance AS PaymentAmountBalance, IIF(@PaymentAmountReal = @PaymentAmountBalance, 0, 1) AS PaymentError;

/* Sold */
		SELECT @SoldSumReal = SUM([dbo].[LeadMainReportDay].BuyerPrice)
		FROM [dbo].[LeadMainReportDay]
		WHERE [dbo].[LeadMainReportDay].BuyerId = @BUYER_ID AND [dbo].[LeadMainReportDay].[Status] = 1
	
		SELECT @SoldSumBalance = [dbo].[BuyerBalance].[SoldSum]
		FROM [dbo].[BuyerBalance]
		WHERE [dbo].[BuyerBalance].BuyerId = @BUYER_ID

		SELECT @BUYER_ID AS BuyerId, @SoldSumReal, @SoldSumBalance,  IIF(@SoldSumReal = @SoldSumBalance, 0, 1) AS SoldSumError;

/* Auto Correction */
	IF(@AutoCorrect = 1)
	BEGIN
		IF(@SoldSumReal != @SoldSumBalance)
		BEGIN
			UPDATE [dbo].[BuyerBalance]
			SET [dbo].[BuyerBalance].[SoldSum] = @SoldSumReal
			WHERE [dbo].[BuyerBalance].BuyerId = @BUYER_ID
		END

		IF(@PaymentAmountReal != @PaymentAmountBalance)
		BEGIN
			UPDATE [dbo].[BuyerBalance]
			SET [dbo].[BuyerBalance].[PaymentSum] = @PaymentAmountReal
			WHERE [dbo].[BuyerBalance].BuyerId = @BUYER_ID
		END

	END

	FETCH NEXT FROM Buyer_Cursor into @BUYER_ID
END;  
CLOSE Buyer_Cursor;  
DEALLOCATE Buyer_Cursor;  
  
	

END