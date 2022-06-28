-- =============================================
-- Author:		<Author,,Arman Zakaryan>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetLeads]
	-- Add the parameters for the stored procedure here
	@dateFrom datetime,
	@dateTo datetime,
	@Email nvarchar(50) = '',
	@AffiliateId bigint = 0,
	@AffiliateChannelId bigint = 0,
	@AffiliateChannelSubId nvarchar(50) = '',
	@BuyerId bigint = 0,
	@BuyerChannelId bigint = 0,
	@CampaignId bigint = 0,
	@Status smallint = -1,
	@IP nvarchar(20) = '',
	@FirstName nvarchar(50) = '',
	@LastName nvarchar(50) = '',
	@BPrice smallmoney = 0,
	@ZipCode nvarchar(50) = '',
	@State nvarchar(20) = '',
	@start int,
	@count int = 1,
	@leadId bigint = 0,
	@Notes nvarchar(MAX) = ''
AS
BEGIN
	SET NOCOUNT ON;
		SELECT /*distinct*/ LeadMain.*, LeadContent.*, LeadMainResponse.BuyerId, LeadMainResponse.BuyerChannelId, RedirectUrl.Clicked, RedirectUrl.Ip, LeadMainResponse.AffiliatePrice, LeadMainResponse.BuyerPrice  FROM LeadMain
		INNER JOIN LeadContent ON LeadMain.Id = LeadContent.LeadId 
		LEFT JOIN LeadMainResponse ON LeadMain.Id = LeadMainResponse.LeadId AND ((@BuyerChannelId = 0 and @BuyerId = 0 and LeadMainResponse.Status=1) or ((@BuyerChannelId > 0 or @BuyerId > 0) and LeadMainResponse.Status<=3))
		LEFT JOIN RedirectUrl ON RedirectUrl.LeadId = LeadMain.Id AND RedirectUrl.Clicked=1
		WHERE  LeadMain.Created BETWEEN @dateFrom AND @dateTo

		AND (@leadId = 0 or LeadMain.Id = @leadId)
		AND (@Status = -1 OR LeadMain.Status = @Status)
		AND (@State = '' OR LeadContent.State like '%'+@State+'%')
		AND (@IP = '' OR LeadContent.Ip like '%'+@IP+'%')
		AND (@Email = '' OR LeadContent.Email like '%'+@Email+'%')

		AND (@FirstName = '' OR LeadContent.Firstname like '%'+@FirstName+'%')
		AND (@LastName = '' OR LeadContent.Lastname like '%'+@LastName+'%')
		AND (@BPrice = 0 OR LeadMainResponse.BuyerPrice = @BPrice)
		AND (@ZipCode = '' OR LeadContent.Zip like '%'+@ZipCode+'%')

		AND (@AffiliateId = 0 OR LeadMain.AffiliateId = @AffiliateId)
		AND (@AffiliateChannelId = 0 OR LeadMain.AffiliateChannelId = @AffiliateChannelId)
		AND (@AffiliateChannelSubId = '' OR LeadContent.AffiliateSubId like '%'+@AffiliateChannelSubId+'%')
		AND (@BuyerId = 0 OR LeadMainResponse.BuyerId = @BuyerId)
		AND (@BuyerChannelId = 0 OR LeadMainResponse.BuyerChannelId = @BuyerChannelId)
		AND (@CampaignId = 0 OR LeadMain.CampaignId = @CampaignId)
		AND (len(@Notes) = 0 or (len(@Notes) > 0 and LeadMain.Id in (select LeadId from LeadNote where Note like '%' + @Notes + '%' or NoteTitleId in (select Id from NoteTitle where Title like '%' + @Notes + '%'))))

		ORDER BY LeadMain.Created Desc
		OFFSET @start ROWS FETCH NEXT @count ROWS ONLY;
END