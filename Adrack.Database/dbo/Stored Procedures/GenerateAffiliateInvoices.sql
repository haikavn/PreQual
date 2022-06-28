
-- =============================================
-- Author:		<Author, Arman Zakaryan>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GenerateAffiliateInvoices] 
		@aID bigint,
		@dateFrom datetime,
		@dateTo datetime,
		@UserID bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
if (dbo.CheckStoredProcActive('GenerateAffiliateInvoices')> 0)
   return;

DECLARE @s float = 0;
DECLARE @insID int = 0;
DECLARE @maxDate datetime;
DECLARE @maxInvID int = 0;
DECLARE @refundSum float = 0;


set @maxDate = @dateFrom;


IF (@maxDate IS NULL)
	BEGIN
		SELECT @maxDate = MIN(Created)
		FROM [dbo].[LeadMainReport]
		WHERE AInvoiceId is null and AffiliateId = @aID AND [Status] = 1
	END

SELECT @refundSum = SUM(dbo.RefundedLeads.APrice)
FROM dbo.RefundedLeads
WHERE dbo.RefundedLeads.Approved = 1 AND dbo.RefundedLeads.AInvoiceId IS NULL

IF @refundSum IS NULL
BEGIN
SELECT @refundSum = 0;
END

SELECT @maxInvID = max(Number) from AffiliateInvoice where AffiliateId=@aID;
IF( @maxInvID is null )
	BEGIN
		SELECT @maxInvID = 0;
	END


SELECT @s = sum(AffiliatePrice)
FROM dbo.LeadMainReport
WHERE AInvoiceId is null AND AffiliateId=@aID AND created <= @dateTo AND status = 1;

declare @dateTo2 DateTime = @dateTo;

IF (@s > 0)
BEGIN
	declare @TimeZone int = 0;

	select @TimeZone = cast([Value] as int) from Setting where [Key] = 'TimeZone';

	if @TimeZone is null 
	begin
		set @TimeZone = 0;
	end;

	if @TimeZone < 0
	begin
		set @dateTo2 = dateadd(day, -1, @dateTo2);
	end;

	INSERT into dbo.AffiliateInvoice VALUES (@maxInvID+1, @maxDate, @dateTo2, GetutcDATE(), @aID, @s, @refundSum, 0, @UserID, 0, 0 )

	SELECT @insID = SCOPE_IDENTITY();

	UPDATE dbo.LeadMainReport
	SET AInvoiceId = @insID
	WHERE AInvoiceId IS NULL AND AffiliateId=@aID AND created <= @dateTo AND status = 1;
END
SELECT CAST(@insID AS bigint);

END