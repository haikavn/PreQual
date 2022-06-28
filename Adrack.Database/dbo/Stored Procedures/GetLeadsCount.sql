
-- =============================================
-- Author:		<Author,,Arman Zakaryan>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetLeadsCount]
	@dateFrom datetime,
	@dateTo datetime,
	@Email nvarchar(50) = '',
	@AffiliateId bigint = 0,
	@AffiliateChannelId bigint = 0,
	@BuyerId bigint = 0,
	@BuyerChannelId bigint = 0,
	@CampaignId bigint = 0,
	@Status smallint = -1,
	@IP nvarchar(20) = '',
	@State nvarchar(20) = '',
	@Notes nvarchar(MAX) = ''
AS
BEGIN
	SET NOCOUNT ON;

	IF @BuyerChannelId=0 and @BuyerId=0 and @State = '' AND @IP = '' and @Email='' and @Notes='' 
	BEGIN
	 SELECT count(LeadMain.Id) FROM LeadMain WHERE  (@Status = -1 OR LeadMain.Status = @Status) AND (LeadMain.Created BETWEEN @dateFrom AND @dateTo)
	 		AND (@AffiliateId = 0 OR LeadMain.AffiliateId = @AffiliateId)
		AND (@AffiliateChannelId = 0 OR LeadMain.AffiliateChannelId = @AffiliateChannelId)	
	END
	ELSE
	IF @BuyerChannelId=0 and @BuyerId=0 
	BEGIN
	SELECT /*distinct*/ count(LeadMain.Id)
		FROM LeadMain
		INNER JOIN LeadContent ON LeadMain.Id = LeadContent.LeadId 		
		LEFT JOIN RedirectUrl ON RedirectUrl.LeadId = LeadMain.Id AND RedirectUrl.Clicked=1
		WHERE  LeadMain.Created BETWEEN @dateFrom AND @dateTo
		AND (@Status = -1 OR LeadMain.Status = @Status)
		AND (@State = '' OR LeadContent.State like '%'+@State+'%')
		AND (@IP = '' OR LeadContent.Ip like '%'+@IP+'%')
		AND (@Email = '' OR LeadContent.Email like '%'+@Email+'%')

		AND (@AffiliateId = 0 OR LeadMain.AffiliateId = @AffiliateId)
		AND (@AffiliateChannelId = 0 OR LeadMain.AffiliateChannelId = @AffiliateChannelId)		
		
		AND (@CampaignId = 0 OR LeadMain.CampaignId = @CampaignId)
		AND (len(@Notes) = 0 or (len(@Notes) > 0 and LeadMain.Id in (select LeadId from LeadNote where Note like '%' + @Notes + '%' or NoteTitleId in (select Id from NoteTitle where Title like '%' + @Notes + '%'))))	
	END
	ELSE
	BEGIN

	SELECT /*distinct*/ count(LeadMain.Id)
		FROM LeadMain
		INNER JOIN LeadContent ON LeadMain.Id = LeadContent.LeadId 
		LEFT JOIN LeadMainResponse ON LeadMain.Id = LeadMainResponse.LeadId --AND ((@BuyerChannelId = 0 and @BuyerId = 0 and LeadMainResponse.Status=1) or ((@BuyerChannelId > 0 or @BuyerId > 0) and LeadMainResponse.Status<=3))
		LEFT JOIN RedirectUrl ON RedirectUrl.LeadId = LeadMain.Id AND RedirectUrl.Clicked=1
		WHERE  LeadMain.Created BETWEEN @dateFrom AND @dateTo

		AND (@Status = -1 OR LeadMain.Status = @Status)
		AND (@State = '' OR LeadContent.State like '%'+@State+'%')
		AND (@IP = '' OR LeadContent.Ip like '%'+@IP+'%')
		AND (@Email = '' OR LeadContent.Email like '%'+@Email+'%')

		AND (@AffiliateId = 0 OR LeadMain.AffiliateId = @AffiliateId)
		AND (@AffiliateChannelId = 0 OR LeadMain.AffiliateChannelId = @AffiliateChannelId)
		AND (@BuyerId = 0 OR LeadMainResponse.BuyerId = @BuyerId)
		AND (@BuyerChannelId = 0 OR LeadMainResponse.BuyerChannelId = @BuyerChannelId)
		AND (@CampaignId = 0 OR LeadMain.CampaignId = @CampaignId)
		AND (len(@Notes) = 0 or (len(@Notes) > 0 and LeadMain.Id in (select LeadId from LeadNote where Note like '%' + @Notes + '%' or NoteTitleId in (select Id from NoteTitle where Title like '%' + @Notes + '%'))))
	END
END