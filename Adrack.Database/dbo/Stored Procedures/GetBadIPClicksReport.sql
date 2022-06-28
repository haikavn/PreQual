-- =============================================
-- Author:		<Author: Arman Zakaryan>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetBadIPClicksReport]
	-- Add the parameters for the stored procedure here
	@dateFrom datetime,
	@dateTo datetime,
	@AffiliateId bigint = 0,
	@LeadIP nvarchar(20) = '',
	@ClickIP nvarchar(20) = '',
	@start int,
	@count int = 1,
	@leadId bigint = 0
AS

IF(@leadId = 0)
BEGIN
	SELECT LeadMain.Id AS LeadId, LeadMain.Created, LeadMain.AffiliateId, LeadContent.Ip AS LeadIp, RedirectUrl.Ip AS ClickIp
	FROM dbo.LeadMain
	INNER JOIN dbo.LeadContent ON LeadMain.Id = LeadContent.LeadId
	LEFT JOIN dbo.LeadMainResponse ON LeadMain.Id = LeadMainResponse.LeadId AND LeadMainResponse.Status=1
	INNER JOIN dbo.RedirectUrl ON RedirectUrl.LeadId = LeadMain.Id AND RedirectUrl.Clicked=1
	WHERE  LeadContent.Ip != RedirectUrl.Ip AND LeadMain.Created BETWEEN @dateFrom AND @dateTo

	AND (@LeadIP = '' OR LeadContent.Ip like '%'+@LeadIP+'%')
	AND (@ClickIP = '' OR RedirectUrl.Ip like '%'+@ClickIP+'%')
	AND (@AffiliateId = 0 OR LeadMain.AffiliateId = @AffiliateId)

	ORDER BY LeadMain.Created Desc
	OFFSET @start ROWS FETCH NEXT @count ROWS ONLY;
END
ELSE
BEGIN
	SELECT LeadMain.Id AS LeadId, LeadMain.Created, LeadMain.AffiliateId, LeadContent.Ip AS LeadIp, RedirectUrl.Ip AS ClickIp
	FROM dbo.LeadMain
	INNER JOIN dbo.LeadContent ON LeadMain.Id = LeadContent.LeadId
	LEFT JOIN dbo.LeadMainResponse ON LeadMain.Id = LeadMainResponse.LeadId AND LeadMainResponse.Status=1
	INNER JOIN dbo.RedirectUrl ON RedirectUrl.LeadId = LeadMain.Id AND RedirectUrl.Clicked=1 
	WHERE LeadContent.Ip != RedirectUrl.Ip AND LeadMain.Id=@leadId

END